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
        Vector2Int bestTarget = new Vector2Int(int.MaxValue, int.MaxValue);
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
                            if ((option.GetAbility() as Attack).LethalAttack(affectableUnits[key].Item1))
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

                    case EffectState.IMMOBILIZE:
                        // viable units are (nonfriendly) non-EnemyUnits that are not stunned
                        if (!(affectableUnits[key].Item1 is EnemyUnit) && !(affectableUnits[key].Item1.GetImmobilizedDuration() > 0))
                        {
                            unitIsAffectable = true;
                            affectableUnits[key].Item2 += 2.5f;
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
                        if (!(affectableUnits[key].Item1 is EnemyUnit) && !(affectableUnits[key].Item1.GetDisabledDuration() > 0))
                        {
                            unitIsAffectable = true;
                            affectableUnits[key].Item2 += 3.5f;
                        }
                        break;

                    default:
                        break;
                }
                // We have a new Best Target IF: its viably affectable AND: there isn't one OR this significance is bigger than the current best
                if ( unitIsAffectable && ((bestTarget.x == int.MaxValue && bestTarget.y == int.MaxValue) || (affectableUnits[key].Item2 >= affectableUnits[bestTarget].Item2)) )
                {
                    bestTarget = key;
                }
            }
        }

        // If this Ability cannot affect any meaningful Units: Do NOT use it
        // IF:    This ability deals some sort of effect   AND      either: there is no bestTarget NOR affectable Unit
        if ( option.GetAbility().GetEffectState().Count > 0 && ((bestTarget.x == int.MaxValue && bestTarget.y == int.MaxValue) || affectableUnits.Keys.Count <= 0) )
        {
            option.SetSignificance(float.NegativeInfinity);
        }
        else if (option.GetAbility() is Wait)
        {
            // wait should be zero, AKA it should only be chosen if everything else is an awful choice
            option.SetSignificance(0.0f);
            // a disabled Unit MUST wait
            if (myUnit.GetDisabledDuration() > 0)
                option.SetSignificance(float.PositiveInfinity);
        }
        else
        {
            option.SetSignificance(affectableUnits[bestTarget].Item2);
        }
    }





    protected override Tuple<Queue<Vector2Int>, Direction> CommitMovement()
    {
        // default return value: stay where you are, point South
        Tuple<Queue<Vector2Int>, Direction> result = new Tuple<Queue<Vector2Int>, Direction>(new Queue<Vector2Int>(), Direction.S);

        // get the affectable Units
        Dictionary<Vector2Int, MutableTuple<Unit, float>> affectableUnits = committedAbilityOption.GetAffectableUnits();

        // if there are no Units that can be affected with the chosen Ability, or that Ability is not an Attack
        if (affectableUnits.Keys.Count <= 0 || !(committedAbilityOption.GetAbility().GetType().IsSubclassOf(typeof(Attack))))
        {
            // finding the path to the closest Unit...
            Tuple<Queue<Vector2Int>, int> bestPath = new Tuple<Queue<Vector2Int>, int>(new Queue<Vector2Int>(), int.MaxValue);
            // check the path to the adjacent squares of each Unit on the map
            foreach(Vector2Int current in myUnit.globalPositionalData.GetLocations())
            {
                Unit unit = myUnit.globalPositionalData.SearchLocation(current);
                Dictionary<Vector2Int, Direction> neighbors = MapMath.GetNeighbors(current);
                foreach (Vector2Int neighbor in neighbors.Keys)
                {
                    Tuple<Queue<Vector2Int>, int> currentPath = MapController.instance.GetShortestPath(myUnit.shortestPaths, neighbor);
                    // IF the current path exists, is for a non-EnemyUnit, and has a shorter distance
                    if (currentPath != null && !(unit is EnemyUnit) && currentPath.Item2 < bestPath.Item2)
                    {
                        bestPath = new Tuple<Queue<Vector2Int>, int>(currentPath.Item1, currentPath.Item2);
                    }
                }
            }

            // from that path, find the farthest tile that the Unit can reach and set it as the result
            Queue<Vector2Int> closestPath = bestPath.Item1;
            Vector2Int finalTile = myUnit.GetMapPosition();
            while (closestPath.Count != 0)
            {
                Vector2Int currentTile = closestPath.Dequeue();
                if (myUnit.MoveableTiles.ContainsKey(currentTile) && MapController.instance.weightedMap[currentTile] != (int)TileWeight.OBSTRUCTED)
                {
                    result.Item1.Enqueue(currentTile);
                }
                else
                    break;
            }
        }

        // if there is a Unit that can be affected
        else if (affectableUnits.Keys.Count > 0)
        {
            // get the variable bestTarget, storing the tile containing the most effective Unit to strike
            List<Vector2Int> keys = new List<Vector2Int>(affectableUnits.Keys);
            Vector2Int bestTarget = new Vector2Int(int.MaxValue, int.MaxValue);
            float bestValue = int.MinValue;
            foreach (Vector2Int key in keys)
            {
                if (affectableUnits[key].Item2 >= bestValue)
                {
                    bestTarget = key;
                    bestValue = affectableUnits[key].Item2;
                }
            }

            // iterate through HashSet containing all possible (x, y) + Direction combinations that can hit the bestTarget (x, y) coordinate
            // find the one that is the most reasonable, using a similar but lesser significance system to other sections of this AI
            HashSet<Tuple<Vector2Int, Direction>> possibleMovements = committedAbilityOption.GetAffectableTiles()[bestTarget];
            float bestVal = float.MinValue;
            Tuple<Vector2Int, Direction> bestEndingPosition = new Tuple<Vector2Int, Direction>(myUnit.GetMapPosition(), Direction.S);
            foreach(Tuple<Vector2Int, Direction> current in possibleMovements)
            {
                List<Unit> affectedUnits = committedAbilityOption.GetAffectedUnits(current.Item1, current.Item2);
                float currentVal = 0.0f;
                float friendlies = 0.0f;
                float hostiles = 0.0f;
                foreach(Unit unit in affectedUnits)
                {
                    if (unit is EnemyUnit && currentVal <= (float.MinValue / 2)) // this current selection would hit a friendly Unit as well, making this way less likely to do
                    {
                        currentVal = (float.MinValue / 2);
                    }
                }
                foreach (Vector2Int tile in myUnit.globalPositionalData.GetLocations())
                {
                    if (Math.Abs((tile-current.Item1).magnitude) <= myUnit.GetMoveSpeed())
                    {
                        if (myUnit.globalPositionalData.SearchLocation(tile) is EnemyUnit)
                            friendlies++;
                        else if (myUnit.globalPositionalData.SearchLocation(tile) is AlliedUnit)
                            hostiles++;
                    }
                }
                // the AI has a VERY slight bias towards staying where it is
                if (current.Item1 == myUnit.GetMapPosition())
                    currentVal += 0.01f;
                currentVal += friendlies / hostiles;
                if (currentVal >= bestVal)
                {
                    bestVal = currentVal;
                    bestEndingPosition = current;
                }
            }
            Queue<Vector2Int> finalPath = MapController.instance.GetShortestPath(myUnit.shortestPaths, bestEndingPosition.Item1).Item1;
            result = new Tuple<Queue<Vector2Int>, Direction>(finalPath, result.Item2);
        }

        return result;
    }
}
