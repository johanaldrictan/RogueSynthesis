using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EnemyAction is an abstract class that represents a possible option for the enemy AI.
/// The When an EnemyUnit takes its turn, it must evaluate all of these options and choose the best one
/// Once chosen, this object will also be called return the necessary information in order to complete the action.
/// </summary>

public abstract class EnemyAction
{
    // Reference to the Unit that this object has control over
    protected EnemyUnit myUnit;

    // significance is the float value that represents how useful a particular action would be compared to others.
    // the higher this float is, the more effective and realistic activating this ability should be.
    // a value of 0.0 signifies the lowest possible significance, and 10.0 represents the highest possible significance.
    protected float significance;

    // constructor. Takes a unit and assigns it to myUnit
    public EnemyAction(EnemyUnit unit)
    {
        myUnit = unit;
    }

    // This function looks at the game-field and evalates it, resulting in a float significance.
    // The function stores the float within the object
    public abstract void Evaluate();

    // This function evaluates the game-field 
    // It then creates and returns an ActionData object referring to the steps for executing the action
    public abstract ActionData GetActionData();


    public float GetSignificance()
    {
        return significance;
    }
}