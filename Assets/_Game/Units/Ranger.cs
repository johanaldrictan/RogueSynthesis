using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : AlliedUnit
{
    List<Vector2Int> attackTiles;
    public override void Start()
    {
        attackTiles = new List<Vector2Int>();
    }
    void Attack()
    {
        if (attackTiles != null)
        {
            //attack depending on attack type
            attackTiles.Clear();
        }
    }
    void GetAttackTiles()
    {
        
    }
}
