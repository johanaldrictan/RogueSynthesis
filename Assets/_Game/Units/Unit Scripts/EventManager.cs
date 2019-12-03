using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EventManager handles the control heirarchies behind events and control of other scripts
/// "events" refer to an occurence where something is happening, and other things should not while so
/// For example, the event involving a Unit moving to new space should inhibit the ability to move other Units until that event finishes
/// </summary>

public class EventManager : MonoBehaviour
{
    /// <summary>
    /// storage is a Dictionary with key string and value int
    /// the key is any name for the event, as long as similar events use the same name
    /// the value is the number of active 'events' that are stored of that name
    /// </summary>
    [System.NonSerialized] private Dictionary<string, int> storage;

    public static EventManager instance;

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

        storage = new Dictionary<string, int>();
    }

    // wipes all events off of the storage
    public void WipeEvents()
    {
        storage.Clear();
    }

    // takes the name of an event and adds it to the storage
    public void AddEvent(string name)
    {
        if (storage.ContainsKey(name))
        {
            storage[name] += 1;
        }
        else
        {
            storage.Add(name, 1);
        }
    }

    // adds multiple events to the storage
    public void AddMultipleEvents(List<string> names)
    {
        foreach (string name in names)
            AddEvent(name);
    }

    // takes the name of an event and removes it to the storage
    public void RemoveEvent(string name)
    {
        if (storage.ContainsKey(name) && storage[name] > 0)
        {
            storage[name] -= 1;
        }
        else
        {
            Debug.Log("An Event that doesn't exist was attempted to be removed");
        }
    }

    // removes multiple events from the storage
    public void RemoveMultipleEvents(List<string> names)
    {
        foreach (string name in names)
            RemoveEvent(name);
    }

    // returns true if there are currently existing events in the storage
    public bool EventsExist()
    {
        foreach (string key in storage.Keys)
        {
            if (storage[key] > 0)
            {
                return true;
            }
        }
        return false;
    }

    // checks a specific event
    // returns the number of existing instances of the event name
    public int CheckEvent(string name)
    {
        if (storage.ContainsKey(name))
            return storage[name];
        else
            return 0;
    }
}
