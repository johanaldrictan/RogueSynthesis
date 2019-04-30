﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// UnityEvent class; creates an event that passes a UnitController with it
public class UnitControllerUnityEvent : UnityEvent<UnitController> { }

// UnitController is a Base Class for an object that stores and commands Unit objects
public abstract class UnitController : MonoBehaviour
{

    // the maximum number of units allowed to be stored
    public const int MAX_TEAM_SIZE = 3;

    // the weight used by the turn system to determine order of the controllers
    // lower is faster
    public const int TURN_WEIGHT = 0;

    // is it my turn right now?
    public bool myTurn;

    // stores the unit used by the controller;
    // likely to be changed later
    public Unit unitPrefab;


    // storage of units
    // likely to be changed later
    public Unit[] units;

    // index of currently active unit
    // may be changed later
    public int activeUnit;

    // Event for queueing up for the TurnController's initialization
    public UnitControllerUnityEvent queueUpEvent = new UnitControllerUnityEvent();

    // Event for asking to end this controller's turn
    public UnityEvent endTurnEvent = new UnityEvent();

    public static UnitController instance;


    public virtual void Awake()
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

        // reset storage
        units = new Unit[MAX_TEAM_SIZE];
        myTurn = true;
        activeUnit = 0;
    }

    public virtual void Start()
    {
        // I would like to be added to the TurnController
        queueUpEvent.Invoke(this);
    }

    public virtual void Update() {}

    //spawn unit at x,y in map units
    public Unit SpawnUnit(int x, int y)
    {
        Vector3 playerPos = MapMath.MapToWorld(x, y);
        return Instantiate(unitPrefab.gameObject, playerPos, Quaternion.identity).GetComponent<AlliedUnit>();
    }


    // determine whether this controller is ready to end its turn
    public virtual bool isTurnOver()
    {
        for (int i = 0; i < MAX_TEAM_SIZE; i++)
        {
            // if a unit that hasn't moved or attacked exists
            if (units[i].GetType() == typeof(Unit) && (!units[i].hasAttacked || !units[i].hasMoved))
            {
                return false;
            }
        }
        return true;
    }

    public bool isMyTurn()
    { return myTurn; }

    public void startTurn()
    { myTurn = true; }

    public void endTurn()
    { myTurn = false; }

    public int getWeight()
    { return TURN_WEIGHT; }

    public string getName()
    { return name; }

}

public enum HoverState
{
    NONE,
    HOVER
}