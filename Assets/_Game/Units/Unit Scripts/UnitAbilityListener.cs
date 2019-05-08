using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// An AbilityUnityEvent is a type of UnityEvent that takes a source Unit and a target Unit.
// It is meant for special activatable effects of units.
[System.Serializable]
public class AbilityUnityEvent : UnityEvent<Unit, Unit> { }

// THESE DO NOT CURRENTLY WORK
public class UnitAbilityListener : MonoBehaviour
{
    public UnitAbility Event;
    public AbilityUnityEvent Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnRegisterListener(this); }

    public void OnEventRaised(Unit source, Unit target)
    { Response.Invoke(source, target); }
}
