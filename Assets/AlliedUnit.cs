using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlliedUnit : Unit
{
    public bool isSelected;
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
}
