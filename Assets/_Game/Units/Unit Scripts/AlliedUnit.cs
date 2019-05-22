using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlliedUnit : Unit
{
    [System.NonSerialized] public List<Vector2Int> plannedPath;

    public override void Awake()
    {
        hasAttacked = false;
        hasMoved = false;
        plannedPath = new List<Vector2Int>();
        m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
        m_SpriteRenderer.sortingOrder = 99;
    }
    
    public override void DisplayMovementTiles()
    {
        //clear tilemap
        MapUIController.instance.tileHighlighting.ClearAllTiles();
        if (!hasAttacked)
        {
            Dictionary<Vector2Int, Direction>.KeyCollection moveLocs = FindMoveableTiles(MapController.instance.map).Keys;
            foreach (Vector2Int loc in moveLocs)
            {
                MapUIController.instance.RangeHighlight(loc);
            }
        }
    }

    public void DisplayShortestPath(Vector2Int dest)
    {
        MapUIController.instance.pathHighlighting.ClearAllTiles();
        Stack<Vector2Int> path = GetMovementPath(FindMoveableTiles(MapController.instance.map), dest);
        //plannedPath.Clear();

        if (path == null) { return; }
        
        //plannedPath.AddRange(path);
        while(path.Count != 0)
        {
            Vector2Int loc = path.Pop();    
            MapUIController.instance.PathHighlight(loc);
        }
    }

    public void DisplayPlannedPath()
    {
        foreach (Vector2Int loc in this.plannedPath)
        {
            MapUIController.instance.PathHighlight(loc);
        }
    }

    public override void Move(int x, int y)
    {
        MapUIController.instance.pathHighlighting.ClearAllTiles();
        MapUIController.instance.tileHighlighting.ClearAllTiles();
        //restore old tilevalue
        MapController.instance.map[mapPosition.x, mapPosition.y] = (int)tile;
        mapPosition.x = x;
        mapPosition.y = y;
        tile = (TileWeight)MapController.instance.map[mapPosition.x, mapPosition.y];
        MapController.instance.map[mapPosition.x, mapPosition.y] = (int)TileWeight.OBSTRUCTED;
        this.transform.position = MapMath.MapToWorld(x,y);
        plannedPath.Clear();
        hasMoved = true;
        hasAttacked = true; // for testing only. CHANGE LATER
    }
}
