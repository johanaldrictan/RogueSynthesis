using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ActionData is a class used to store the data regarding an EnemyUnit's actions taken on its turn
/// This object stores the movement data the unit takes, as well as the index of the unit's ability list that it activates
/// </summary>

public class ActionData
{
    // the (x, y) coordinates the unit starts at before acting
    private Vector2Int startingPosition;

    // the cardinal direction that the Unit faces before acting
    private Direction startingDirection;

    // the stack of movement directions that the unit walks through
    // the top of the stack is the first step; continuously Pop() for step-by-step directions to move
    private Stack<Direction> movement;

    // the index of the Unit's Ability list to activate after it moves
    private int abilityIndex;

    // Constructor. Takes the variables necessary for the object and stores them
    public ActionData(Vector2Int start, Direction direction, Stack<Direction> move, int index)
    {
        startingPosition = start;
        startingDirection = direction;
        movement = move;
        abilityIndex = index;
    }

    public Vector2Int GetStartingPosition()
    {
        return startingPosition;
    }

    public Direction GetStartingDirection()
    {
        return startingDirection;
    }

    public Stack<Direction> GetMovement()
    {
        return movement;
    }

    public int GetAbilityIndex()
    {
        return abilityIndex;
    }
}
