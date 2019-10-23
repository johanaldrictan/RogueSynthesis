using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Aggro is a type of EnemyAction that represents the Unit taking offensive actions
/// This Action entails moving towards an enemy and then attacking it
/// </summary>

public class Aggro : EnemyAction
{
    // Constructor. Same as the Base Class.
    public Aggro(EnemyUnit unit) : base(unit)
    {
        myUnit = unit;
    }

    // TEMPORARY. Currently Aggro is the only option, so it will always be chosen.
    // Once more EnemyAction objects are created, this function must be returned to.
    protected override void Evaluate()
    {
        significance = 10.0f;
    }

    protected override void Execute(ActionData data)
    {
        throw new System.NotImplementedException();
    }

    protected override ActionData GetActionData()
    {
        throw new System.NotImplementedException();
    }
}
