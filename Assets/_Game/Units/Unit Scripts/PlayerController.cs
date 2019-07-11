using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// PlayerController is an object that inherits from the UnitController (see UnitController.cs)
// It is a variety that is user-controlled
// see the UnitController Abstract Class for more information about how this class ought to behave

public class PlayerController : UnitController
{
    new public const int TURN_WEIGHT = -1;
    bool tabbed;
    int distanceSoFar;
    Vector2Int lastPivot;

    Stack<Vector2Int> pivots = new Stack<Vector2Int>();
    Stack<int> distances = new Stack<int>();
    Stack<Direction> directions = new Stack<Direction>();



    public override void Update()
    {
        

        // if it's not currently this controller's turn, it's not allowed to do anything
        if (!myTurn)
        { return; }

        // No logic runs without a unit.
        if (units.Count == 0) { return; }

        // if the current unit has moved but hasn't attacked, it needs to select an ability
        if (units[activeUnit].hasMoved && !units[activeUnit].hasAttacked)
        {
            units[activeUnit].chooseAbility();
            return;
        }

        // if the current unit has moved and attacked, get the next unit
        if (units[activeUnit].hasMoved && units[activeUnit].hasAttacked)
        {
            GetNextUnit();
            return;
        }

        //check for tab input
        //select next unit
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GetNextUnit();
            return;
        }

        // ----Do Movement----
        AlliedUnit theUnit = units[activeUnit] as AlliedUnit;
        // Get old stuff
        if (distances.Count != 0) { distanceSoFar = distances.Peek(); }
        if (pivots.Count != 0) { lastPivot = pivots.Peek(); }
        // Find the tile the cursor most points to.
        if (pivots.Count == 0) { pivots.Push(theUnit.mapPosition); }
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
        if (!Unit.FindMoveableTilesStraight(MapController.instance.map, lastPivot, theUnit.moveSpeed - distanceSoFar).ContainsKey(diff + lastPivot))
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

                    Debug.Assert(distanceSoFar <= theUnit.moveSpeed);
                    distances.Push(distanceSoFar);
                    pivots.Push(lastPivot + diff);

                    if (diff.y < 0) { directions.Push(Direction.N); }
                    if (diff.y > 0) { directions.Push(Direction.S); }
                    if (diff.x > 0) { directions.Push(Direction.E); }
                    if (diff.x < 0) { directions.Push(Direction.W); }

                    MapUIController.instance.tileHighlighting.ClearAllTiles();
                    foreach (Vector2Int tile in Unit.FindMoveableTiles(MapController.instance.map, pivots.Peek(), theUnit.moveSpeed - distanceSoFar).Keys)
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
                foreach (Vector2Int tile in Unit.FindMoveableTiles(MapController.instance.map, pivots.Peek(), theUnit.moveSpeed - distances.Peek()).Keys)
                {
                    MapUIController.instance.RangeHighlight(tile);
                }
            }
        }

        // if (Input.GetMouseButtonDown(0))
        // {
        //Start Tile highlighting code
        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100f);
        // Vector2Int dest = MapMath.WorldToMap(hit.point);
        // if ( (!theUnit.plannedPath.Contains(dest)) || theUnit.plannedPath[theUnit.plannedPath.Count - 1] != dest )
        // {
        //     theUnit.DisplayPath(dest);
        // }
        // else
        // {
        //     theUnit.Move(dest.x, dest.y);
        //     GetNextUnit();
        // }
        // }
    }


    private void GetNextUnit()
    {
        // clean up last unit
        MapUIController.instance.ClearPathHighlight();
        MapUIController.instance.ClearRangeHighlight();
        pivots.Clear();
        distances.Clear();
        directions.Clear();

        if (IsTurnOver())
        {
            endTurnEvent.Invoke(this as PlayerController);
            ResetUnits();
            CameraController.instance.targetZoom = 5;
        }
        else if (units.Count != 0)
        {
            // go through the list, skipping units that are inactive in the heirarchy, or have both moved and attacked already
            do
            {
                activeUnit = (activeUnit + 1) % units.Count;
            }
            while (!units[activeUnit].gameObject.activeInHierarchy || (units[activeUnit].hasAttacked && units[activeUnit].hasMoved) );

            units[activeUnit].DisplayMovementTiles();
            CameraController.instance.targetPos = units[activeUnit].transform.position;
            pivots.Push(units[activeUnit].mapPosition);
            distances.Push(0);
            directions.Push(units[activeUnit].direction);
            (units[activeUnit] as AlliedUnit).plannedPath.Clear();
        }
    }

}
