﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// PlayerController is an object that inherits from the UnitController (see UnitController.cs)
// It is a variety that is user-controlled
// see the UnitController Abstract Class for more information about how this class ought to behave

public class PlayerController : UnitController
{
    new public const int TURN_WEIGHT = -1;


    public override void Update()
    {
        // if it's not currently this controller's turn, it's not allowed to do anything
        if (!myTurn)
        {
            return;
        }

        //check for tab input
        //select next unit
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MapUIController.instance.tileHighlighting.ClearAllTiles();
            MapUIController.instance.pathHighlighting.ClearAllTiles();
            GetNextUnit();
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Start Tile highlighting code
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100f);
            Vector2Int dest = MapMath.WorldToMap(hit.point);
            AlliedUnit theUnit = units[activeUnit] as AlliedUnit;
            if ( (!theUnit.plannedPath.Contains(dest)) || theUnit.plannedPath[theUnit.plannedPath.Count - 1] != dest )
            {
                theUnit.DisplayPath(dest);
            }
            else
            {
                theUnit.Move(dest.x, dest.y);
                GetNextUnit();
            }
        }
    }


    private void GetNextUnit()
    {
        if (isTurnOver())
        {
            resetUnits();
            endTurnEvent.Invoke(this as PlayerController);
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
        }
    }

}
