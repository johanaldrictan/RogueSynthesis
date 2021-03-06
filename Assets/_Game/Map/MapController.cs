﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    public static MapController instance;
    public Grid grid;
    public Tilemap walkableTiles;

    //2D grid. value is the weight of the tile
    //public int[,] map;
    public Dictionary<Vector2Int, int> weightedMap;

    /// <summary>
    /// THIS SPECIFIC VARIABLE IS NOT CURRENTLY BEING USED. PIECES OF IT ARE GENERATED DYNAMICALLY WHEN NEEDED
    /// 
    /// scannedMap represents the entire map, scanned to find the distances between all points
    /// it should be created once, and only once, for each map.
    /// This object's existence, once created, makes pathfinding, bounds-checking, etc. more or less constant time
    /// 
    /// Its data structure is a Dictionary
    /// The keys represent a specific tile on the map, as a starting point.
    /// the value represents a Dictionary, with each key in that list representing a movement amount
    /// At each index will be a value of Dictionary representing all tiles that can be reached from the start using the given amount of movement.
    /// It's keys represent a specific tile on the map, as an ending point.
    /// it's values are Queues representing the shortest pathway that was taken to reach that tile
    /// 
    /// For example: scannedMap[(0,0)][3][(3, 0)]
    /// the starting tile is (0, 0), and returns the Queue shortest path in order to reach (3, 0) using exactly 3 movement
    /// </summary>
    private Dictionary<Vector2Int, Dictionary<int, Dictionary<Vector2Int, Queue<Vector2Int>>>> scannedMap;

    //integers that store the maximal/minimal values for each direction
    public int mostWest;
    public int mostEast;
    public int mostNorth;
    public int mostSouth;

    private void Awake()
    {
        weightedMap = new Dictionary<Vector2Int, int>();
        scannedMap = new Dictionary<Vector2Int, Dictionary<int, Dictionary<Vector2Int, Queue<Vector2Int>>>>();
        //each scene load I want a new instance of the mapcontroller but it needs to stay static
        //needs to load start again in each scene
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        InitMap();
        // ScanMap();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void InitMap()
    {
        mostWest = 0;
        mostEast = 0;
        mostNorth = 0;
        mostSouth = 0;
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Queue<Vector2Int> frontier = new Queue<Vector2Int>();
        Vector2Int origin = new Vector2Int(0, 0);
        frontier.Enqueue(origin); // Should only contain tiles in range
        weightedMap.Add(origin, (int)(walkableTiles.GetTile(new Vector3Int(origin.x, origin.y, 0)) as WeightedTile).weight);

        while (frontier.Count != 0)
        {
            Vector2Int visiting = frontier.Dequeue();
            if (visited.Contains(visiting)) { continue; } // TODO: Implement changing priority in the PQ, and remove this.
            mostEast = Mathf.Max(mostEast, visiting.y);
            mostWest = Mathf.Min(mostWest, visiting.y);
            mostNorth = Mathf.Max(mostNorth, visiting.x);
            mostSouth = Mathf.Min(mostSouth, visiting.x);

            Dictionary<Vector2Int, Direction> neighbors = MapMath.GetNeighbors(visiting);
            foreach (Vector2Int neighbor in neighbors.Keys)
            {
                //check if there are tiles in that location, check if its not in weightedMap dict
                if (walkableTiles.GetTile(new Vector3Int(neighbor.x, neighbor.y, 0)))
                {
                    if (!visited.Contains(neighbor) && !MapMath.InMapBounds(MapMath.GridToMap(neighbor)))
                    {
                        frontier.Enqueue(neighbor);
                        if (walkableTiles.GetTile(new Vector3Int(neighbor.x, neighbor.y, 0)) is WeightedTile)
                        {
                            //Debug.Log(neighbor);
                            weightedMap.Add(MapMath.GridToMap(neighbor), (int)(walkableTiles.GetTile(new Vector3Int(neighbor.x, neighbor.y, 0)) as WeightedTile).weight);
                        }
                    }
                }
            }

            visited.Add(visiting);
        }
        /*
        Debug.Log("most east: " + mostEast);
        Debug.Log("most west: " + mostWest);
        Debug.Log("most south: " + mostSouth);
        Debug.Log("most north: " + mostNorth);
        */
    }


    /// <summary>
    ///  Scans the map and creates scannedMap
    /// </summary>
    private void ScanMap()
    {
        scannedMap.Clear();
        // for every tile on the map:
        foreach (Vector2Int key in weightedMap.Keys)
        {
            // starting from this tile, calculate the shortest distance to every other tile
           scannedMap[key] = ScanFromStart(key);
        }
    }


    /// <summary>
    /// Takes a single tile on the map and calculates all paths to every other tile, starting from that initial tile.
    /// returns those pathways as a Dictionary
    /// it's keys are ints representing the distance required to travel
    /// its values are dictionaries with keys representing the destination and values representing the Queue path to follow
    /// </summary>
    /// <param name="start"> (x, y) coordinate to start from</param>
    public Dictionary<int, Dictionary<Vector2Int, Queue<Vector2Int>>> ScanFromStart(Vector2Int start)
    {
        // create two data structures: tiles that need to be visited and tiles already visited
        // visited is just a set of Vector2Int
        // toVisit is a queue, storing the tile to visit, the path taken to get there, and the total movement spent to reach the tile
        HashSet <Vector2Int> visited = new HashSet<Vector2Int>();
        Queue<MutableTuple<Vector2Int, MutableTuple<Queue<Vector2Int>, int>>> toVisit = new Queue<MutableTuple<Vector2Int, MutableTuple<Queue<Vector2Int>, int>>>();

        Dictionary<int, Dictionary<Vector2Int, Queue<Vector2Int>>> result = new Dictionary<int, Dictionary<Vector2Int, Queue<Vector2Int>>>();
        toVisit.Enqueue(new MutableTuple<Vector2Int, MutableTuple<Queue<Vector2Int>, int>>(start, new MutableTuple<Queue<Vector2Int>, int>(new Queue<Vector2Int>(), 0)));

        while (toVisit.Count != 0)
        {
            // get the next tile in line to visit
            MutableTuple<Vector2Int, MutableTuple<Queue<Vector2Int>, int>> current = toVisit.Dequeue();

            // make sure that this tile is valid to be visited. an Invalid tile:
            //      has already been visited            does not exist within weightedMap         or has a weight of OBSTRUCTED AND is not the starting tile
            if (visited.Contains(current.Item1) || (!weightedMap.ContainsKey(current.Item1)) || (weightedMap[current.Item1] == (int)TileWeight.OBSTRUCTED && current.Item1 != start))
                continue;

            // this is the first time that we've reached a tile this much movement away. set it up.
            if (!result.ContainsKey(current.Item2.Item2))
            {
                result.Add(current.Item2.Item2, new Dictionary<Vector2Int, Queue<Vector2Int>>());
            }

            // This is the first time that we've reached this tile with this amount of movement. it is the shortest path. store it
            if (!result[current.Item2.Item2].ContainsKey(current.Item1))
            {
                result[current.Item2.Item2].Add(current.Item1, current.Item2.Item1);
            }

            // check each neighbor of this tile
            Dictionary<Vector2Int, Direction> neighbors = MapMath.GetNeighbors(current.Item1);
            foreach (Vector2Int neighbor in neighbors.Keys)
            {
                int movement = current.Item2.Item2;
                if (weightedMap.ContainsKey(neighbor))
                    movement += weightedMap[neighbor];

                Queue<Vector2Int> newPath = new Queue<Vector2Int>(current.Item2.Item1.ToArray());
                newPath.Enqueue(neighbor);
                // Debug.Log("Need to visit Tile " + neighbor);
                toVisit.Enqueue(new MutableTuple<Vector2Int, MutableTuple<Queue<Vector2Int>, int>>(neighbor, new MutableTuple<Queue<Vector2Int>, int>(newPath, movement)));
            }

            // this tile has been officially 'visited'
            visited.Add(current.Item1);
        }

        return result;
    }

    /// <summary>
    /// Gets the shortest pathway from the start tile to the end tile
    /// </summary>
    /// <remarks>
    /// O(n) time, where n is the movement required to move between start and end
    /// </remarks>
    /// <param name="map"> map representing all shortest paths from starting point</param>
    /// <param name="end"> (x, y) coordinate to end at</param>
    /// <returns> Shortest pathway from start to end; null if nonexistent or invalid argumnts</returns>
    public Tuple<Queue<Vector2Int>, int> GetShortestPath(Dictionary<int, Dictionary<Vector2Int, Queue<Vector2Int>>> map, Vector2Int end)
    {
        int foundIndexes = 0;
        for (int i = 0; i < int.MaxValue; i++)
        {
            if (map.ContainsKey(i))
            {
                if (map[i].ContainsKey(end))
                {
                    return new Tuple<Queue<Vector2Int>, int>(map[i][end], i);
                }
                else
                {
                    foundIndexes += 1;
                    if (foundIndexes == map.Keys.Count)
                    {
                        return null;
                    }
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Gets the smallest amount of movement required to move from start to end
    /// </summary>
    /// <remarks>
    /// O(n) time, where n is the movement required to move between start and end
    /// </remarks>
    /// <param name="map"> map representing all shortest paths from start</param>
    /// <param name="end"> (x, y) coordinate to end at</param>
    /// <returns> Smallest movement required; -1 if nonexistent or invalid argumnts</returns>
    public int GetDistance(Dictionary<int, Dictionary<Vector2Int, Queue<Vector2Int>>> map, Vector2Int end)
    {
        int foundIndexes = 0;
        for (int i = 0; i < int.MaxValue; i++)
        {
            if (map.ContainsKey(i))
            {
                if (map[i].ContainsKey(end))
                {
                    return i;
                }
                else
                {
                    foundIndexes += 1;
                    if (foundIndexes == map.Keys.Count)
                    {
                        return -1;
                    }
                }
            }
        }
        return -1;
    }
}

