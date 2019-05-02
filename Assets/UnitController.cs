using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// UnityEvent class; creates an event that passes a UnitController with it
public class UnitControllerUnityEvent : UnityEvent<UnitController> { }

// UnitController is an Abstract Base Class for an object that stores and commands Unit objects
public abstract class UnitController : MonoBehaviour
{
    public UnitController instance;

    private bool myTurn;

    public int maxTeamSize = 3;

    // the weight used by the turn system to determine order of the controllers
    // lower is faster
    private const int TURN_WEIGHT = 0;

    // storage of units
    // likely to be changed later
    public Unit[] units;
    
    // index of currently active unit
    public int activeUnit;

    // Event for queueing up for the TurnController's initialization
    public static UnitControllerUnityEvent queueUpEvent = new UnitControllerUnityEvent();

    // Event for asking to end this controller's turn
    public static UnityEvent endTurnEvent = new UnityEvent();


    public virtual void Awake()
    {
        instance = this;
        myTurn = true;
        activeUnit = 0;
    }

    public virtual void Start()
    {
        // I would like to be added to the TurnController
        queueUpEvent.Invoke(instance);
    }

    public virtual void Update()
    {

    }

    /*spawn unit at x,y in map units
    public Unit SpawnUnit(int x, int y)
    {
        Vector3 playerPos = MapMath.MapToWorld(x, y);
        return Instantiate(unitPrefab.gameObject, playerPos, Quaternion.identity).GetComponent<AlliedUnit>();
    }
    */

    // determine whether this controller is ready to end its turn
    public virtual bool isTurnOver()
    {
        for (int i = 0; i < maxTeamSize; i++)
        {
            // if a unit that hasn't moved or attacked exists
            // Debug.Log(units[i].hasAttacked);
            // Debug.Log(units[i].hasMoved);
            if ( units[i].GetType().IsSubclassOf(typeof(Unit)) && (!units[i].hasAttacked || !units[i].hasMoved) )
            {
                // Debug.Log("Turn not over");
                return false;
            }
        }
        return true;
    }

    public virtual void resetUnits()
    {
        for (int i = 0; i < maxTeamSize; i++)
        {
            if (units[i].GetType().IsSubclassOf(typeof(Unit)))
            {
                units[i].hasAttacked = false;
                units[i].hasMoved = false;
            }
        }
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