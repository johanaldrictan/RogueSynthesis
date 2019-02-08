using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapMath
{
    public static Vector2Int GridToMap(Vector3Int gridUnits)
    {
        Vector2Int converted = new Vector2Int
        {
            x = gridUnits.x - MapController.instance.mapWidthOffset,
            y = (gridUnits.y - MapController.instance.mapHeightOffset) * -1
        };
        return converted;
    }
    public static Vector3Int MapToGrid(Vector2Int mapUnits)
    {
        Vector3Int converted = new Vector3Int
        {
            x = mapUnits.x + MapController.instance.mapWidthOffset,
            y = (mapUnits.y - MapController.instance.mapHeightOffset) * -1
        };
        return converted;
    }
    public static Vector3Int MapToGrid(int x, int y)
    {
        Vector3Int converted = new Vector3Int
        {
            x = x + MapController.instance.mapWidthOffset,
            y = (y - MapController.instance.mapHeightOffset) * -1
        };
        return converted;
    }
    public static Vector3 MapToWorld(Vector2Int mapUnits)
    {
        Vector3 worldCoords = new Vector3();
        worldCoords = MapController.instance.grid.CellToWorld(MapToGrid(mapUnits));
        return worldCoords;
    }
    public static Vector3 MapToWorld(int x, int y)
    {
        Vector3 worldCoords = new Vector3();
        worldCoords = MapController.instance.grid.CellToWorld(MapToGrid(x,y));
        return worldCoords;
    }
    public static Vector2Int WorldToMap(Vector3 worldCoords)
    {
        Vector2Int mapCoords = new Vector2Int();
        mapCoords = GridToMap(MapController.instance.grid.WorldToCell(worldCoords));
        return mapCoords;
    }
}
