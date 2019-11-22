using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An EnemyController is an object that inherits from the UnitController (see UnitController.cs)
// It is a variety that is NOT user-controlled, but instead controlled by an AI.
// see the UnitController Abstract Class for more information about how this class ought to behave

public class EnemyController : UnitController
{
    // override of the base UnitController's TURN_WEIGHT
    new public const int TURN_WEIGHT = 1;

    public static int ability;

    //public GameObject abilityPanel;
    //public GameObject UI;
    public int playerID;


    private void OnEnable()
    {
        Graveyard.NewEnemiesEvent.AddListener(AddUnits);
    }

    private void OnDisable()
    {
        Graveyard.NewEnemiesEvent.RemoveListener(AddUnits);
    }

    public override void Update()
    {
        // if it's not currently this controller's turn, it's not allowed to do anything
        if (!myTurn)
        { return; }

        // dead men tell no... uh... commands to the game engine
        if (units[activeUnit].GetHealth() <= 0)
        { GetNextUnit(); }

        // No logic runs without a unit.
        if (units.Count == 0) { return; }

        // if the unit hasn't moved or attacked, get new ActionData and move
        if (!units[activeUnit].hasMoved && !units[activeUnit].hasActed)
        {
            (units[activeUnit] as EnemyUnit).ScanMap();
            (units[activeUnit] as EnemyUnit).EvaluateBoxIn();
            if ((units[activeUnit] as EnemyUnit).boxedIn && !TotalBoxIn())
            {
                GetNextUnit();
                return;
            }

            (units[activeUnit] as EnemyUnit).NewActionData();
            (units[activeUnit] as EnemyUnit).ChooseMovement();
            return;
        }

        // if the current unit has moved but hasn't attacked, it needs to select an ability
        if (units[activeUnit].hasMoved && !units[activeUnit].hasActed)
        {
            (units[activeUnit] as EnemyUnit).ChooseAbility();
            return;
        }

        // if the current unit hasn't moved but has acted, it needs to move
        if (!units[activeUnit].hasMoved && units[activeUnit].hasActed)
        {
            (units[activeUnit] as EnemyUnit).ChooseMovement();
            return;
        }

        // if the current unit has moved and acted, get the next unit
        if (units[activeUnit].hasMoved && units[activeUnit].hasActed)
        {
            GetNextUnit();
            return;
        }
    }

    public override void RelinquishPower()
    {
        if (IsMyTurn())
        {
            ResetUnits();
            EndTurnEvent.Invoke(this);
            //UI.GetComponent<UI_Operator>().SetPhaseText(playerID);
            //UI.GetComponent<UI_Operator>().PhaseTextDisplay();
        }
    }

    public override void GetNextUnit()
    {
        if (IsTurnOver())
        {
            RelinquishPower();
            CameraController.instance.targetZoom = 5;
        }

        else
        {
            setActiveUnit(GetNextIndex());
        }
    }

    // returns whether or not there is a Total Box-In
    // a Total Box-In is when every Unit that can still take actions is classified as "Boxed-In"
    public bool TotalBoxIn()
    {
        foreach (Unit unit in units)
        {
            // IF   the Unit is not boxed in    AND   the Unit has either not acted or not moved
            if ( !((unit as EnemyUnit).boxedIn) && (!unit.hasActed || !unit.hasMoved))
                return false;
        }
        return true;
    }

    public void AbilityManual(int abilityID)
    {
        ability = abilityID;
    }

    public override int GetWeight()
    {
        return TURN_WEIGHT;
    }
}
