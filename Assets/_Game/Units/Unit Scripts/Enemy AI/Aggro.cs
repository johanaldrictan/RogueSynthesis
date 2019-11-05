using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Aggro is a type of EnemyAction that represents the Unit taking offensive actions
/// This Action entails picking an enemy, moving towards it and then attacking it
/// </summary>

public class Aggro : EnemyAction
{
    // Constructor. Same as the Base Class.
    public Aggro(EnemyUnit unit) : base(unit)
    {
        myUnit = unit;
    }

    // TEMPORARY. Currently Aggro is currently the only implemented option, so it will always be chosen.
    // Once more EnemyAction objects are created, this function must be edited.
    public override void Evaluate()
    {
        significance = 10.0f;
    }

    protected override void SignifyAbility(AbilityOption option)
    {
        // wait should be the last option chosen. If every other option is negative Infinity (AKA impossible), then this will be chosen 
        if (option is Wait)
        {
            option.SetSignificance(0.0f);
            return;
        }

        if (option.GetType().IsSubclassOf(typeof(Attack)))
        {

        }
    }

    protected override Tuple<Vector2Int, Direction> CommitMovement()
    {
        Tuple<Vector2Int, Direction> result = new Tuple<Vector2Int, Direction>(myUnit.GetMapPosition(), Direction.S);

        return result;
    }
}
