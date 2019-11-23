using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A TurnController is a class that is able to manage multiple UnitControllers.
// It responds to events invoked by UnitControllers;
// no linking is required other than the presence of both this Class and 1+ UnitController Classes (or SubClasses)

// TurnController also handles delayed effects from Abilities. 
// They receive those effects and trigger them when they are supposed to be triggered.

public class TurnController : MonoBehaviour
{
    // storage of unit controllers
    private List<UnitController> controllers;

    // storage of Unit Positional Data
    private UnitPositionStorage globalPositionalData;

    // storage of Trap positional Data
    public TrapPositionStorage trapPositionalData;

    // the index of the controllers List that is currently the active controller
    private int currentTurn;

    // the number of full rounds that have passed
    private int currentRound;

    // the storage of delayed Ability effects that have yet to occur
    // whenever this List is evaluated, the last item in the List is first to be evaluated.
    // adding an item to the List shoves it to the front of the List
    private List<DelayedEffect> delayedEffects;

    // A UnityEvent that is called whenver a turn ends. it asks for any Units that should be turned into enemies to happen
    public static ConversionConditionUnityEvent ToEnemyEvent = new ConversionConditionUnityEvent();

    public static TurnController instance;
    public List<Attack> delayedAttacks;


    // initialization
    private void Awake()
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

        currentTurn = 0;
        currentRound = 1;
        controllers = new List<UnitController>();
        globalPositionalData = new UnitPositionStorage();
        trapPositionalData = new TrapPositionStorage(globalPositionalData);
        delayedEffects = new List<DelayedEffect>();
    }

    private void OnEnable()
    {
        // register for events called by UnitControllers
        UnitController.EndTurnEvent.AddListener(NextTurn);
        UnitController.QueueUpEvent.AddListener(EnqueueController);
        UnitAbility.NewDelayedEffectEvent.AddListener(EnqueueDelayedEffect);
    }

    private void OnDisable()
    {
        // un-register for events called by UnitControllers
        UnitController.EndTurnEvent.RemoveListener(NextTurn);
        UnitController.QueueUpEvent.RemoveListener(EnqueueController);
        UnitAbility.NewDelayedEffectEvent.RemoveListener(EnqueueDelayedEffect);
    }

    // takes a UnitController and adds it to the turn system in the correct order
    protected void EnqueueController(UnitController controller)
    {
        controller.globalPositionalData = this.globalPositionalData;

        // if empty storage
        if (controllers.Count == 0)
        {
            controllers.Add(controller);
            StartController(0);
            return;
        }
        else
        {
            // find the right place to insert
            for (int i = 0; i < controllers.Count; i++)
            {
                if (controllers[i].GetWeight() > controller.GetWeight())
                {
                    controllers.Insert(i, controller);

                    // if this is the new "first place"
                    if (i == 0)
                    {
                        StartController(i);
                        EndController(i + 1);
                    }
                    else
                    {
                        EndController(i);
                    }
                    
                    return;
                }
            }

            // the controller at this point is in "last place"
            controller.EndTurn();
            controllers.Add(controller);
        }
    }

    // EnqueueDelayedEffect takes a DelayedEffect object and adds it to its storage
    // This function adds the object to the front, because when evaluating the List it starts from the back
    protected void EnqueueDelayedEffect(DelayedEffect effect)
    {
        delayedEffects.Insert(0, effect);
    }

    protected void NextTurn(UnitController controller)
    {
        // make sure that the controller asking for the next turn currently has control
        if (controllers[currentTurn] == controller && controller.IsMyTurn())
        {
            CycleEffects(false);

            if (controllers[currentTurn] is PlayerController && controllers[currentTurn].units.Count > 0)
            { (controllers[currentTurn] as PlayerController).ClearSpotlight(); }

            EndController(currentTurn);
            currentTurn = ((currentTurn + 1) % controllers.Count);
            if (currentTurn == 0)
            { 
                currentRound += 1;
                Debug.Log("Round: " + currentRound);
            }

            // if there's any Allied Units that died to enemy units, convert them to enemies
            // IF:                     I'm an AlliedUnit  AND  The person who most recently killed me is an EnemyUnit
            ToEnemyEvent.Invoke(unit => unit is AlliedUnit && unit.Deaths.Peek().GetKiller() is EnemyUnit);

            StartController(currentTurn);
            // wrap around the unit List to select the first valid unit
            if (controllers[currentTurn].units.Count > 0)
            {
                controllers[currentTurn].setActiveUnit(controllers[currentTurn].units.Count - 1);
                controllers[currentTurn].GetNextUnit();
            }

            CycleEffects(true);

            if (controllers[currentTurn] is PlayerController && controllers[currentTurn].units.Count > 0)
            { (controllers[currentTurn] as PlayerController).SpotlightActiveUnit(); }
        }
    }

    // this function evaluates the storage of DelayedEffects
    // each one gets Ticked if it's the correct time to do so
    // if an Effect gets Ticked below 0, it triggers and is then removed from the storage
    // the parameter, atEnd, refers to whether this function is being called at the start or at the end of the turn
    protected void CycleEffects(bool atEnd)
    {
        for (int i = delayedEffects.Count - 1; i >= 0; i--)
        {
            // if it is currently the correct turn and section for this particular Effect, Tick it.
            switch(delayedEffects[i].GetTriggerType())
            {
                case UnitType.AlliedUnit:
                    if (controllers[currentTurn] is PlayerController && atEnd == delayedEffects[i].AtEnd())
                    { delayedEffects[i].Tick(); }
                    break;

                case UnitType.EnemyUnit:
                    if (controllers[currentTurn] is EnemyController && atEnd == delayedEffects[i].AtEnd())
                    { delayedEffects[i].Tick(); }
                    break;

                default:
                    break;
            }

            // when an Effect is Ticked below 0, Trigger and remove it
            if (delayedEffects[i].GetTimer() < 0)
            {
                delayedEffects[i].Trigger();
                delayedEffects.RemoveAt(i);
            }
        }
    }

    // this creates a GameObject with a corresponding Trap on it, and stores it inside of the trapPositionalData
    public void SetTrap(Vector2Int position, TrapType trap, Unit source, UnitAbility ability)
    {
        // parse the spawn location and spawn a new object there
        Vector3 pos = MapMath.MapToWorld(position.x, position.y);
        GameObject shell = new GameObject();
        GameObject newTrap = Instantiate(shell, pos, Quaternion.identity);
        Destroy(shell);

        // add the correct inherited member of Trap to the object
        Trap newTrapComponent = null;
        switch(trap)
        {
            case TrapType.Claymore:
                newTrapComponent = newTrap.AddComponent<ClaymoreTrap>() as ClaymoreTrap;
                break;

            default:
                break;
        }

        newTrapComponent.mapPosition = position;
        newTrapComponent.sourceUnit = source;
        newTrapComponent.placingAbility = ability;
        newTrap.AddComponent<SpriteRenderer>();
        Sprite newSprite = Resources.Load<Sprite>(newTrapComponent.GetResourcePath());
        newTrap.GetComponent<SpriteRenderer>().sprite = newSprite;
        trapPositionalData.AddTrap(position, newTrapComponent);
    }


    protected void StartController(int index)
    { controllers[index].StartTurn(); }

    protected void EndController(int index)
    { controllers[index].EndTurn(); }

    // gets the current Round (full-round) number
    public int GetRound()
    { return currentRound; }
    
}
