using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An EnemyController is an object that inherits from the UnitController (see UnitController.cs)
// It is a variety that is NOT (Shouldn't be, but is right now lol) user-controlled
// see the UnitController Abstract Class for more information about how this class ought to behave
// work in progress... currently just a copy-paste of PlayerController

public class EnemyController : UnitController
{
    // override of the base UnitController's TURN_WEIGHT
    new public const int TURN_WEIGHT = 1;

    public static int ability;

    public GameObject abilityPanel;
    public GameObject UI;
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

        // No logic runs without a unit.
        if (units.Count == 0) { return; }

        // if the unit hasn't moved or attacked, get new ActionData and move
        if (!units[activeUnit].hasMoved && !units[activeUnit].hasActed)
        {
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
            UI.GetComponent<UI_Operator>().SetPhaseText(playerID);
            UI.GetComponent<UI_Operator>().PhaseTextDisplay();
        }
    }

    public void GetNextUnit()
    {
        if (IsTurnOver())
        {
            RelinquishPower();
            CameraController.instance.targetZoom = 5;
        }

        else
        {
            SelectUnit(GetNextIndex());
        }
    }

    // selects the unit at the given Index, setting necessary data
    public void SelectUnit(int newIndex)
    {
        // set up the new unit
        setActiveUnit(newIndex);
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
