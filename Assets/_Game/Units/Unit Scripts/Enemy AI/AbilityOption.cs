using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AbilityOption is an object used to store a possible ability the EnemyController can choose
/// this script stores and caluclates values in order to help determine viabilities between abilities
/// </summary>

public class AbilityOption
{
    // reference to the actual ability that this object is storing information about
    private UnitAbility ability;

    // reference to the Unit that owns this ability
    private EnemyUnit sourceUnit;

    // affectableTiles is a dictionary storing all possible tiles that this particular ability can affect
    // the keys are (x, y) coordinates that this ability can affect
    // the value is a HashSet of Tuples containing:
    // 1: the (x, y) coordinate from where the ability is be actitvated to affect the key location
    // 2: the direction the Unit must face from that coordinate to affect the key location
    private Dictionary<Vector2Int, HashSet<Tuple<Vector2Int, Direction>>> affectableTiles;

    // affectableUnits is a dictionary storing all possible Units that this particular ability can affect
    // the keys are (x, y) coordinates of the Unit's position
    // the values are Tuples containing: the Units themselves and a significance value for each one
    private Dictionary<Vector2Int, MutableTuple<Unit, float>> affectableUnits;

    // significance is the float value that represents how useful this particular Ability would be compared to others.
    // the higher this float is, the more effective and realistic activating this ability should be.
    // a value of 0.0 signifies the lowest possible significance, and 10.0 represents the highest possible significance.
    private float significance;
    
    // basic constructor
    public AbilityOption(EnemyUnit source, UnitAbility abil)
    {
        sourceUnit = source;
        ability = abil;
        affectableTiles = new Dictionary<Vector2Int, HashSet<Tuple<Vector2Int, Direction>>>();
        affectableUnits = new Dictionary<Vector2Int, MutableTuple<Unit, float>>();
        significance = 0.0f;
    }

    public UnitAbility GetAbility()
    {
        return ability;
    }

    public float GetSignificance()
    {
        return significance;
    }

    public void SetSignificance(float newValue)
    {
        significance = newValue;
        if (significance > 10.0f && significance != float.PositiveInfinity)
        {
            significance = 10.0f;
        }
        else if (significance < 0.0f && significance != float.NegativeInfinity)
        {
            significance = 0.0f;
        }
    }

    public void AddToSignificance(float rightHandValue)
    {
        significance += rightHandValue;
        if (significance < 0.0f && significance != float.NegativeInfinity)
        {
            significance = 0.0f;
        }
        else if (significance > 10.0f && significance != float.PositiveInfinity)
        {
            significance = 10.0f;
        }
    }

    public Dictionary<Vector2Int, HashSet<Tuple<Vector2Int, Direction>>> GetAffectableTiles()
    {
        return affectableTiles;
    }

    public void ResetAffectableTiles()
    {
        affectableTiles.Clear();
    }

    public Dictionary<Vector2Int, MutableTuple<Unit, float>> GetAffectableUnits()
    {
        return affectableUnits;
    }

    public void ResetAffectableUnits()
    {
        affectableUnits.Clear();
    }

    // This function will evaluate the tiles in which this ability can reach, and update the affectableTiles Dictionary to its correct state
    public void EvaluateAffectableTiles()
    {
        ResetAffectableTiles();
        ResetAffectableUnits();

        // Attacks have affectable areas. Therefore, only those types of UnitAbilities need to be evaluated
        // if not an Attack, this function will only add the tile that the unit is occupying
        if (! (ability.GetType().IsSubclassOf(typeof(Attack))) )
        {
            // add to affectableTiles, creating a new set at the key if it hasn't been made yet
            if (! affectableTiles.ContainsKey(sourceUnit.GetMapPosition()))
            { 
                affectableTiles[sourceUnit.GetMapPosition()] = new HashSet<Tuple<Vector2Int, Direction>>(); 
            }
            affectableTiles[sourceUnit.GetMapPosition()].Add(new Tuple<Vector2Int, Direction>(sourceUnit.GetMapPosition(), Direction.S));

            affectableUnits[sourceUnit.GetMapPosition()] = new MutableTuple<Unit, float>(sourceUnit, 0.0f);
            return;
        }

        // otherwise, if this ability is a type of Attack, iterate through
        else
        {
            // iterate three times: first for all possible moveable tiles, second for all cardinal directions, third for each tile in the area of effect at that location
            List<Direction> directionsToCheck = new List<Direction> {Direction.N, Direction.S, Direction.E, Direction.W};
            foreach(Vector2Int tile in sourceUnit.MoveableTiles.Keys)
            {
                foreach(Direction direction in directionsToCheck)
                {
                    List<Vector2Int> areaOfEffect = (ability as Attack).GetAreaOfEffect(tile, direction);

                    foreach(Vector2Int affectedTile in areaOfEffect)
                    {
                        // if this tile Key is new to the Dictionary:
                        if (! affectableTiles.ContainsKey(affectedTile))
                        {
                            // create the HashSet for this Key
                            affectableTiles[affectedTile] = new HashSet<Tuple<Vector2Int, Direction>>();
                            
                            // check if there's a Unit here. if there is one and it's new, add it to the affectableUnits
                            Unit searchResult = sourceUnit.globalPositionalData.SearchLocation(affectedTile);
                            if (searchResult != null && !affectableUnits.ContainsKey(affectedTile))
                            {
                                affectableUnits[affectedTile] = new MutableTuple<Unit, float>(searchResult, 0.0f);
                            }
                        }
                        // add the movement information to the HashSet at this Key. (If it already exists, it will automatically do nothing since its a Set)
                        affectableTiles[affectedTile].Add(new Tuple<Vector2Int, Direction>(tile, direction));
                    }
                }
            }
        }
    }

}
