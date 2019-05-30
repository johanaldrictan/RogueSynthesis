using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// UnitAbility is an abstract class that represents an action a Unit can take
// it contains a range for the ability and a function that executes the action, ending its turn afterwards
public abstract class UnitAbility : MonoBehaviour
{
    // every Ability must have a range, regardless of whether or not it is used
    public abstract int getRange();

    // execute() does the action's job, and sets the unit's hasActed to true
    public abstract void execute(Unit source);
}

