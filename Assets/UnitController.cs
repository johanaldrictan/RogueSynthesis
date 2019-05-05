using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// UnityEvent class; creates an event that passes a UnitController with it
public class UnitControllerUnityEvent : UnityEvent<UnitController> { }

// UnitController is an Abstract Base Class for an object that stores and commands Unit objects
public abstract class UnitController : MonoBehaviour
{

    public bool myTurn;

    // the weight used by the turn system to determine order of the controllers
    // lower is faster
    public const int TURN_WEIGHT = 0;

    public int activeUnit;

    // storage of real units
    [System.NonSerialized] public List<Unit> units;

    // Serialized (Editable in Unity's Inspector) List of data used for initially spawning units
    [SerializeField] public List<UnitDataContainer> unitSpawnData;


    // Event for queueing up to be added to a TurnController
    public static UnitControllerUnityEvent queueUpEvent = new UnitControllerUnityEvent();

    // Event for asking a TurnController to end this controller's turn
    public static UnitControllerUnityEvent endTurnEvent = new UnitControllerUnityEvent();

    // Every class that inherits from this one must create a method 
    // that translates unitSpawnData into real units for the units list
    public abstract void loadUnits();

    // this should (in theory) always be overridden in subclasses.
    // Things done here should probably be done there a well.
    public virtual void Awake()
    {
        myTurn = true;
        activeUnit = 0;
    }

    // this will probably be overridden most of the time ( like Awake() )
    // remember to invoke the queueUpEvent if so
    public virtual void Start()
    {
        // I would like to be added to the TurnController
        queueUpEvent.Invoke(this);
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
        for (int i = 0; i < units.Count; i++)
        {
            // if a unit that hasn't moved or attacked exists
            if (!units[i].hasAttacked || !units[i].hasMoved)
            {
                // Debug.Log("Turn not over");
                return false;
            }
        }
        // Debug.Log("Turn over");
        return true;
    }

    public virtual void resetUnits()
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].hasAttacked = false;
            units[i].hasMoved = false;
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


