﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Wait is a UnitAbility that does nothing, setting the Unit to hasAttacked = True

// If you want to create a new UnitAbility, refer to the comments/code on UnitAbility.cs and AbilityDatabase.cs

public class Wait : UnitAbility
{
    public override void Execute(Unit source)
    {
        source.hasAttacked = true;
        return;
    }

    public override int GetRange()
    {
        return 0;
    }

    public override bool InferiorComparator(UnitAbility inQuestion)
    {
        return ( inQuestion.GetType() == typeof(Wait) );
    }
}
