using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapMath
{
    public static Vector2Int RelativeNorth = new Vector2Int(0, 1);
    public static Vector2Int RelativeSouth = new Vector2Int(0, -1);
    public static Vector2Int RelativeEast = new Vector2Int(1, 0);
    public static Vector2Int RelativeWest = new Vector2Int(-1, 0);
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
        worldCoords = MapController.instance.walkableTiles.GetCellCenterWorld(MapToGrid(x,y));
        return worldCoords;
    }
    public static Vector2Int WorldToMap(Vector3 worldCoords)
    {
        Vector2Int mapCoords = new Vector2Int();
        mapCoords = GridToMap(MapController.instance.grid.WorldToCell(worldCoords));
        return mapCoords;
    }
    public static bool InMapBounds(Vector2Int loc)
    {
        //check x
        if(loc.x >= MapController.instance.map.GetLowerBound(0) && loc.x <= MapController.instance.map.GetUpperBound(0))
        {
            //check y
            if(loc.y >= MapController.instance.map.GetLowerBound(1) && loc.y <= MapController.instance.map.GetUpperBound(1))
            {
                return true;
            }
        }
        return false;
    }
    //can probably do this mathematically
    public static Direction GetOppositeDirection(Direction d)
    {
        Direction output = Direction.NO_DIR;
        switch (d)
        {
            case Direction.N:
                output = Direction.S;
                break;
            case Direction.S:
                output = Direction.N;
                break;
            case Direction.W:
                output = Direction.E;
                break;
            case Direction.E:
                output = Direction.W;
                break;
        }
        return output;
    }
}
public enum Direction
{
    N,
    W,
    E,
    S,
    NO_DIR
}
