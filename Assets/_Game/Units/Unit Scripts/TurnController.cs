using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A TurnController is a class that is able to manage multiple UnitControllers.
// It responds to events invoked by UnitControllers;
// no linking is required other than the presence of both this Class and 1+ UnitController Classes (or SubClasses)

public class TurnController : MonoBehaviour
{
    // storage of unit controllers
    private List<UnitController> controllers;

    // storage of Unit Positional Data
    private UnitPositionStorage globalPositionalData;

    // the index of the controllers List that is currently the active controller
    private int currentTurn;

    // the number of full rounds that have passed
    private int currentRound;

    // A UnityEvent that is called whenver a turn ends. it asks for any Units that should be turned into enemies to happen
    public static ConversionConditionUnityEvent ToEnemyEvent = new ConversionConditionUnityEvent();

    public static TurnController instance;

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
    }

    private void OnEnable()
    {
        // register for events called by UnitControllers
        UnitController.EndTurnEvent.AddListener(NextTurn);
        UnitController.QueueUpEvent.AddListener(EnqueueController);
    }

    private void OnDisable()
    {
        // un-register for events called by UnitControllers
        UnitController.EndTurnEvent.RemoveListener(NextTurn);
        UnitController.QueueUpEvent.RemoveListener(EnqueueController);
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

    protected void NextTurn(UnitController controller)
    {
        // make sure that the controller asking for the next turn currently has control
        if (controllers[currentTurn] == controller && controller.IsMyTurn())
        {
            if (controllers[currentTurn].GetType() == typeof(PlayerController))
            {
                (controllers[currentTurn] as PlayerController).ClearSpotlight();
            }

            EndController(currentTurn);
            currentTurn = ((currentTurn + 1) % controllers.Count);
            if (currentTurn == 0)
            { currentRound += 1; }
            StartController(currentTurn);

            if(controllers[currentTurn] is EnemyController)
            {
                //should be enemy controller's turn
                
            }

            // if there's any Allied Units that died to enemy units, convert them to enemies
            // IF                                      I'm an AlliedUnit    AND       The person who most recently killed me is an EnemyUnit
            ToEnemyEvent.Invoke(unit => unit.GetType()  == typeof(AlliedUnit) && unit.Deaths.Peek().GetKiller().GetType() == typeof(EnemyUnit));

            if (controllers[currentTurn].GetType() == typeof(PlayerController))
            {
                (controllers[currentTurn] as PlayerController).SpotlightActiveUnit();
            }
        }
    }

    protected void StartController(int index)
    { controllers[index].StartTurn(); }

    protected void EndController(int index)
    { controllers[index].EndTurn(); }

    // gets the current Round (full-round) number
    public int GetRound()
    { return currentRound; }
    
}
