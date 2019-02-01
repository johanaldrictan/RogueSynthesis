using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    public Grid grid;
    public Tilemap walkableTiles;

    public int[,] map;

    public int mapHeightOffset;
    public int mapWidthOffset;
    

    // Start is called before the first frame update
    void Start()
    {
        GameObject tilemapObject = GameObject.FindGameObjectWithTag("Tilemap");
        if (tilemapObject != null)
        {
            walkableTiles = tilemapObject.GetComponent<Tilemap>();
        }
        if (tilemapObject == null)
        {
            Debug.Log("Cannot find Tilemap object");
        }
        grid = walkableTiles.GetComponentInParent<Grid>();
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
