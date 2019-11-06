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
    public int[,] map;

    //change origin. these offsets are the actual coordinates of the left corner tile
    public int mapHeightOffset;
    public int mapWidthOffset;

    public int mapWidth;
    public int mapHeight;

    private void Awake()
    {
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
        //This method of programmatically figuring out what the layout of the map is to ease the workload of designing maps
        //But this is not really optimal for load times??
        //initialize map
        //                x         y
        map = new int[mapWidth+1, mapHeight+1];

        for (int x = mapWidthOffset; x < mapWidthOffset + mapWidth; x++)
        {
            //Debug.Log("MapHeightOffset: " + mapHeightOffset);
            //Debug.Log("MapWidthOffset: " + mapWidthOffset);
            //Debug.Log("map bound: " + (mapHeightOffset - mapHeight));
            //Debug.Log("X: " + x);           
            for (int y = mapHeightOffset; y > (mapHeightOffset - mapHeight); y--)
            {
                Vector2Int currCoords = MapMath.GridToMap(new Vector3Int(x, y, 0));
                //Debug.Log(currCoords.ToString());
                //Debug.Log("X: " + x + " Y: " + y);
                if (MapMath.InMapBounds(currCoords))
                {
                    map[currCoords.x, currCoords.y] = (int)TileWeight.OBSTRUCTED;
                    //null check
                    if (walkableTiles.GetTile(new Vector3Int(x, y, 0)))
                    {
                        // if(TileDatabase.instance.tileDB.ContainsKey(walkableTiles.GetTile(new Vector3Int(x, y, 0)).name))
                        if (walkableTiles.GetTile(new Vector3Int(x, y, 0)) is WeightedTile)
                        {
                            // map[currCoords.x, currCoords.y] = (int)TileDatabase.instance.tileDB[walkableTiles.GetTile(new Vector3Int(x, y, 0)).name];
                            map[currCoords.x, currCoords.y] = (int)(walkableTiles.GetTile(new Vector3Int(x, y, 0)) as WeightedTile).weight;
                        }
                    }
                }
            }

        }
    }
}

