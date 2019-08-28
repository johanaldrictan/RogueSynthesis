﻿using System.Collections;
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

    // What Directino was I facing when I died?
    private Direction deathDirection;

    // Constructor. Takes references to the data it needs to store
    public DeathData(Unit source, UnitAbility attack, int damage, Vector2Int location, Direction direction)
    {
        killer = source;
        finishingAbility = attack;
        damageTaken = damage;
        deathLocation = location;
        deathDirection = direction;
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

    public Direction GetDeathDirection()
    {
        return deathDirection;
    }
}