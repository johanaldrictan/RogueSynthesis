using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Cleave is an Ability derrived form the Attack Abstract Class (UnitAbility)
// Cleave hits opponents from an origin based on the unit's facing direction and range, 
// and also damages tiles so that the area of effect forms a perpendicular line of 3 tiles long
// simply put, deals damage in front of the unit, in a 3 tile wide perpendicular line

// If you want to create a new UnitAbility, refer to the comments/code on UnitAbility.cs and AbilityDatabase.cs

public class Focus : Attack
{
    public Focus()
    {
        isAOE = true;
    }
    // We're just doing straight damage here
    public override void DealEffects(Unit target, Unit source)
    {
        target.ChangeHealth( (GetDamage()*(-1)), source, this );
    }

    public override void DealDelayedEffect(Unit target, Unit source)
    {
        //do nothing because this does not have a delayed effect
    }

    // we're making a list of coordinates that this attack reaches
    // Cleave has an origin point range tiles in front of the Unit (default 1)
    // it also hits the two adjacent squares on either side of the origin
    public override List<Vector2Int> GetAreaOfEffect(Unit source, Direction direction)
    {
        List<Vector2Int> result = new List<Vector2Int>();

        return result;
    }

    public override int GetDamage()
    {
        return 0;
    }

    public override int GetRange()
    {
        return 0;
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(Cleave));
    }
}
