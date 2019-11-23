using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Traps are objects that are placed on the map that can be 'triggered'
/// Traps have a trigger condition and an effect that activates when the trigger condition is met
/// a Trap's trigger condition and effect can vary, hence why this is an abstract class
/// </summary>
public abstract class Trap : MonoBehaviour
{
    public Vector2Int mapPosition;
    public Unit sourceUnit;
    public UnitAbility placingAbility;

    public abstract string GetResourcePath();

    // takes a Unit target as a parameter and deals the appropriate effect to that Unit
    public abstract void Effect(Unit target);

    // Returns true when the inQuestion Unit has fulfilled the conditions to trigger this object
    public abstract bool TriggerCondition(Unit inQuestion);

}

// list of all the different Types of Traps
public enum TrapType
{
    Claymore
};