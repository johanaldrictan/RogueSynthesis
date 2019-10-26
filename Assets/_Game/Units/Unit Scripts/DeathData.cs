using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is used by Units to store and reference information about how they died

public class DeathData
{
    // Who killed me?
    private Unit killer;

    // What move killed me?
    private UnitAbility finishingAbility;

    // How much damage did the finishing move do to me?
    private int damageTaken;

    // Where was I when I died?
    private Vector2Int deathLocation;

    // Constructor. Takes references to the data it needs to store
    public DeathData(Unit source, UnitAbility attack, int damage, Vector2Int location)
    {
        killer = source;
        finishingAbility = attack;
        damageTaken = damage;
        deathLocation = location;
    }

    public Unit GetKiller()
    {
        return killer;
    }

    public UnitAbility GetFinishingAbility()
    {
        return finishingAbility;
    }

    public int GetDamageTaken()
    {
        return damageTaken;
    }

    public Vector2Int GetDeathLocation()
    {
        return deathLocation;
    }

    //  prints the contents of this object to the Debug Console
    public void DebugLog()
    {
        Debug.Log("Killer: " + killer + "; Finishing Ability: " + finishingAbility + "; Damage Taken: " + damageTaken + "; Location: " + deathLocation);
    }
}
