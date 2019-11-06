using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// MinePositionStorage is just an embellished public Dictionary
// Its keys are (x, y) coordinates on the map
// Its values are mines that are occupying those spaces


public class MinePositionStorage
{
    // the storage of units. 
    private Dictionary<Vector2Int, Unit> storage;

    // constructor. simply creates the storage Dictionary
    public MinePositionStorage()
    {
        storage = new Dictionary<Vector2Int, Unit>();
    }

    // wipes the storage
    public void Wipe()
    {
        storage.Clear();
    }

    // adds a given unit, and its positional data, into storage
    public void AddMine(Vector2Int location, Unit theUnit)
    {
        if (storage.ContainsKey(location))
        {
            //Don't let them place another mine in the same location
            return;
        }
        else
        {
            storage.Add(location, theUnit);
            return;
        }
    }

    // removes the given key from storage, if anything exists at that key
    // returns true if something was actually erased
    public bool RemoveUnit(Vector2Int location)
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
    // if a unit exists at that coordinate, returns that unit
    // otherwise returns null
    public Unit SearchLocation(Vector2Int location)
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

    // Debugging function. Prints the contents of storage
    public void DebugPrint()
    {
        // iterate through each key-value pair in storage
        foreach (KeyValuePair <Vector2Int, Unit> entry in storage)
        {
            Debug.Log("UnitPositionStorage: " + entry.Key + " --> " + entry.Value);
        }
    }
}
