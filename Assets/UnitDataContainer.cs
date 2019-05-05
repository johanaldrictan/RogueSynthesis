using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// A UnitDataContainer stores parameters regarding to the
// spawning of a Unit.
// It stores the UnitData Class of the unit,
// the (X,Y) spawning position,
// and the direction it faces when it spawns

[System.Serializable]
public class UnitDataContainer
{
    public UnitData data;
    public Vector2Int spawnPosition;
    public Direction spawnDirection;
}
