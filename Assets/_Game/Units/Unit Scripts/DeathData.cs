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

    // Constructor. Takes references to the data it needs to store (Unit that killed, Ability that killed)
    public DeathData(Unit source, UnitAbility attack, int damage)
    {
        killer = source;
        finishingAbility = attack;
        damageTaken = damage;
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
}
