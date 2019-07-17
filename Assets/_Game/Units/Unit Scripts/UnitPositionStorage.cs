using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// UnitPositionStorage is just an embellished public Dictionary
// Its keys are (x, y) coordinates on the map
// Its values are Units that are occupying those spaces


public class UnitPositionStorage
{
    // the storage of units. 
    private Dictionary<Tuple<int, int>, Unit> storage;

    // constructor. simply creates the storage Dictionary
    public UnitPositionStorage()
    {
        storage = new Dictionary<Tuple<int, int>, Unit>();
    }

    // wipes the storage
    public void Wipe()
    {
        storage.Clear();
    }

    // adds a given unit, and its positional data, into storage
    public void StoreUnit(int x, int y, Unit theUnit)
    {
        Tuple<int, int> convertedTuple = new Tuple<int, int>(x, y);

        if (storage.ContainsKey(convertedTuple))
        {
            Debug.Log("UnitPositionSorage: This isn't supposed to fire. Something is trying to be stored in a location that is already occupied.");
            return;
        }
        else
        {
            storage.Add(convertedTuple, theUnit);
            return;
        }
    }

    // removes the given key from storage, if anything exists at that key
    // returns true if something was actually erased
    public bool RemoveUnit(int x, int y)
    {
        Tuple<int, int> convertedTuple = new Tuple<int, int>(x, y);

        if (storage.ContainsKey(convertedTuple))
        {
            storage.Remove(convertedTuple);
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
    public Unit SearchLocation(int x, int y)
    {
        Tuple<int, int> convertedTuple = new Tuple<int, int>(x, y);

        if (storage.ContainsKey(convertedTuple))
        {
            return storage[convertedTuple];
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
        foreach (KeyValuePair <Tuple<int, int>, Unit> entry in storage)
        {
            Debug.Log("UnitPositionStorage: " + entry.Key + " --> " + entry.Value);
        }
    }
}
