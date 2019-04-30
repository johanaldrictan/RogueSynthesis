using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// PlayerController is an object that inherits from the UnitController (see UnitController.cs)
// It is a variety that is user-controlled
public class PlayerController : UnitController
{
    new public const int TURN_WEIGHT = -1;

    public override void Awake()
    {
        // there can only be one
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        activeUnit = 0;
        myTurn = true;

        // this is just to test
        units = new AlliedUnit[MAX_TEAM_SIZE];
        units[0] = SpawnUnit(0, 0);
        units[1] = SpawnUnit(5, 5);
        units[2] = SpawnUnit(2, 2);
    }

    // Start is called before the first frame update
    public override void Start()
    {
        // I would like to be added to the TurnController
        queueUpEvent.Invoke(this);
    }

    // Update is called once per frame
    public override void Update()
    {
        // if it's not currently this controller's turn, it's not allowed to do anything
        if (!myTurn)
        {
            return;
        }
        else if (isTurnOver())
        {
            // it is still my turn and it is supposed to be over
            // I would like to end my turn now
            endTurnEvent.Invoke();
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
            if (!theUnit.plannedPath.Contains(dest))
            {
                theUnit.DisplayPath(dest);
            }
            else
            {
                units[activeUnit].Move(dest.x, dest.y);
                GetNextUnit();
            }
        }
    }


    private void GetNextUnit()
    {
        if (units.Length != 0)
        {
            int i = activeUnit + 1;
            int n = i % MAX_TEAM_SIZE;
            bool unitFound = false;
            while (!unitFound)
            {
                if (units[i % MAX_TEAM_SIZE] != null && units[i % MAX_TEAM_SIZE].gameObject.activeInHierarchy)
                {
                    //Debug.Log("Switch");
                    activeUnit = i % MAX_TEAM_SIZE;
                    units[activeUnit].DisplayMovementTiles();
                    unitFound = true;
                }
                i++;
                if (units[i % MAX_TEAM_SIZE] == null && i % MAX_TEAM_SIZE == n)
                {
                    return;
                }
            }
        }
    }

    // return true if every unit in the units list has both moved and acted
    public override bool isTurnOver()
    {
        for (int i = 0; i < MAX_TEAM_SIZE; i++)
        {
            if (units[i].GetType() == typeof(AlliedUnit) && (!units[i].hasAttacked || !units[i].hasMoved))
            {
                return false;
            }
        }
        return true;
    }
}
