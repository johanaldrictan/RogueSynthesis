using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AbilityOption is an object used to store a possible ability the EnemyController can choose
/// these are used specifically for when a Unit has decided to Aggro, and is choosing a specific aggressive ability
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
    // the values are Tuples containing:
    // 1: the (x, y) coordinate from where the ability is be actitvated to affect the key location
    // 2: the direction the Unit must face from that coordinate to affect the key location
    private Dictionary<Vector2Int, Tuple<Vector2Int, Direction>> affectableTiles;

    // significance is the float value that represents how useful this particular Ability would be compared to others.
    // the higher this float is, the more effective and realistic activating this ability should be.
    // a value of 0.0 signifies the lowest possible significance, and 10.0 represents the highest possible significance.
    private float significance;

    // basic constructor
    public AbilityOption(EnemyUnit source, UnitAbility abil)
    {
        sourceUnit = source;
        ability = abil;
        affectableTiles = new Dictionary<Vector2Int, Tuple<Vector2Int, Direction>>();
        significance = 0.0f;
    }

    public float GetSignificance()
    {
        return significance;
    }

    public void SetSignificance(int newValue)
    {
        significance = newValue;
    }

    public void AddToSignificance(int rightHandValue)
    {
        significance += rightHandValue;
    }

    public Dictionary<Vector2Int, Tuple<Vector2Int, Direction>> GetAffectableTiles()
    {
        return affectableTiles;
    }

    public void ResetAffectableTiles()
    {
        affectableTiles.Clear();
    }

    // This function will evaluate the tiles in which this ability can reach, and update the affectableTiles Dictionary to its correct state
    public void EvaluateAffectableTiles()
    {
        // the Ability that this object stores must be a type of Attack, as those Abilities have affectable areas.
        // if not an Attack, this function will only add the tile that the unit is occupying
        if (!ability.GetType().IsSubclassOf(typeof(Attack)))
        {
            affectableTiles[sourceUnit.GetMapPosition()] = new Tuple<Vector2Int, Direction>(sourceUnit.GetMapPosition(), Direction.S);
            return;
        }
        // otherwise, this ability is a type of Attack.
        // iterate through 
        else
        {
            List<Direction> directionsToCheck = new List<Direction> {Direction.N, Direction.S, Direction.E, Direction.W};
            foreach(Vector2Int tile in sourceUnit.MoveableTiles.Keys)
            {
                foreach(Direction direction in directionsToCheck)
                {
                    // ****************
                }
            }
        }
    }

}
