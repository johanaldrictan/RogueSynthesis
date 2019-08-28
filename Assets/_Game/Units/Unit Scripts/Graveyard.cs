using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The Graveyard is an object that stores and manages dead Units
// It will also eventually be able to convert Units from one type to another (such as AlliedUnit --> EnemyUnit)
// Whenever a Unit dies, this object will snag a reference to it before it does

public class Graveyard : MonoBehaviour
{
    // the storage of dead EnemyUnit Objects
    [System.NonSerialized] private List<Unit> enemies;

    // the storage of dead AlliedUnit Objects. These will be converted into enemy units
    [System.NonSerialized] private List<Unit> allies;

    // the storage of dead Civilian Objects. 
    [System.NonSerialized] private List<Unit> civilians;

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

        enemies = new List<Unit>();
        allies = new List<Unit>();
        civilians = new List<Unit>();
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
        enemies.Clear();
        allies.Clear();
        civilians.Clear();
    }
    
    // takes a Unit and adds it to the correct section of the storage
    private void EnqueueUnit(Unit target)
    {
        switch (target.unitData.unitType)
        {
            case UnitType.AlliedUnit:
                allies.Add(target as AlliedUnit);
                break;

            case UnitType.EnemyUnit:
                enemies.Add(target as EnemyUnit);
                break;

            case UnitType.Civilian:
                // civilians.Add(target as Civilian); <-- Civilians don't exist yet lmao
                break;

            default:
                break;
        }
    }
}
