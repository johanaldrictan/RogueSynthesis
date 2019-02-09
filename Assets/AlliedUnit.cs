using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlliedUnit : Unit
{

    public override void OnMouseDown()
    {
        List<Vector2Int> moveLocs = FindMoveableTiles(MapController.instance.map);
        foreach(Vector2Int loc in moveLocs)
        {
            MapController.instance.walkableTiles.SetColor(MapMath.MapToGrid(loc), Color.blue);
        }
    }
}
