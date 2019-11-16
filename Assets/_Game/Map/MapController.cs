using System;
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
        ScanMap();

        //Queue<Vector2Int> path = GetShortestPath(new Vector2Int(-8, -5), new Vector2Int(-8, 3));
        foreach(int tile in scannedMap[new Vector2Int(-8, -8)].Keys)
        {
            Debug.Log(tile);
            foreach(Vector2Int tiles in scannedMap[new Vector2Int(-8, -8)][tile].Keys)
            {
                Debug.Log(tiles);

            }
            // if (scannedMap[new Vector2Int(-8, -8)][tile].ContainsKey(new Vector2Int(-8, 3)))
            // Debug.Log(scannedMap[new Vector2Int(-8, -8)][tile][new Vector2Int(-8, 3)]);
            //else
            //Debug.Log(tile + " nah");
        }
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


    // this scans the map and creates scannedMap
    public void ScanMap()
    {
        scannedMap.Clear();
        ScanFromStart(new Vector2Int(-8, -8));
        /*foreach (Vector2Int key in weightedMap.Keys)
        {
            Debug.Log("Starting at " + key);
            ScanFromStart(key);
        }*/
            
    }


    // Takes a single tile on the map and calculates all paths to every other tile, starting from that initial tile
    private void ScanFromStart(Vector2Int start)
    {
        if (!weightedMap.ContainsKey(start))
            return;

        HashSet <Vector2Int> visited = new HashSet<Vector2Int>();
        Queue<MutableTuple<Vector2Int, MutableTuple<Queue<Vector2Int>, int>>> toVisit = new Queue<MutableTuple<Vector2Int, MutableTuple<Queue<Vector2Int>, int>>>();

        scannedMap.Add(start, new Dictionary<int, Dictionary<Vector2Int, Queue<Vector2Int>>>());
        toVisit.Enqueue(new MutableTuple<Vector2Int, MutableTuple<Queue<Vector2Int>, int>>(start, new MutableTuple<Queue<Vector2Int>, int>(new Queue<Vector2Int>(), 0)));

        while (toVisit.Count != 0)
        {
            MutableTuple<Vector2Int, MutableTuple<Queue<Vector2Int>, int>> current = toVisit.Dequeue();

            if (visited.Contains(current.Item1) || (!weightedMap.ContainsKey(current.Item1)) || weightedMap[current.Item1] == (int)TileWeight.OBSTRUCTED)
                continue;

            if (!scannedMap[start].ContainsKey(current.Item2.Item2))
            {
                scannedMap[start].Add(current.Item2.Item2, new Dictionary<Vector2Int, Queue<Vector2Int>>());
            }

            if (!scannedMap[start][current.Item2.Item2].ContainsKey(current.Item1))
            {
                scannedMap[start][current.Item2.Item2].Add(current.Item1, current.Item2.Item1);
            }

            Dictionary<Vector2Int, Direction> neighbors = MapMath.GetNeighbors(current.Item1);
            foreach (Vector2Int neighbor in neighbors.Keys)
            {
                
                int movement = current.Item2.Item2;
                if (weightedMap.ContainsKey(neighbor))
                    movement += weightedMap[neighbor];

                Queue<Vector2Int> newPath = new Queue<Vector2Int>(current.Item2.Item1.ToArray());
                newPath.Enqueue(neighbor);

                toVisit.Enqueue(new MutableTuple<Vector2Int, MutableTuple<Queue<Vector2Int>, int>>(neighbor, new MutableTuple<Queue<Vector2Int>, int>(newPath, movement)));
            }

            visited.Add(current.Item1);
            if (current.Item1.x == -8 && current.Item1.y == -5)
            {
                Debug.Log(current.Item1 + ": " + current.Item2.Item1.Count + ", " + current.Item2.Item2);
                Debug.Log(weightedMap[current.Item1]);
            }
        }
    }

    public Queue<Vector2Int> GetShortestPath(Vector2Int start, Vector2Int end)
    {
        if (!scannedMap.ContainsKey(start) || !scannedMap.ContainsKey(end))
            return null;

        for (int i = 0; i < int.MaxValue; i++)
        {
            if (scannedMap[start].ContainsKey(i) && scannedMap[start][i].ContainsKey(end))
            {
                return scannedMap[start][i][end];
            }
        }
        return null;
    }
}

