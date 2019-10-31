﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// If you want to create a new UnitAbility, refer to the comments/code on UnitAbility.cs and AbilityDatabase.cs

public class ImmobilizationBeam : Attack
{
    public ImmobilizationBeam()
    {
        isAOE = false;
    }
    // We're just doing straight damage here
    public override void DealEffects(Unit target, Unit source)
    {
        if (target != null)
        {
            target.ChangeHealth((GetDamage() * (-1)), source, this);
            target.isImmobilized = true;
        }
    }

    public override void DealDelayedEffect(Unit target, Unit source)
    {
        //do nothing because this does not have a delayed effect
    }

    public override List<Vector2Int> GetAreaOfEffect(Unit source, Direction direction)
    {
        List<Vector2Int> result = AttackHelper.GetLineAOE(source.GetMapPosition(), direction, GetRange());
        return result;
    }

    public override int GetDamage()
    {
        return 5;
    }

    public override int GetRange()
    {
        return 13;
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(Cleave));
    }
}