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

    // this can be overridden in subclasses.
    // Things done here should probably be done there a well.
    public virtual void Awake()
    {
        myTurn = true;
        activeUnit = 0;
        units = new List<Unit>();
    }

    // this can be overridden ( like Awake() )
    // remember to invoke the queueUpEvent and call loadUnits() if so
    public virtual void Start()
    {
        // I would like to be added to the TurnController
        queueUpEvent.Invoke(this);

        // Initialize my unitSpawnData into real units
        LoadUnits();
    }

    public virtual void Update()
    {

    }

    // Parses unitSpawnData, instantiates and initializes Units
    // adds the newly instantiated Units to units list
    public void LoadUnits()
    {
        for (int i = 0; i < unitSpawnData.Count; i++)
        {
            // parse the spawn location and spawn a new object there
            Vector3 playerPos = MapMath.MapToWorld(unitSpawnData[i].spawnPosition.x, unitSpawnData[i].spawnPosition.y);
            GameObject shell = new GameObject();
            GameObject newUnit = Instantiate(shell, playerPos, Quaternion.identity);
            Destroy(shell);

            newUnit.AddComponent<SpriteRenderer>();

            // add the correct inherited member of Unit to the object
            Unit newUnitComponent = null;
            if (unitSpawnData[i].data.unitType == UnitType.AlliedUnit)
            { newUnitComponent = newUnit.AddComponent<AlliedUnit>() as AlliedUnit; }
            else if (unitSpawnData[i].data.unitType == UnitType.EnemyUnit)
            { /* newUnitComponent = newUnit.AddComponent<EnemyUnit>() as EnemyUnit; */ }
            else if (unitSpawnData[i].data.unitType == UnitType.Civilian)
            { /* newUnitComponent = newUnit.AddComponent<Civilian>() as Civilian; */ }

            // give this new Unit the raw data for creating it, set its direction
            newUnitComponent.unitData = unitSpawnData[i].data;
            newUnitComponent.direction = unitSpawnData[i].spawnDirection;

            // i've given you the data you need to make yourself. now make yourself, please
            newUnitComponent.loadData();

            // add the brand-spankin-new and created unit to your units list
            units.Add(newUnit.GetComponent<Unit>());
        }
    }

    // determine whether this controller is ready to end its turn
    public virtual bool IsTurnOver()
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

    public abstract void endTurn();
    

    public virtual void ResetUnits()
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].hasAttacked = false;
            units[i].hasMoved = false;
        }
    }

    public void setActiveUnit(int newIndex)
    { activeUnit = newIndex; }

    public bool IsMyTurn()
    { return myTurn; }

    public void StartTurn()
    { myTurn = true; }

    public void EndTurn()
    { myTurn = false; }

    public int GetWeight()
    { return TURN_WEIGHT; }

    public string GetName()
    { return name; }

}


