using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TrapPositionStorage is just an embellished public Dictionary
// Its keys are (x, y) coordinates on the map
// Its values are Trap objects that are occupying those spaces


public class TrapPositionStorage
{
    // reference to UnitPositionStorage
    private UnitPositionStorage globalPositionalData;

    // the storage of traps. 
    private Dictionary<Vector2Int, Trap> storage;

    // constructor. simply creates the storage Dictionary
    public TrapPositionStorage(UnitPositionStorage units)
    {
        storage = new Dictionary<Vector2Int, Trap>();
        globalPositionalData = units;
    }

    // wipes the storage
    public void Wipe()
    {
        storage.Clear();
    }

    // checks every trap and every unit on the map
    // if any units have triggered a trap, spring and remove it
    public void CheckTraps()
    {
        foreach(Trap trap in GetTraps())
        {
            bool triggered = false;
            foreach (Unit unit in globalPositionalData.GetUnits())
            {
                if (trap.TriggerCondition(unit))
                {
                    trap.Effect(unit);
                    triggered = true;
                }
            }
            if (triggered)
                RemoveTrap(trap.mapPosition);
        }
    }

    // adds a given unit, and its positional data, into storage
    public void AddTrap(Vector2Int location, Trap trap)
    {
        if (storage.ContainsKey(location))
        {
            //Don't let them place another mine in the same location
            return;
        }
        else
        {
            storage.Add(location, trap);
            return;
        }
    }

    // removes the given key from storage, if anything exists at that key
    // returns true if something was actually erased
    public bool RemoveTrap(Vector2Int location)
    {
        if (storage.ContainsKey(location))
        {
            storage.Remove(location);
            return true;
        }
        else
        {
            return false;
        }
    }

    // takes a coordinate as an argument
    // if a Trap exists at that coordinate, returns that unit
    // otherwise returns null
    public Trap SearchLocation(Vector2Int location)
    {
        if (storage.ContainsKey(location))
        {
            return storage[location];
        }
        else
        {
            return null;
        }
    }

    public List<Vector2Int> GetLocations()
    {
        return new List<Vector2Int>(storage.Keys);
    }

    public List<Trap> GetTraps()
    {
        return new List<Trap>(storage.Values);
    }

    // Debugging function. Prints the contents of storage
    public void DebugPrint()
    {
        // iterate through each key-value pair in storage
        foreach (KeyValuePair <Vector2Int, Trap> entry in storage)
        {
            Debug.Log("TrapPositionStorage: " + entry.Key + " --> " + entry.Value);
        }
    }
}
