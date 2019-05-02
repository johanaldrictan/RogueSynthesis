using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnController : MonoBehaviour
{
    // storage of unit controllers
    public List<UnitController> controllers;

    public int currentTurn;

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

        UnitController.endTurnEvent.AddListener(nextTurn);
        UnitController.queueUpEvent.AddListener(enqueueController);
    }


    // takes the UnitController Parameter and adds it to the turn system in the correct order
    protected void enqueueController(UnitController controller)
    {
        // if empty storage
        if (controllers.Count == 0)
        {
            controllers.Add(controller);
            startController(0);
            return;
        }
        else
        {
            // find the right place to insert
            for (int i = 0; i < controllers.Count; i++)
            {
                if (controllers[i].getWeight() > controller.getWeight())
                {
                    controllers.Insert(i, controller);

                    // if this is the new "first up"
                    if (i == 0)
                    {
                        startController(i);
                        endController(i + 1);
                    }
                    else
                    {
                        endController(i);
                    }
                    
                    return;
                }
            }

            // the controller at this point is in "last place"
            controller.endTurn();
            controllers.Add(controller);
        }
    }

    protected void startController(int index)
    {
        controllers[index].startTurn();
    }

    protected void endController(int index)
    {
        controllers[index].endTurn();
    }

    protected void nextTurn()
    {
        // Debug.Log("Next Turn...");
        endController(currentTurn);

        if (currentTurn != (controllers.Count - 1))
        { currentTurn++; }
        else
        { currentTurn = 0; }

        // Debug.Log("Starting: Turn " + currentTurn);
        startController(currentTurn);
    }
}
