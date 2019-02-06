using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    public Grid grid;
    public Tilemap walkableTiles;

    //2D grid. value is the weight of the tile
    public int[,] map;

    //change origin. these offsets are the actual coordinates of the left corner tile
    public int mapHeightOffset;
    public int mapWidthOffset;

    public int mapWidth;
    public int mapHeight;

    // Start is called before the first frame update
    void Start()
    {
        //initialize map
        //                x         y
        map = new int[mapWidth, mapHeight];
        for(int x = mapWidthOffset; x < mapWidth + mapWidthOffset; x++)
        {
            //Debug.Log("X: " + x);
            
            for(int y = mapHeightOffset; y <= mapHeight - mapWidthOffset; y++)
            {
                Debug.Log("X: " + x + " Y: " + y);
                /*
                switch (walkableTiles.GetTile(new Vector3Int(x,y,0)))
                {
                    
                }
                */
            }
            
        }
    }

    public Vector2Int GridToMap(Vector3Int gridUnits)
    {
        Vector2Int converted = new Vector2Int();
        converted.x = gridUnits.x - mapWidthOffset;
        converted.y = (gridUnits.y - mapHeightOffset) * -1;
        return converted;
    }
    public Vector3Int MapToGrid(Vector2Int mapUnits)
    {
        Vector3Int converted = new Vector3Int();
        converted.x = mapUnits.x + mapWidthOffset;
        converted.y = (mapUnits.y - mapHeightOffset)* -1;
        return converted;
    }
    public Vector3 MapToWorld(Vector2Int mapUnits)
    {
        Vector3 worldCoords = new Vector3();
        worldCoords = grid.CellToWorld(MapToGrid(mapUnits));
        return worldCoords;
    }
    public Vector2Int WorldToMap(Vector3 worldCoords)
    {
        Vector2Int mapCoords = new Vector2Int();
        mapCoords = GridToMap(grid.WorldToCell(worldCoords));
        return mapCoords;
    }
    
}
public enum TileWeight
{
    //grass
    UNOBSTRUCTED = 1,
    LIGHT_OBS = 2,
    MEDIUM_OBS = 3,
    HEAVY_OBS = 4,
    //mountains, other allies
    OBSTRUCTED = 99
}
