using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlliedUnit : Unit
{
    private Stack<Vector2Int> plannedPath;
    private bool hasNotSelectedMove;
    public bool isSelected;
    private void Start()
    {
        isSelected = false;
        plannedPath = new Stack<Vector2Int>();
        hasNotSelectedMove = false;
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
                MapUIController.instance.tileHighlighting.SetTile(MapMath.MapToGrid(loc), MapUIController.instance.movementTile);
            }
        }
    }
    public void DisplayPath(Vector2Int dest)
    {
        MapUIController.instance.pathHighlighting.ClearAllTiles();
        Stack<Vector2Int> path = GetMovementPath(FindMoveableTiles(MapController.instance.map), dest);
        plannedPath = path;
        while(path.Count != 0)
        {
            Vector2Int loc = path.Pop();
            MapUIController.instance.pathHighlighting.SetTile(MapMath.MapToGrid(loc), MapUIController.instance.attackTile);
        }
    }
}
