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

    // the path the Unit has committed to moving through
    private Queue<Vector2Int> movementPath;

    // the index of the Unit's Ability list to activate after it moves
    private int abilityIndex;

    // Constructor. Takes the variables necessary for the object and stores them
    public ActionData(Queue<Vector2Int> path, Direction direction, int index)
    {
        abilityDirection = direction;
        movementPath = path;
        abilityIndex = index;
    }

    public Direction GetAbilityDirection()
    {
        return abilityDirection;
    }
    
    public Queue<Vector2Int> GetMovementPath()
    {
        return movementPath;
    }

    public int GetAbilityIndex()
    {
        return abilityIndex;
    }

    public Vector2Int GetEndingPosition()
    {
        

        Queue<Vector2Int> clone = new Queue<Vector2Int>(movementPath);
        Vector2Int result = new Vector2Int(int.MaxValue, int.MaxValue);
        while (clone.Count > 0)
        {
            result = clone.Dequeue();
        }
        return result;
    }
}
