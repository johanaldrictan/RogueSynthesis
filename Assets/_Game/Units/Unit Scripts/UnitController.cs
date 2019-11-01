using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// UnitController is an Abstract Base Class for an object that stores and commands Unit objects

public abstract class UnitController : MonoBehaviour
{

    protected bool myTurn;

    // the weight used by the turn system to determine order of the controllers
    // lower is faster
    [System.NonSerialized] protected const int TURN_WEIGHT = 0;

    // the index of the currently selected Unit in the List
    [SerializeField] protected int activeUnit;

    // storage of real units
    [System.NonSerialized] public List<Unit> units;

    // Serialized (Editable in Unity's Inspector) List of data used for initially spawning units
    [SerializeField] protected List<UnitDataContainer> unitSpawnData;

    // reference to the TurnController's positional data storage
    [System.NonSerialized] public UnitPositionStorage globalPositionalData;

    // Event for queueing up to be added to a TurnController
    public static UnitControllerUnityEvent QueueUpEvent = new UnitControllerUnityEvent();

    // Event for asking a TurnController to end this controller's turn
    public static UnitControllerUnityEvent EndTurnEvent = new UnitControllerUnityEvent();

    // this can be overridden in subclasses.
    // Things done here should probably be done there a well.
    protected virtual void Awake()
    {
        myTurn = true;
        activeUnit = 0;
        units = new List<Unit>();
    }

    private void OnEnable()
    {
        Unit.DeathEvent.AddListener(RemoveUnit);
    }

    private void OnDisable()
    {
        Unit.DeathEvent.RemoveListener(RemoveUnit);
    }

    // this can be overridden ( like Awake() )
    // remember to invoke the queueUpEvent and call loadUnits() if so
    public virtual void Start()
    {
        // I would like to be added to the TurnController
        QueueUpEvent.Invoke(this);

        // Initialize my unitSpawnData into real units
        LoadUnits();

    }

    public virtual void Update()
    {
        // Debug.Log(units.Count);
    }

    // Parses unitSpawnData, instantiates and initializes Units
    // adds the newly instantiated Units to units list
    protected void LoadUnits()
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
            switch (unitSpawnData[i].data.unitType)
            {
                case UnitType.AlliedUnit:
                    newUnitComponent = newUnit.AddComponent<AlliedUnit>() as AlliedUnit;
                    break;

                case UnitType.EnemyUnit:
                    newUnitComponent = newUnit.AddComponent<EnemyUnit>() as EnemyUnit;
                    break;

                case UnitType.Civilian:
                    break;

                default:
                    newUnitComponent = newUnit.AddComponent<AlliedUnit>() as AlliedUnit;
                    break;
            }

            // give this new Unit the raw data for creating it, set its direction
            newUnitComponent.StartData = unitSpawnData[i].data;
            newUnitComponent.SetDirection(Direction.S);
            newUnitComponent.globalPositionalData = this.globalPositionalData;
            newUnitComponent.globalPositionalData.AddUnit(unitSpawnData[i].spawnPosition, newUnitComponent);

            // i've given you the data you need to make yourself. now make yourself, please
            newUnitComponent.LoadData();

            // add the brand-spankin-new and created unit to your units list
            units.Add(newUnit.GetComponent<Unit>());
        }
    }

    // removeUnit takes a Unit object and removes it from the units List, if it finds an that Unit in the list
    // it also makes sure that the activeUnit index doesn't get messed up
    protected void RemoveUnit(Unit wanted)
    {
        // Search for the wanted unit, and get the index of where it is
        int index = units.FindIndex(x => x == wanted);

        // stop if you didn't find the unit
        if (index == -1)
        { return; }

        units.Remove(wanted);

        // make sure the activeUnit index is correct, since the list just shifted
        if (index < activeUnit)
        { activeUnit--; }
    }

    // takes a list of Units and adds it to the controller's storage of Units
    protected void AddUnits(List<Unit> inputList)
    {
        foreach (Unit unit in inputList)
        {
            units.Add(unit);
            if (unit.GetHealth() == 0)
            {
                unit.Revive(unit.Deaths.Peek().GetDeathLocation());
            }
        }
    }

    // determine whether this controller is ready to end its turn
    public virtual bool IsTurnOver()
    {
        for (int i = 0; i < units.Count; i++)
        {
            // if a unit that hasn't moved or attacked exists
            if (!units[i].hasActed || !units[i].hasMoved)
            {
                // Debug.Log("Turn not over");
                return false;
            }
        }
        // Debug.Log("Turn over");
        return true;
    }

    // the function for a UnitController to allow a TurnController to switch to the next phase
    public abstract void RelinquishPower();

    public virtual void ResetUnits()
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].isImmobilized)
            {
                units[i].isImmobilized = false;
                continue;
            }
            else
            {
                units[i].hasActed = false;
                units[i].hasMoved = false;
            }
        }
    }

    // find the next Index in the list of Units that is available
    public int GetNextIndex()
    {
        int newActiveUnit = activeUnit;

        if (units.Count != 0)
        {
            // go through the list, skipping units that are inactive in the heirarchy, or have both moved and attacked already
            do
            {
                newActiveUnit = (newActiveUnit + 1) % units.Count;
            }
            while (!units[newActiveUnit].gameObject.activeInHierarchy || (units[newActiveUnit].hasActed && units[newActiveUnit].hasMoved));
        }

        return newActiveUnit;
    }

    public int getActiveUnit()
    { return activeUnit; }

    public void setActiveUnit(int newIndex)
    { activeUnit = newIndex; }

    public bool IsMyTurn()
    { return myTurn; }

    public void StartTurn()
    { myTurn = true; }

    public void EndTurn()
    { myTurn = false; }

    public virtual int GetWeight()
    { return TURN_WEIGHT; }

    public string GetName()
    { return name; }

}


