using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ActionData is a class used to store the data regarding an EnemyUnit's actions taken on its turn
/// This object stores the movement data the unit takes, as well as the index of the unit's ability list that it activates
/// </summary>

public class ActionData
{
    // the cardinal direction that the Unit faces to execute its ability
    private Direction abilityDirection;

    // the (x, y) coordinates the unit ends at after moving
    private Vector2Int endingPosition;

    // the index of the Unit's Ability list to activate after it moves
    private int abilityIndex;

    // Constructor. Takes the variables necessary for the object and stores them
    public ActionData(Vector2Int end, Direction direction, int index)
    {
        abilityDirection = direction;
        endingPosition = end;
        abilityIndex = index;
    }

    public Direction GetAbilityDirection()
    {
        return abilityDirection;
    }
    
    public Vector2Int GetEndingPosition()
    {
        return endingPosition;
    }

    public int GetAbilityIndex()
    {
        return abilityIndex;
    }
}
