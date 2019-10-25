using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An EnemyController is an object that inherits from the UnitController (see UnitController.cs)
// It is a variety that is NOT (Shouldn't be, but is right now lol) user-controlled
// see the UnitController Abstract Class for more information about how this class ought to behave
// work in progress... currently just a copy-paste of PlayerController

public class EnemyController : UnitController
{
    new public const int TURN_WEIGHT = 1;
    bool tabbed;
    int distanceSoFar;
    Vector2Int lastPivot;

    public static int ability;

    Stack<Vector2Int> pivots = new Stack<Vector2Int>();
    Stack<int> distances = new Stack<int>();
    Stack<Direction> directions = new Stack<Direction>();

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

        // if the current unit has moved but hasn't attacked, it needs to select an ability
        if (units[activeUnit].hasMoved && !units[activeUnit].hasActed)
        {
            abilityPanel.SetActive(true);
            units[activeUnit].ChooseAbility();
            return;
        }
        else
        { abilityPanel.SetActive(false); }


        // if the current unit has moved and attacked, get the next unit
        if (units[activeUnit].hasMoved && units[activeUnit].hasActed)
        {
            abilityPanel.SetActive(false);
            GetNextUnit();
            return;
        }

        //check for tab input
        //select next unit
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GetNextUnit();
            UI.GetComponent<UI_Operator>().InitializeUI();
            return;
        }

        // ----Do Movement----
        EnemyUnit theUnit = units[activeUnit] as EnemyUnit;
        // Get old stuff
        if (distances.Count != 0) { distanceSoFar = distances.Peek(); }
        if (pivots.Count == 0) { pivots.Push(theUnit.GetMapPosition()); }
        // Find the tile the cursor most points to.
        lastPivot = pivots.Peek();
        Vector2Int diff = MapUIController.instance.cursorPosition - lastPivot;
        if (Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
        {
            diff.x = 0;
        }
        else
        {
            diff.y = 0;
        }
        // Check if cursor's snapped position is on a valid straight path.
        // TODO: Maybe dont run this every frame. (i mean its cheaper than whole dijkstras)
        if (!Unit.FindMoveableTilesStraight(MapController.instance.map, lastPivot, theUnit.GetMoveSpeed() - distanceSoFar).ContainsKey(diff + lastPivot))
        {
            // Draw a red arrow for invalid path
        }
        else
        {
            // Draw the path so far
            MapUIController.instance.ClearPathHighlight();
            theUnit.DisplayPlannedPath();
            // Draw the new part after the pivot
            MapUIController.instance.PathHighlight(lastPivot, lastPivot + diff);

            if (Input.GetMouseButtonDown(0))
            {
                if (diff == Vector2Int.zero) // Commit to the path
                {
                    theUnit.Move(lastPivot.x, lastPivot.y);
                    theUnit.ChangeDirection(directions.Peek());
                    ClearSpotlight();
                }
                else // Extend Path
                {
                    // grr. copy paste
                    for (int y = 1; y <= diff.y; y++)
                    {
                        distanceSoFar += MapController.instance.map[lastPivot.x, lastPivot.y + y];
                        theUnit.plannedPath.Add(lastPivot + Vector2Int.up * y);
                    }
                    for (int y = 1; y <= -diff.y; y++)
                    {
                        distanceSoFar += MapController.instance.map[lastPivot.x, lastPivot.y - y];
                        theUnit.plannedPath.Add(lastPivot + Vector2Int.down * y);
                    }
                    for (int x = 1; x <= diff.x; x++)
                    {
                        distanceSoFar += MapController.instance.map[lastPivot.x + x, lastPivot.y];
                        theUnit.plannedPath.Add(lastPivot + Vector2Int.right * x);
                    }
                    for (int x = 1; x <= -diff.x; x++)
                    {
                        distanceSoFar += MapController.instance.map[lastPivot.x - x, lastPivot.y];
                        theUnit.plannedPath.Add(lastPivot + Vector2Int.left * x);
                    }

                    Debug.Assert(distanceSoFar <= theUnit.GetMoveSpeed());
                    distances.Push(distanceSoFar);
                    pivots.Push(lastPivot + diff);

                    if (diff.y > 0) { directions.Push(Direction.N); }
                    if (diff.y < 0) { directions.Push(Direction.S); }
                    if (diff.x > 0) { directions.Push(Direction.E); }
                    if (diff.x < 0) { directions.Push(Direction.W); }

                    MapUIController.instance.tileHighlighting.ClearAllTiles();
                    foreach (Vector2Int tile in Unit.FindMoveableTiles(MapController.instance.map, pivots.Peek(), theUnit.GetMoveSpeed() - distanceSoFar).Keys)
                    {
                        MapUIController.instance.RangeHighlight(tile);
                    }

                    MapUIController.instance.ClearPathHighlight();
                    theUnit.DisplayPlannedPath();
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(0))
        {
            if (pivots.Count > 1)
            {
                pivots.Pop();
                distances.Pop();
                directions.Pop();

                while (theUnit.plannedPath.Count != 0 && theUnit.plannedPath[theUnit.plannedPath.Count - 1] != pivots.Peek())
                {
                    theUnit.plannedPath.RemoveAt(theUnit.plannedPath.Count - 1);
                }

                MapUIController.instance.ClearPathHighlight();
                theUnit.DisplayPlannedPath();
                MapUIController.instance.tileHighlighting.ClearAllTiles();
                foreach (Vector2Int tile in Unit.FindMoveableTiles(MapController.instance.map, pivots.Peek(), theUnit.GetMoveSpeed() - distances.Peek()).Keys)
                {
                    MapUIController.instance.RangeHighlight(tile);
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

    public void GetNextUnit()
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
    }

    // selects the unit at the given Index, highlighting the relevant tiles and setting necessary data
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
