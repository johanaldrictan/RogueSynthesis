using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Claymore is a type of Trap
/// It triggers when a Unit ends its movement ontop of the same tile that this object inhabits
/// Triggering a Claymore will deal damage to the Unit that triggered it and immobilize it for 1 turn
/// </summary>

public class ClaymoreTrap : Trap
{
    public override string GetResourcePath()
    {
        return "Sprites/TestMine";
    }

    public override void Effect(Unit target)
    {
        target.ChangeHealth(-10, sourceUnit, placingAbility);
        target.Disable(1);
    }

    public override bool TriggerCondition(Unit inQuestion)
    {
        if (TurnController.instance != null)
        {
            // a Claymore triggers when a Unit that is not the same type of Unit as the one who placed the Claymore is currently standing ontop of it
            return ((TurnController.instance.trapPositionalData.SearchLocation(inQuestion.GetMapPosition()) is Trap) && (inQuestion.GetType() != sourceUnit.GetType()));
        }
        return false;
    }

    
}
