using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Cleave is an Ability derrived form the Attack Abstract Class (UnitAbility)
// Cleave hits opponents from an origin based on the unit's facing direction and range, 
// and also damages tiles so that the area of effect forms a perpendicular line of 3 tiles long
// simply put, deals damage in front of the unit, in a 3 tile wide perpendicular line

// If you want to create a new UnitAbility, refer to the comments/code on UnitAbility.cs and AbilityDatabase.cs

public class Cleave : Attack
{
    public Cleave()
    {
        isAOE = true;
    }

    // We're just doing straight damage here
    public override void DealEffects(Unit target, Unit source)
    {
        target.ChangeHealth( (GetDamage()*(-1)), source, this );
        target.isImmobilized = true;
    }

    // we're making a list of coordinates that this attack reaches
    // Cleave has an origin point range tiles in front of the Unit (default 1)
    // it also hits the two adjacent squares on either side of the origin
    public override List<Vector2Int> GetAreaOfEffect(Vector2Int source, Direction direction)
    {
        return AttackHelper.GetTShapedAOE(source, direction, GetRange());      
    }

    public override int GetDamage()
    {
        return 5;
    }

    public override List<EffectState> GetEffectState()
    {
        return new List<EffectState> { EffectState.DAMAGE, EffectState.STUN };
    }

    public override string GetName()
    {
        return "Cleave";
    }

    public override int GetRange()
    {
        return 1;
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(Cleave));
    }
}
