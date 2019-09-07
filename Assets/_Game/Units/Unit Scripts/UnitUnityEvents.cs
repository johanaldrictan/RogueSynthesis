using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// This is a place to define all UnityEvent Objects and Delegates for general use by all Scripts.
// Specifically for scripts associated with Untis

// UNITYEVENT

// Creates an event that passes a UnitController with it
public class UnitControllerUnityEvent : UnityEvent<UnitController> { }

// Creates an event that passes a Unit with it
public class UnitUnityEvent : UnityEvent<Unit> { }

// Creates an event that passes a List of Units with it
public class ListofUnitUnityEvent : UnityEvent<List<Unit>> { }



// DELEGATE

public delegate bool ConversionCondition(Unit inQuestion);