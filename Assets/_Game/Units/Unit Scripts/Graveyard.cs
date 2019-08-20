using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The Graveyard is an object that stores and manages dead Units
// It will also eventually be able to convert Units from one type to another (such as AlliedUnit --> EnemyUnit)
// Whenever a Unit dies, this object will snag a reference to it before it does

public class Graveyard : MonoBehaviour
{
    // the storages of dead enemy units. 
    [System.NonSerialized] private List<Unit> enemies;
    [System.NonSerialized] private List<Unit> allies;
    [System.NonSerialized] private List<Unit> civilians;

    private void Awake()
    {
        enemies = new List<Unit>();
        allies = new List<Unit>();
        civilians = new List<Unit>();
    }

    private void OnEnable()
    {
        Unit.deathEvent.AddListener(EnqueueUnit);
    }

    private void OnDisable()
    {
        Unit.deathEvent.RemoveListener(EnqueueUnit);
    }

    public void Wipe()
    {
        enemies.Clear();
        allies.Clear();
        civilians.Clear();
    }
    
    // takes a Unit and adds it to the correct section of storage
    private void EnqueueUnit(Unit target)
    {
        switch (target.unitData.unitType)
        {
            case UnitType.AlliedUnit:
                allies.Add(target as AlliedUnit);
                break;

            case UnitType.EnemyUnit:
                // enemies.Add(target as EnemyUnit);
                break;

            case UnitType.Civilian:
                // civilians.Add(target as Civilian);
                break;

            default:
                break;
        }
    }
}
