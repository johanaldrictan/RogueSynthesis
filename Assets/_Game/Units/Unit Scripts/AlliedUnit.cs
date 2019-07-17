using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlliedUnit : Unit
{
    [System.NonSerialized] public List<Vector2Int> plannedPath;

    // positionMemory is a record of where a unit was, after they moved, and before they acted
    // if the action should be cancelled, the unit will remember where it should go back to
    [SerializeField] public Vector2Int positionMemory;

    // similar to positionMemory, directionMemory functions the same, but stores the direction
    [SerializeField] public Direction directionMemory;

    public override void Awake()
    {
        hasActed = false;
        hasMoved = false;
        plannedPath = new List<Vector2Int>();
        m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
        m_SpriteRenderer.sortingOrder = 99;
    }
    
    // this function highlights tiles that this unit instance can move to
    public override void DisplayMovementTiles()
    {
        //clear tilemap
        MapUIController.instance.tileHighlighting.ClearAllTiles();
        if (!hasMoved)
        {
            Dictionary<Vector2Int, Direction>.KeyCollection moveLocs = FindMoveableTiles(MapController.instance.map).Keys;
            foreach (Vector2Int loc in moveLocs)
            {
                MapUIController.instance.RangeHighlight(loc);
            }
        }
    }
    

    public void DisplayPlannedPath()
    {
        bool lastWasNS = false;
        Vector2Int last = mapPosition;
        foreach (Vector2Int loc in this.plannedPath)
        {
            if (last == mapPosition)
            {
                lastWasNS = (last - loc).x == 0;
                MapUIController.instance.PathHighlight(last, lastWasNS);
            }
            else if (lastWasNS != ((last - loc).x == 0))
            {
                lastWasNS = !lastWasNS;
                MapUIController.instance.PathHighlight(last, lastWasNS);
            }
            MapUIController.instance.PathHighlight(loc, lastWasNS);
            last = loc;
        }
    }

    public override void Move(int x, int y)
    {
        // remember where we started first
        positionMemory = mapPosition;
        directionMemory = direction;

        // remove old coordinates from globalPositionalData
        globalPositionalData.RemoveUnit(mapPosition);

        // clear the highlighting
        MapUIController.instance.ClearPathHighlight();
        MapUIController.instance.ClearRangeHighlight();

        //restore old tilevalue
        MapController.instance.map[mapPosition.x, mapPosition.y] = (int)tile;

        // set new coordinates
        mapPosition.x = x;
        mapPosition.y = y;

        // update the globalPositionalData
        globalPositionalData.AddUnit(mapPosition, this);

        tile = (TileWeight)MapController.instance.map[mapPosition.x, mapPosition.y];
        MapController.instance.map[mapPosition.x, mapPosition.y] = (int)TileWeight.OBSTRUCTED;
        this.transform.position = MapMath.MapToWorld(x,y);
        plannedPath.Clear();
        hasMoved = true;
    }


    // this function is called in the Update Loop in other modules
    // therefore, you can treat this function as being called every frame under the correct conditions
    // conditions: the unit's hasMoved is True, but its hasActed is False
    // meant to get the unit to choose an ability or cancel its movement
    // THIS CURRENT SOLUTION IS TEMPORARY
    public override void chooseAbility()
    {
        // 0 on the NumPad
        if (Input.GetKeyDown(KeyCode.Keypad0) || PlayerController.wait == true)
        {
            availableAbilities[0].Execute(this);
            hasActed = true;
            PlayerController.wait = false;
            return;
        }

        // 1 on the NumPad
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            availableAbilities[1].Execute(this);
            hasActed = true;
            return;
        }

        // Esc Key = cancel
        else if (Input.GetKeyDown(KeyCode.Escape) || PlayerController.cancel == true)
        {
            Move(positionMemory.x, positionMemory.y);
            ChangeDirection(directionMemory);
            hasMoved = false;
            PlayerController.cancel = false;
            return;
        }
    }

}





//public void DisplayShortestPath(Vector2Int dest)
//{
//    MapUIController.instance.ClearPathHighlight();
//    Stack<Vector2Int> path = GetMovementPath(FindMoveableTiles(MapController.instance.map), dest);
//    //plannedPath.Clear();

//    if (path == null) { return; }

//    //plannedPath.AddRange(path);
//    while(path.Count != 0)
//    {
//        Vector2Int loc = path.Pop();    
//        MapUIController.instance.PathHighlight(loc);
//    }
//}