using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class contains methods that help move information from one Unit to another
// use these methods in order to "copy-paste" from one type of unit to another

public static class UnitConversions
{
    public static void UnitToEnemy(Unit toCopy, EnemyUnit target)
    {
        target.SetName(toCopy.GetName());
        target.SetHealth(toCopy.GetHealth());
        target.SetMoveSpeed(toCopy.GetMoveSpeed());
        target.SetDirection(toCopy.GetDirection());
        target.SetMapPosition(toCopy.GetMapPosition());
        target.globalPositionalData = toCopy.globalPositionalData;
        target.sprites = toCopy.sprites;
        target.unitData = toCopy.unitData;
        target.availableAbilities = toCopy.availableAbilities;
        target.deathData = toCopy.deathData;
    }
}
