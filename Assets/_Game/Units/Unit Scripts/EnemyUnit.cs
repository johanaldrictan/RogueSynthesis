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

    // a Dictionary storing the possible tiles that this unit can move into during its turn
    public Dictionary<Vector2Int, Direction> MoveableTiles;

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
        MoveableTiles = new Dictionary<Vector2Int, Direction>();
        
        // create the list of possible actions to take
        possibleActions = new List<EnemyAction>
        {
            new Aggro(this)
        };
    }

    // this function is called when the EnemyUnit needs to know what it's going to do
    // it evaluates its possibleActions, selects the best one, and asks it to create ActionData to store
    public void NewActionData()
    {
        if (possibleActions.Count != 0)
        {
            MoveableTiles = FindMoveableTiles(MapController.instance.map);

            EnemyAction bestAction = null;
            float highestSignificance = 0.0f;

            // for each possible action:
            foreach (EnemyAction action in possibleActions)
            {
                // evaluate its significance level
                action.Evaluate();

                // check if it currently has the highest significance
                if (action.GetSignificance() > highestSignificance)
                {
                    bestAction = action;
                    highestSignificance = action.GetSignificance();
                }
            }

            plannedActionData = bestAction.GetActionData();
        }
    }

    // if there is a movement pattern planned, move in that way.
    public void ChooseMovement()
    {
        if (plannedActionData != null)
        {
            Move(plannedActionData.GetEndingPosition().x, plannedActionData.GetEndingPosition().y);
            ChangeDirection(plannedActionData.GetAbilityDirection());
        }
        hasMoved = true;
    }

    // if there is a planned ability to activate, execute that ability
    public override void ChooseAbility()
    {
        if (plannedActionData != null)
        {
            AvailableAbilities[plannedActionData.GetAbilityIndex()].Execute(this, plannedActionData.GetAbilityDirection());
        }
        ChangeDirection(Direction.S);
        hasActed = true;
    }


   
}
