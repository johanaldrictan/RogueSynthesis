using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EnemyUnit: A type of Unit that is controller by AI. 
// Work in progress... just a copy-paste of AlliedUnit right now
// Refer to Unit.cs if you want to see how a Unit ought to behave

public class EnemyUnit : Unit
{
    // plannedActionData is a place to store the action that this particular unit has decided to execute.
    // its value is either null or contains the action that it will next execute.
    private ActionData plannedActionData;

    // possibleActions is the List of actions that this EnemyUnit can choose to take.
    // each of these actions are evaluated and one is executed on its turn.
    private List<EnemyAction> possibleActions;

    public override void Awake()
    {
        hasActed = false;
        hasMoved = false;
        m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
        m_SpriteRenderer.sortingOrder = 99;
        Deaths = new Stack<DeathData>();
        plannedActionData = null;
        
        // create the list of possible actions to take
        possibleActions = new List<EnemyAction>
        {
            new Aggro(this)
        };
    }

    public override void ChooseAbility()
    {
        throw new System.NotImplementedException();
    }


   
}
