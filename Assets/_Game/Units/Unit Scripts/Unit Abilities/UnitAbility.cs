using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// UnitAbility is an abstract class that represents an action a Unit can take

public abstract class UnitAbility
{
    // every Ability must have a range, regardless of whether or not it is used
    public abstract int GetRange();

    // execute() does the action's job, and sets the unit's hasActed to True
    public abstract void Execute(Unit source);

    // takes a UnitAbility object, returns True when the parameter shouldn't exist with this one at the same time
    // Examples: the same Ability, inferior versions of this Ability (Gun IV > Gun III)
    protected abstract bool InferiorComparator(UnitAbility inQuestion);


    // Takes a List of UnitAbility and removes from it any UnitAbility that can't exist at the same time as this one
    // Examples: the same Ability, inferior versions of this Ability (Gun IV > Gun III)
    public void RemoveInferiors(List<UnitAbility> toFilter)
    { toFilter.RemoveAll(InferiorComparator); }
}

