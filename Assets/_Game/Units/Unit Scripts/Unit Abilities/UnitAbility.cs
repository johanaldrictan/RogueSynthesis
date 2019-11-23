using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// UnitAbility is an abstract class that represents an action a Unit can take
// If you want to make a new Ability, read this and AbilityDatabase.cs

public abstract class UnitAbility
{
    // simply returns the in-game name of the Ability
    public abstract string GetName();

    // The List of all of the different types of effects that this Ability does 
    public abstract List<EffectState> GetEffectState();

    // every Ability must have a range, regardless of whether or not it is used
    public abstract int GetRange();

    // execute() is called in order to do the action's job.
    public abstract void Execute(Unit source, Direction direction);

    // takes a UnitAbility object, returns True when the parameter shouldn't exist with this particular Ability at the same time
    // Examples: the same Ability, inferior versions of this Ability (Gun IV > Gun III)
    protected abstract bool InferiorComparator(UnitAbility inQuestion);

    // NewDelayedEffectEvent is a UnityEvent that fires whenever a DelayedEffect is created and needs to be dealt with
    // The TurnController will respond to this event by adding the passed DelayedEffect object to its list of effects to deal with
    public static DelayedEffectUnityEvent NewDelayedEffectEvent = new DelayedEffectUnityEvent();


    // Takes a List of UnitAbility and removes from it any UnitAbility that can't exist at the same time as this one
    // Examples: the same Ability, inferior versions of this Ability (Gun IV > Gun III)
    public void RemoveInferiors(List<UnitAbility> toFilter)
    { toFilter.RemoveAll(InferiorComparator); }
    
    // Takes a List of UnitAbility and checks to see if this Ability is inferior to any of those in the list
    public bool AmIInferior(List<UnitAbility> contenders)
    {
        foreach (UnitAbility ability in contenders)
        {
            if (ability.InferiorComparator(this))
            {
                return true;
            }
        }
        return false;
    }
}

// UnitAbilities deal varying types of effects. These effects are categorized to assist logic
public enum EffectState
{
    DAMAGE,
    BUFF_DR,
    BUFF_DMG,
    IMMOBILIZE,
    KNOCKBACK,
    DISABLE
}