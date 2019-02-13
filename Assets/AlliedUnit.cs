using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlliedUnit : Unit
{

    public override void Selected()
    {
        List<Vector2Int> moveLocs = FindMoveableTiles(MapController.instance.map);
        foreach (Vector2Int loc in moveLocs)
        {
            MapUIController.instance.tileHighlighting.SetTile(MapMath.MapToGrid(loc), MapUIController.instance.movementTile);
        }
    }
}
