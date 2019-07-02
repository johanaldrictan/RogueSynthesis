using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// A TurnController is a class that is able to manage multiple UnitControllers.
// It responds to events invoked by UnitControllers;
// no linking is required other than the presence of both this Class and 1+ UnitController Classes (or SubClasses)

public class TurnController : MonoBehaviour
{
    // storage of unit controllers
    private List<UnitController> controllers;

    private int currentTurn;

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
        controllers = new List<UnitController>();
    }

    private void OnEnable()
    {
        // register for events called by UnitControllers
        UnitController.endTurnEvent.AddListener(NextTurn);
        UnitController.queueUpEvent.AddListener(EnqueueController);
    }

    private void OnDisable()
    {
        // un-register for events called by UnitControllers
        UnitController.endTurnEvent.RemoveListener(NextTurn);
        UnitController.queueUpEvent.RemoveListener(EnqueueController);
    }

    // takes a UnitController and adds it to the turn system in the correct order
    protected void EnqueueController(UnitController controller)
    {
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

    protected void StartController(int index)
    { controllers[index].StartTurn(); }

    protected void EndController(int index)
    { controllers[index].EndTurn(); }

    protected void NextTurn(UnitController controller)
    {
        // make sure that the controller asking for the next turn currently has control
        if (controllers[currentTurn] == controller && controller.IsMyTurn())
        {
            EndController(currentTurn);
            currentTurn = ((currentTurn + 1) % controllers.Count);
            StartController(currentTurn);
        }
    }
}
