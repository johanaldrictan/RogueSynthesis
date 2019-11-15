﻿using System.Collections;
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
                    if (!visited.Contains(neighbor) && !MapMath.InMapBounds(neighbor))
                    {
                        frontier.Enqueue(neighbor);
                        if (walkableTiles.GetTile(new Vector3Int(neighbor.x, neighbor.y, 0)) is WeightedTile)
                        {
                            //Debug.Log(neighbor);
                            weightedMap.Add(neighbor, (int)(walkableTiles.GetTile(new Vector3Int(neighbor.x, neighbor.y, 0)) as WeightedTile).weight);
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
        if (weightedMap.Count <= 0)
            return;

        scannedMap.Clear();
        HashSet<Vector2Int> toVisit = new HashSet<Vector2Int>(weightedMap.Keys);
        recursiveScan(new Vector2Int(0, 0), new HashSet<Vector2Int>(), toVisit, new Queue<Vector2Int>(), 0);
    }

    private void recursiveScan(Vector2Int current, HashSet<Vector2Int> visited, HashSet<Vector2Int> toVisit, Queue<Vector2Int> path, int movement)
    {

    }
}

