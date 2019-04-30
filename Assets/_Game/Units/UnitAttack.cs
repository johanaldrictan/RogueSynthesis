using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitAttack
{
    //projects a line with range r from the front of the unit
    public static List<Vector2Int> GetAttackLine(Direction unitDir, int range, Vector2Int currLoc, bool isRanged)
    {
        List<Vector2Int> attackLine = new List<Vector2Int>();
        //move to attackable tiles
        currLoc += MapMath.DirToRelativeLoc(unitDir);
        if(isRanged)
            currLoc += MapMath.DirToRelativeLoc(unitDir);
        for (Vector2Int loc = currLoc ; MapMath.InMapBounds(loc); loc += MapMath.DirToRelativeLoc(unitDir))
        {
            if(range != 0)
            {
                attackLine.Add(loc);
                range--;
            }
            else
            {
                //break out of loop
                break;
            }
        }
        return attackLine;
    }
}
