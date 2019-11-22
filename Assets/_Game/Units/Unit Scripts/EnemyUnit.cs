using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EnemyUnit: A type of Unit that is controller by AI. 
// Refer to Unit.cs if you want to see how a Unit ought to behave

public class EnemyUnit : Unit
{
    // boxedIn signifies whether or not this Unit has been "boxed in" and cannot make much movement
    // being "boxed in" will be defined as follows: the Unit is unable to move along any path that has a total movement equal to this Unit's movement speed
    // as in, there is no path this Unit can take which allows it to move its maximum possible distance
    public bool boxedIn;

    // plannedActionData is a place to store the action that this particular unit has decided to execute.
    // its value is either null or contains the action that it will next execute.
    public ActionData plannedActionData;

    // a Dictionary storing the possible tiles that this unit can move into during its turn
    public Dictionary<Vector2Int, Direction> MoveableTiles;

    // shortestPaths stores the shortest paths from this Unit's current position to every other tile on the map
    // Keys are the distance required to travel values are dictionaries with key destination and value pathway
    public Dictionary<int, Dictionary<Vector2Int, Queue<Vector2Int>>> shortestPaths;

    // possibleActions is the List of actions that this EnemyUnit can choose to take.
    // each of these actions are evaluated and one is executed on its turn.
    private List<EnemyAction> possibleActions;

    // this is the storage of AbilityOption objects for each of this Unit's abilities. 
    // Each ability will be evaluated separately and each store independent data
    // this list will mirror the Unit's list of Abilities: the index of an object in this list will be the same as the index of the Ability list for the relevant item
    private List<AbilityOption> possibleAbilities;
    
    public override void Awake()
    {
        hasActed = false;
        hasMoved = false;
        boxedIn = false;
        m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
        m_SpriteRenderer.sortingOrder = 99;
        Deaths = new Stack<DeathData>();
        plannedActionData = null;
        MoveableTiles = new Dictionary<Vector2Int, Direction>();
    }

    public override void Start()
    {
        mapPosition = MapMath.WorldToMap(this.transform.position);
        tile = (TileWeight)MapController.instance.weightedMap[mapPosition];
        MapController.instance.weightedMap[mapPosition] = (int)TileWeight.OBSTRUCTED;

        // create the list of possible action categories to take
        possibleActions = new List<EnemyAction>
        {
            new Aggro(this)
        };

        // create list of possible abilities to choose from
        possibleAbilities = new List<AbilityOption>();
        foreach (UnitAbility ability in this.AvailableAbilities)
        {
            possibleAbilities.Add(new AbilityOption(this, ability));
        }
    }

    // this function is called when the EnemyUnit needs to know what it's going to do
    // it evaluates its possibleActions, selects the best one, and asks it to create ActionData to store
    public void NewActionData()
    {
        if (possibleActions.Count != 0)
        {
            // get data about the board-state, possible options, etc
            MoveableTiles = FindMoveableTiles(MapController.instance.weightedMap);
            
            foreach(AbilityOption option in possibleAbilities)
            { option.EvaluateAffectableTiles(); }

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

            // This useful Debug statement will print to the console the details of what this Unit committed to doing
            Debug.Log(this.GetName() + " at Starting Position " + this.GetMapPosition()
                + " has committed to moving to " + plannedActionData.GetEndingPosition() + " and using " 
                + this.AvailableAbilities[plannedActionData.GetAbilityIndex()] + " in direction " 
                + plannedActionData.GetAbilityDirection());
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

    public List<AbilityOption> GetAbilityOptions()
    {
        return possibleAbilities;
    }

    public void ScanMap()
    {
        shortestPaths = MapController.instance.ScanFromStart(GetMapPosition());
    }

    // determines if this Enemy Unit is "boxed in" by other obstructions, limiting its movement
    // the function updates the boxedIn attribute
    public void EvaluateBoxIn()
    {
        foreach(int i in shortestPaths.Keys)
        {
            if (i >= this.moveSpeed && shortestPaths[i].Keys.Count >= 0)
            {
                boxedIn = false;
                return;
            }
        }
        boxedIn = true;
        return;
    }
   
}
