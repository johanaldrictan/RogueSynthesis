﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AlliedUnit: A type of Unit that is controller by the Player
// Refer to Unit.cs if you want to see how a Unit ought to behave

public class AlliedUnit : Unit
{
    [System.NonSerialized] public List<Vector2Int> plannedPath;

    // positionMemory is a record of where a unit was, after they moved, and before they acted
    // if the action should be cancelled, the unit will remember where it should go back to
    [SerializeField] public Vector2Int positionMemory;
    [SerializeField] public Direction directionMemory;

    public override void Awake()
    {
        hasPivoted = false;
        hasActed = false;
        hasMoved = false;
        plannedPath = new List<Vector2Int>();
        m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
        m_SpriteRenderer.sortingOrder = 99;
        Deaths = new Stack<DeathData>();
    }
    
    // this function highlights tiles that this unit instance can move to
    public void DisplayMovementTiles()
    {
        //clear tilemap
        MapUIController.instance.tileHighlighting.ClearAllTiles();
        if (!hasMoved)
        {
            Dictionary<Vector2Int, Direction>.KeyCollection moveLocs = FindMoveableTiles(MapController.instance.weightedMap).Keys;
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


    // this function is called in the Update Loop in other modules
    // therefore, you can treat this function as being called every frame under the correct conditions
    // conditions: the unit's hasMoved is True, but its hasActed is False
    // meant to get the unit to choose an ability or cancel its movement
    // THIS CURRENT SOLUTION IS TEMPORARY
    public override void ChooseAbility()
    {
        // 0 on the NumPad (wait)
        if (Input.GetKeyDown(KeyCode.Keypad0) || PlayerController.ability == 1)
        {
            AvailableAbilities[0].Execute(this, direction); // CHANGE THIS
            hasActed = true;
            PlayerController.ability = 0;
            return;
        }

        // 1 on the NumPad
        if (Input.GetKeyDown(KeyCode.Keypad1) || PlayerController.ability == 2)
        {
            AvailableAbilities[1].Execute(this, direction); // CHANGE THIS
            hasActed = true;
            PlayerController.ability = 0;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Keypad2) || PlayerController.ability == 3)
        {
            AvailableAbilities[2].Execute(this, direction); // CHANGE THIS
            hasActed = true;
            PlayerController.ability = 0;
            return;
        }
        if (Input.GetKeyDown(KeyCode.Keypad3) || PlayerController.ability == 4)
        {
            AvailableAbilities[3].Execute(this, direction); // CHANGE THIS
            hasActed = true;
            PlayerController.ability = 0;
            return;
        }

        // Esc Key = cancel
        else if (Input.GetKeyDown(KeyCode.Escape) || PlayerController.ability == 3)
        {
            Queue<Vector2Int> destination = new Queue<Vector2Int>();
            destination.Enqueue(positionMemory);
            StartCoroutine(Move(destination, MovementType.TELEPORT));
            ChangeDirection(directionMemory);
            hasMoved = false;
            hasPivoted = false;
            PlayerController.ability = 0;
            return;
        }
    }
    public void DisplayShortestPath(Vector2Int dest)
    {
        MapUIController.instance.ClearPathHighlight();
        Stack<Vector2Int> path = GetMovementPath(FindMoveableTiles(MapController.instance.weightedMap), dest);
        plannedPath.Clear();

        if (path == null) { return; }

        plannedPath.AddRange(path);
        while (path.Count != 0)
        {
            Vector2Int loc = path.Pop();
            MapUIController.instance.PathHighlight(loc, true);
        }
    }
    //ANDREW NEEDS TO WORK ON GETTING BUTTONS FOR THIS WORKING AS WELL
    public void ChooseDirection()
    {
        directionMemory = this.direction;
        if(Input.GetKeyDown(KeyCode.UpArrow) || ArrowSelection.directionID == 2)
        {
            this.ChangeDirection(Direction.N);
            hasPivoted = true;
            ArrowSelection.directionID = 0;
            return;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || ArrowSelection.directionID == 4)
        {
            this.ChangeDirection(Direction.S);
            hasPivoted = true;
            ArrowSelection.directionID = 0;
            return;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || ArrowSelection.directionID == 1)
        {
            this.ChangeDirection(Direction.W);
            hasPivoted = true;
            ArrowSelection.directionID = 0;
            return;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || ArrowSelection.directionID == 3)
        {
            this.ChangeDirection(Direction.E);
            hasPivoted = true;
            ArrowSelection.directionID = 0;
            return;
        }
    }
}





