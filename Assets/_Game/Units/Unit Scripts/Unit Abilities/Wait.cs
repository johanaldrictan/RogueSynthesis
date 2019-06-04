using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Wait is a UnitAbility that does nothing, setting the Unit to hasAttacked = True

// If you want to create a new UnitAbility, refer to the comments/code on UnitAbility.cs and AbilityDatabase.cs

public class Wait : UnitAbility
{
    public override void execute(Unit source)
    {
        source.hasAttacked = true;
        return;
    }

    public override int getRange()
    {
        return 0;
    }

    public override bool inferiorComparator(UnitAbility inQuestion)
    {
        return ( inQuestion.GetType() == typeof(Wait) );
    }
}
