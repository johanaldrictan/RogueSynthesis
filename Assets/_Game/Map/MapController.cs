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

    //integers that store the maximal/minimal values for each direction
    public int mostWest;
    public int mostEast;
    public int mostNorth;
    public int mostSouth;

    private void Awake()
    {
        weightedMap = new Dictionary<Vector2Int, int>();
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
                if (walkableTiles.GetTile(new Vector3Int(neighbor.x, neighbor.y, 0)) && !visited.Contains(neighbor) && !MapMath.InMapBounds(neighbor))
                {
                    frontier.Enqueue(neighbor);
                    if (walkableTiles.GetTile(new Vector3Int(neighbor.x, neighbor.y, 0)) is WeightedTile)
                    {
                        //Debug.Log(neighbor);
                        weightedMap.Add(neighbor, (int)(walkableTiles.GetTile(new Vector3Int(neighbor.x, neighbor.y, 0)) as WeightedTile).weight);
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
}

