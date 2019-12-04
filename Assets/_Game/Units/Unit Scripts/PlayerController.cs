using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMOD.Studio;

// PlayerController is an object that inherits from the UnitController (see UnitController.cs)
// It is a variety that is user-controlled
// see the UnitController Abstract Class for more information about how this class ought to behave

public class PlayerController : UnitController
{
    new public const int TURN_WEIGHT = -1;
    bool tabbed;
    //int distanceSoFar;
    //Vector2Int lastPosition;

    public static int ability;

    //Stack<Vector2Int> pivots = new Stack<Vector2Int>();
    //Stack<int> distances = new Stack<int>();
    //Stack<Direction> directions = new Stack<Direction>();

    public GameObject abilityPanel;
    public GameObject UI;
    public GameObject directionSelector;
    public int playerID;

    public override void Update()
    {
        // if it's not currently this controller's turn, the controller has no Units, or there are active events, it's not allowed to do anything
        if (!myTurn || units.Count == 0 || EventsManager.instance.EventsExist())
        { return; }

        AlliedUnit theUnit = units[activeUnit] as AlliedUnit;

        //left click to choose unit
        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int selectedLoc = MapUIController.instance.cursorPosition;
            int? selectedIndex = FindUnit(selectedLoc);
            if(selectedIndex != null)
            {
                ClearSpotlight();
                SelectUnit((int)selectedIndex);
            }

        }

        //check for tab input
        //select next unit
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GetNextUnit();
            
            return;
        }

        if(theUnit.hasMoved && !theUnit.hasPivoted)
        {
            Vector2Int unitPosition = units[activeUnit].GetMapPosition();
            //Vector3 truePosition = MapMath.MapToWorld(unitPosition);
            directionSelector.transform.position = MapMath.MapToWorld(unitPosition);
            directionSelector.transform.position = new Vector3(directionSelector.transform.position.x, directionSelector.transform.position.y + 1.506f, directionSelector.transform.position.z);
            directionSelector.SetActive(true);
            theUnit.ChooseDirection();
            return;
        }

        // if the current unit has moved but hasn't attacked, it needs to select an ability
        if (theUnit.hasMoved && theUnit.hasPivoted && !theUnit.hasActed)
        {
            directionSelector.SetActive(false);
            abilityPanel.SetActive(true);
            theUnit.ChooseAbility();
            return;
        }
        else
        { abilityPanel.SetActive(false); }
        

        // if the current unit has moved and attacked, get the next unit
        if (theUnit.hasMoved && theUnit.hasPivoted && theUnit.hasActed)
        {
            abilityPanel.SetActive(false);
            GetNextUnit();
            return;
        }
        //Only process if the unit has not moved
        if (!theUnit.hasMoved)
        {
            // ----Do Movement----
            Vector2Int dest = MapUIController.instance.cursorPosition;

            if (Input.GetMouseButtonDown(0))
            {
                if (theUnit.plannedPath.Contains(dest)) // Commit to the path
                {
                    Queue<Vector2Int> movementPath = new Queue<Vector2Int>();
                    for (int i = 0; i < theUnit.plannedPath.Count; i++)
                    {
                        movementPath.Enqueue(theUnit.plannedPath[i]);
                        if (theUnit.plannedPath[i] == dest)
                            break;
                    }
                    StartCoroutine(theUnit.Move(movementPath, MovementType.WALK));
                    ClearSpotlight();
                }
                else
                {
                    theUnit.DisplayShortestPath(dest);
                }
            }
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

    public override void GetNextUnit()
    {
        ClearSpotlight();

        if (IsTurnOver())
        {
            RelinquishPower();
            CameraController.instance.targetZoom = 5;
        }

        else
        {
            SelectUnit(GetNextIndex());
        }

        UI.GetComponent<UI_Operator>().InitializeUI();
    }

    public int? FindUnit(Vector2Int selected)
    {
        for(int i = 0; i < units.Count; i++)
        {
            if (units[i].GetMapPosition().Equals(selected))
                return (int?)i;
        }
        return null;
    }

    // clears highlighting for relevant tiles on the current indexed unit
    public void ClearSpotlight()
    {
        MapUIController.instance.ClearPathHighlight();
        MapUIController.instance.ClearRangeHighlight();
        units[activeUnit].UnhighlightUnit();
        //pivots.Clear();
        //distances.Clear();
        //directions.Clear();
    }

    // highlights relevant tiles for the current index, whatever it is
    public void SpotlightActiveUnit()
    {
        (units[activeUnit] as AlliedUnit).DisplayMovementTiles();
        CameraController.instance.targetPos = units[activeUnit].transform.position;
        units[activeUnit].HighlightUnit();
        //pivots.Push(units[activeUnit].GetMapPosition());
        //distances.Push(0);
        //directions.Push(units[activeUnit].GetDirection());
        (units[activeUnit] as AlliedUnit).plannedPath.Clear();
    }

    // selects the unit at the given Index, highlighting the relevant tiles and setting necessary data
    public void SelectUnit(int newIndex)
    {
        // set up the new unit
        setActiveUnit(newIndex);
        SpotlightActiveUnit();
        UI.GetComponent<UI_Operator>().unit = units[activeUnit];
        UI.GetComponent<UI_Operator>().SetInfo();
        
        directionSelector.SetActive(false);
        if (units[activeUnit].selectSoundEvent.isValid())
        {
            units[activeUnit].selectSoundEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            units[activeUnit].selectSoundEvent.start();
        }
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
