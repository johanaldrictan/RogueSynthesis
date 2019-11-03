using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DelayedEffect is a type of container that stores an effect from an ability.
/// These objects are passed to a TurnController and trigger their effect when they are supposed to be triggered
/// </summary>

public class DelayedEffect
{
    // reference to the global positional data of all Units
    private UnitPositionStorage globalPositionalData;

    // the actual effect that will occur
    // (Effects are delegates, AKA a function that takes a Unit and returns void)
    private Effect effect;

    // timer is an integer representing how many turns are left until this effect should activate
    // calling Tick() will reduce this number by 1.
    // An effect should activate when its timer is below 0
    private int timer;

    // atEnd refers to whether this Effect should activate at the start of the turn or at the end of the turn
    // for example, specifying triggerType as a Playercontroller and atEnd to false makes this Effect trigger at the start of the Player Phase.
    private bool atEnd;

    // triggerType refers to the type of controller that should cause this object's timer to Tick.
    // for example, specifying triggerType as a Playercontroller and atEnd to false makes this Effect trigger at the start of the Player Phase.
    private UnitType triggerType;

    // if this effect targets a specified area, it will be stored here.
    // otherwise this will be null
    private List<Vector2Int> areaOfEffect;

    // if this effect targets specific Units, they will be stored here.
    // otherwise this will be null
    private List<Unit> effectTargets;

    //This is needed for effects that affect target health. Change health requires a source unit.
    private Unit sourceUnit;

    // this constructor will create a version of this object where it will target an area once the effect activates
    public DelayedEffect(Effect eff, UnitPositionStorage positions, int time, UnitType trigger, bool end, List<Vector2Int> area, Unit source)
    {
        effect = eff;
        globalPositionalData = positions;
        timer = time;
        triggerType = trigger;
        atEnd = end;
        areaOfEffect = area;
        effectTargets = null;
        sourceUnit = source;
    }

    // this constructor will create a version of this object where it will target specific Units once the effect activates
    public DelayedEffect(Effect eff, UnitPositionStorage positions, int time, UnitType trigger, bool end, List<Unit> targets, Unit source)
    {
        effect = eff;
        globalPositionalData = positions;
        timer = time;
        triggerType = trigger;
        atEnd = end;
        areaOfEffect = null;
        effectTargets = targets;
        sourceUnit = source;
    }

    // this function triggers the Effect.
    // it finds all applicable units and deals the proper effects to each one.
    public void Trigger()
    {
        // this Effect affects an AREA
        if (areaOfEffect != null)
        {
            foreach(Vector2Int coordinate in areaOfEffect)
            {
                Unit searchResult = globalPositionalData.SearchLocation(coordinate);
                if (searchResult != null)
                {
                    effect(searchResult, sourceUnit);
                }
            }
        }

        // this Effect affects SPECIFIC UNITS
       if (effectTargets != null)
        {
            foreach(Unit target in effectTargets)
            {
                effect(target, sourceUnit);
            }
        }
    }

    // this function will decrease the object's timer by 1.
    // it returns the value of the timer after the Tick occurs
    public int Tick()
    {
        timer--;
        return timer;
    }

    public int GetTimer()
    {
        return timer;
    }
    public UnitType GetTriggerType()
    {
        return triggerType;
    }

    public bool AtEnd()
    {
        return atEnd;
    }

}
