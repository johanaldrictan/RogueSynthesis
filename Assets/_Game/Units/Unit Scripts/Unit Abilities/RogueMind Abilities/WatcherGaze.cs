using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// If you want to create a new UnitAbility, refer to the comments/code on UnitAbility.cs and AbilityDatabase.cs

public class WatcherGaze : Attack
{
    public WatcherGaze()
    {
        isAOE = true;
    }
    // We're just doing straight damage here
    public override void DealEffects(Unit target, Unit source)
    {
        target.ChangeHealth( (GetDamage()*(-1)), source, this);
    }

    public override void DealDelayedEffect(Unit target, Unit source)
    {
        //do nothing because this does not have a delayed effect
    }

    public override List<Vector2Int> GetAreaOfEffect(Unit source, Direction direction)
    {
        List<Vector2Int> result = AttackPatterns.GetLineAOE(source.GetMapPosition(), direction, GetRange());
        return result;
    }

    public override int GetDamage()
    {
        return 18;
    }

    public override int GetRange()
    {
        return 3;
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(Cleave));
    }
}
