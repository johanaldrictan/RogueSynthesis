﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapMath
{
    // World: X and Y coordinates in Unity

    // Grid: Unity Tilemap Units
    
    // weightedMap: 2D Array in MapController.cs

    public static Vector2Int RelativeNorth = new Vector2Int(0, 1);
    public static Vector2Int RelativeSouth = new Vector2Int(0, -1);
    public static Vector2Int RelativeEast = new Vector2Int(1, 0);
    public static Vector2Int RelativeWest = new Vector2Int(-1, 0);

    public static Vector2Int GridToMap(Vector3Int gridUnits)
    {
        Vector2Int converted = new Vector2Int(-gridUnits.y, gridUnits.x);
        /*
        Vector2Int converted = new Vector2Int
        {
            x = -gridUnits.y + MapController.instance.mapHeightOffset,
            y = gridUnits.x - MapController.instance.mapWidthOffset
        };*/
        return converted;
    }
    public static Vector2Int GridToMap(Vector2Int gridUnits)
    {
        Vector2Int converted = new Vector2Int(-gridUnits.y, gridUnits.x);
        return converted;
    }

    public static Vector3Int MapToGrid(Vector2Int mapUnits)
    {
        return MapToGrid(mapUnits.x, mapUnits.y);
    }

    public static Vector3Int MapToGrid(int x, int y)
    {
        Vector3Int converted = new Vector3Int(y, -x, 0);
        /*
        Vector3Int converted = new Vector3Int
        {
            x = y + MapController.instance.mapWidthOffset,
            y = -x + MapController.instance.mapHeightOffset
        };*/
        return converted;
    }

    public static Vector3 MapToWorld(Vector2Int mapUnits)
    {
        return MapToWorld(mapUnits.x, mapUnits.y);
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
        mapCoords = GridToMap(MapController.instance.walkableTiles.WorldToCell(worldCoords));
        return mapCoords;
    }

    //TODO: might need to fix for non square maps
    public static bool InMapBounds(Vector2Int loc)
    {
        return MapController.instance.weightedMap.ContainsKey(loc);
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
    /// <summary>
    /// Returns a vector to be added to the location of the unit to get the tile in front of the unit
    /// </summary>
    /// <param name="d"></param>
    public static Vector2Int DirToRelativeLoc(Direction d)
    {
        Vector2Int output = new Vector2Int();
        switch (d)
        {
            case Direction.N:
                output = RelativeNorth;
                break;
            case Direction.S:
                output = RelativeSouth;
                break;
            case Direction.W:
                output = RelativeWest;
                break;
            case Direction.E:
                output = RelativeEast;
                break;
        }
        return output;
    }
    public static Direction LocToDirection(Vector2Int diff)
    {
        Direction d = Direction.NO_DIR;
        if (diff.y > 0)
            d = Direction.N;
        else if (diff.y < 0)
            d = Direction.S;
        if (diff.x > 0)
            d = Direction.E;
        else if (diff.x < 0)
            d = Direction.W;
        return d;
    }
    public static Dictionary<Vector2Int, Direction> GetNeighbors(Vector2Int curr)
    {
        Dictionary<Vector2Int, Direction> neighbors = new Dictionary<Vector2Int, Direction>();
        neighbors.Add(new Vector2Int(curr.x, curr.y + 1), Direction.N);
        neighbors.Add(new Vector2Int(curr.x - 1, curr.y), Direction.W);
        neighbors.Add(new Vector2Int(curr.x, curr.y - 1), Direction.S);
        neighbors.Add(new Vector2Int(curr.x + 1, curr.y), Direction.E);
        return neighbors;
    }

    //public static 
}

public enum Direction
{
    N = 60,
    W = -60,
    E = -120,
    S = 120,
    NO_DIR
}