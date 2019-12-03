using System;
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
    // the higher this float is, the more effective and realistic this option should be.
    // a value of 0.0 signifies the lowest possible significance, and 10.0 represents the highest possible significance.
    protected float significance;

    // committedAbilityOption is the option that this Unit has committed to activating
    protected AbilityOption committedAbilityOption;

    // committedAbilityIndex is the index of the Ability that this Unit has committed to activating.
    protected int committedAbilityIndex;

    // committedMovement is the data regarding the movement that the Unit has committed to doing
    protected Tuple<Queue<Vector2Int>, Direction> committedMovement;

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
    public ActionData GetActionData()
    {
        // first, find the best ability in terms of significance.
        List<AbilityOption> options = myUnit.GetAbilityOptions();
        float highestSignificance = float.NegativeInfinity;

        // for each possible action:
        for (int i = 0; i < options.Count; i++)
        {
            // evaluate the significance of this particular option
            SignifyAbility(options[i]);
            // Debug.Log("Ability: " + options[i].GetAbility() + " | Significance: " + options[i].GetSignificance());

            // check if it has the highest significance
            if (options[i].GetSignificance() >= highestSignificance)
            {
                committedAbilityIndex = i;
                committedAbilityOption = options[i];
                highestSignificance = options[i].GetSignificance();
            }
        }

        committedMovement = CommitMovement();
        return new ActionData(committedMovement.Item1, committedMovement.Item2, committedAbilityIndex);
    }

    // Takes an AbilityOption object and evaluates/sets its significance
    // this function is abstract because way in which an ability is given significance is biased based on the type of EnemyAction that has been decided upon
    // for example, being Aggro will value different things when choosing an option as opposed to being Defensive
    protected abstract void SignifyAbility(AbilityOption option);

    // Decides the best Movement option to choose
    // this is biased/has a heuristic: the ability that is being used has already been chosen at this point.
    // this function is abstract because way in which an ability is given significance is biased based on the type of EnemyAction that has been decided upon
    // for example, being Aggro will value different things when choosing an option as opposed to being Defensive
    protected abstract Tuple<Queue<Vector2Int>, Direction> CommitMovement();




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
        if (significance > 10.0f && significance != float.PositiveInfinity)
        {
            significance = 10.0f;
        }
        else if (significance < 0.0f && significance != float.NegativeInfinity)
        {
            significance = 0.0f;
        }
    }

}