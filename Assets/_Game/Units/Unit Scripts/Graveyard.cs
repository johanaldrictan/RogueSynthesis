﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The Graveyard is an object that stores and manages dead Units
// It will also convert Units from one type to another (such as AlliedUnit --> EnemyUnit)
// Whenever a Unit dies, this object will snag a reference to it before it does

public class Graveyard : MonoBehaviour
{
    // the storage of dead Units
    [System.NonSerialized] private List<Unit> deadUnits;

    public static Graveyard instance;

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

        deadUnits = new List<Unit>();
    }

    private void OnEnable()
    {
        // When a Unit calls out while it is dying, the Graveyard shall answer, taking in the Unit
        Unit.deathEvent.AddListener(EnqueueUnit);
    }

    private void OnDisable()
    {
        Unit.deathEvent.RemoveListener(EnqueueUnit);
    }

    private void Wipe()
    {
        deadUnits.Clear();
    }
    
    // takes a Unit and adds it to the correct section of the storage
    private void EnqueueUnit(Unit target)
    {
        deadUnits.Add(target);
    }

    // this method iterates through the entire storage of dead Units
    // it converts all of the Units that meet the conditions of the parameter function into EnemyUnits
    // it also removes the original Units from the graveyard and returns the final converted EnemyUnits
    public List<EnemyUnit> ConvertToEnemies(ConversionCondition comparator)
    {
        List<EnemyUnit> result = new List<EnemyUnit>();

        // iterate backwards through the units, because we may remove them as we go
        for (int i = deadUnits.Count - 1; i > 0; i--)
        {
            // if the given conditions are met with this particular unit
            if (comparator(deadUnits[i]))
            {
                // Get references
                var oldComponent = deadUnits[i];
                GameObject unitToConvert = deadUnits[i].gameObject;

                // convert the GameObject: add an EnemyUnit component, transfer data over, and then delete the old component
                EnemyUnit newEnemyComponent = unitToConvert.AddComponent<EnemyUnit>() as EnemyUnit;
                UnitConversions.UnitToEnemy(oldComponent, newEnemyComponent);
                deadUnits.RemoveAt(i);
                Destroy(oldComponent);

                // put it away to return at the end
                result.Add(newEnemyComponent);
            }
        }

        return result;
    }
}
