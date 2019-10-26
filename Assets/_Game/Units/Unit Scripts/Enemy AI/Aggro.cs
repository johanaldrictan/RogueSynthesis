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

    public override ActionData GetActionData()
    {
        throw new System.NotImplementedException();
    }
}
