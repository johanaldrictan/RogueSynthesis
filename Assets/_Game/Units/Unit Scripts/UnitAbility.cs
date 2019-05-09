using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// THESE DO NOT CURRENTLY WORK
[CreateAssetMenu]
public class UnitAbility : ScriptableObject
{
    private List<UnitAbilityListener> listeners = new List<UnitAbilityListener>();

    public void Raise(Unit source, Unit target)
    {
        for (int i = listeners.Count-1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(source, target);
        }
    }

    public void RegisterListener(UnitAbilityListener listener)
    { listeners.Add(listener); }
    public void UnRegisterListener(UnitAbilityListener listener)
    { listeners.Remove(listener); }
}
