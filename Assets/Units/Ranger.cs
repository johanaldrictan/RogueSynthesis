using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : AlliedUnit
{
    List<Vector2Int> attackTiles;
    private void Start()
    {
        attackTiles = new List<Vector2Int>();
    }
    void Attack(Vector2Int attackLoc)
    {
        if (attackTiles != null)
        {
            //attack at loc
            attackTiles.Clear();
        }
    }
    
}
