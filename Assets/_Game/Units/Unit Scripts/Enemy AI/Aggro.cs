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
        if (option.GetAbility() is Wait)
        {
            option.SetSignificance(0.0f);
            return;
        }

        // if the Ability is an Attack
        if (option.GetAbility().GetType().IsSubclassOf(typeof(Attack)))
        {
            option.SetSignificance(0.0f);
            Dictionary<Vector2Int, Unit> affectableUnits = option.GetAffectableUnits();
            // if there are Units that this Attack could reach/affect
            if (affectableUnits.Count > 0)
            {
                foreach(Vector2Int key in affectableUnits.Keys)
                {
                    // we don't want to attack EnemyUnits
                    if (! (affectableUnits[key] is EnemyUnit))
                    {
                        // if using this ability would reduce this Unit's HP to 0 or below,
                        // definitely consider doing this.
                        if (affectableUnits[key].GetHealth() <= (option.GetAbility() as Attack).GetDamage())
                        {
                            option.SetSignificance(10.0f);
                        }

                        else
                        {
                            // the significance of this ability against this Unit is the ratio of how much of the Unit's current health that the ability will deal, out of 10.0
                            float contendingSignificance = ( (float)((option.GetAbility() as Attack).GetDamage()) / (float)(affectableUnits[key].GetHealth()) * 10.0f );
                            if (option.GetSignificance() <= contendingSignificance)
                            {
                                option.SetSignificance(contendingSignificance);
                            }
                        }
                    }
                }
            }

            // at this point, if this Attack cannot reach any opposing Units. Do NOT use it
            if (affectableUnits.Count <= 0 || option.GetSignificance() <= 0.0f)
            {
                option.SetSignificance(float.NegativeInfinity);
                return;
            }
        }
        // this Ability is not an Attack nor Wait
        // set it to a relatively low significance, as Attacking should be done if possible/better
        else
        {
            option.SetSignificance(2.0f);
            return;
        }
    }

    protected override Tuple<Vector2Int, Direction> CommitMovement()
    {
        Tuple<Vector2Int, Direction> result = new Tuple<Vector2Int, Direction>(myUnit.GetMapPosition(), Direction.S);

        return result;
    }
}
