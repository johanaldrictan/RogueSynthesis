using System;
using System.Linq;
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
        // start at 0.0 significance
        option.SetSignificance(0.0f);

        // get variables
        Vector2Int bestTarget = new Vector2Int(-1, -1);
        Dictionary<Vector2Int, MutableTuple<Unit, float>> affectableUnits = option.GetAffectableUnits();
        bool unitIsAffectable;

        foreach (EffectState effect in option.GetAbility().GetEffectState())
        {
            foreach (Vector2Int key in affectableUnits.Keys)
            {
                unitIsAffectable = false;
                // go through each type of Effect that the ability deals, adding significance each time
                switch (effect)
                {
                    case EffectState.DAMAGE:
                        // we don't want to damage EnemyUnits
                        if (!(affectableUnits[key].Item1 is EnemyUnit))
                        {
                            unitIsAffectable = true;
                            // if using this ability would reduce this Unit's HP to 0 or below,
                            // definitely consider doing this.
                            if (affectableUnits[key].Item1.GetHealth() + affectableUnits[key].Item1.GetDamageReduction() <= (option.GetAbility() as Attack).GetDamage())
                            {
                                affectableUnits[key].Item2 = float.PositiveInfinity;
                            }
                            else
                            {
                                // the significance of this ability against this Unit is double the ratio of how much of the Unit's current health that the ability will deal, out of 10.0
                                float significance = ((float)((option.GetAbility() as Attack).GetDamage()) / (float)(affectableUnits[key].Item1.GetHealth() + affectableUnits[key].Item1.GetDamageReduction()) * 20.0f);
                                affectableUnits[key].Item2 += significance;
                            }
                        }
                        break;

                    case EffectState.BUFF_DR:
                        // viable units are (friendly) EnemyUnits that are not capped out of DR (max 5)
                        if (affectableUnits[key].Item1 is EnemyUnit && affectableUnits[key].Item1.GetDamageReduction() < 5)
                        {
                            unitIsAffectable = true;
                            // the significance of using this effect is greater the less DR the Unit already has (max 2.5)
                            float significance = ((float)(affectableUnits[key].Item1.GetDamageReduction() - 5)) / -2.0f;
                            affectableUnits[key].Item2 += significance;
                        }
                        break;

                    case EffectState.BUFF_DMG:
                        // viable units are (friendly) EnemyUnits that are not Buffed
                        if (affectableUnits[key].Item1 is EnemyUnit && !affectableUnits[key].Item1.attackBuffed)
                        {
                            unitIsAffectable = true;
                            affectableUnits[key].Item2 += 2.5f;
                        }
                        break;

                    case EffectState.STUN:
                        // viable units are (nonfriendly) non-EnemyUnits that are not stunned
                        if (!(affectableUnits[key].Item1 is EnemyUnit) && !affectableUnits[key].Item1.isImmobilized)
                        {
                            unitIsAffectable = true;
                            affectableUnits[key].Item2 += 2.0f;
                        }
                        break;

                    case EffectState.KNOCKBACK:
                        // viable units are (nonfriendly) non-EnemyUnits
                        if (!(affectableUnits[key].Item1 is EnemyUnit))
                        {
                            unitIsAffectable = true;
                            affectableUnits[key].Item2 += 1.5f;
                        }
                        break;

                    case EffectState.DISABLE:
                        // viable units are (nonfriendly) non-EnemyUnits
                        if (!(affectableUnits[key].Item1 is EnemyUnit) && !affectableUnits[key].Item1.isImmobilized)
                        {
                            unitIsAffectable = true;
                            affectableUnits[key].Item2 += 1.5f;
                        }
                        break;

                    default:
                        break;
                }
                // We have a new Best Target IF: its viably affectable AND: there isn't one OR this significance is bigger than the current best
                if ( unitIsAffectable && ((bestTarget.x == -1 && bestTarget.y == -1) || (affectableUnits[key].Item2 >= affectableUnits[bestTarget].Item2)) )
                {
                    bestTarget = key;
                }
            }
        }

        // If this Ability cannot affect any meaningful Units: Do NOT use it
        if ((bestTarget.x == -1 && bestTarget.y == -1) || affectableUnits.Keys.Count <= 0)
        {
            option.SetSignificance(float.NegativeInfinity);
        }
        else
        {
            option.SetSignificance(affectableUnits[bestTarget].Item2);
        }
    }





    protected override Tuple<Vector2Int, Direction> CommitMovement()
    {
        Tuple<Vector2Int, Direction> result = new Tuple<Vector2Int, Direction>(myUnit.GetMapPosition(), Direction.S);

        // get the affectable Units
        Dictionary<Vector2Int, MutableTuple<Unit, float>> affectableUnits = committedAbilityOption.GetAffectableUnits();

        // if there are no Units that can be affected with the chosen Ability, or that Ability is Wait
        if (affectableUnits.Keys.Count <= 0 || committedAbilityOption.GetAbility() is Wait)
        {
            
        }

        // if there is a Unit that can be affected
        else if (affectableUnits.Keys.Count > 0)
        {
            // get the variable bestTarget, storing the tile containing the most effective Unit to strike
            List<Vector2Int> keys = new List<Vector2Int>(affectableUnits.Keys);
            Vector2Int bestTarget = keys[0];
            foreach (Vector2Int key in keys)
            {
                if (affectableUnits[key].Item2 >= affectableUnits[bestTarget].Item2)
                {
                    bestTarget = key;
                }
            }

            // get the HashSet containing all possible (x, y) + Direction combinations that can hit the bestTarget (x, y) coordinate
            HashSet<Tuple<Vector2Int, Direction>> possibleMovements = committedAbilityOption.GetAffectableTiles()[bestTarget];


        }

        return result;
    }
}
