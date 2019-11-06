using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// If you want to create a new UnitAbility, refer to the comments/code on UnitAbility.cs and AbilityDatabase.cs

public class Focus : UnitAbility
{

    public override void Execute(Unit source, Direction direction)
    {
        if (source.GetDamageReduction() < 5)
            source.SetDamageReduction(1);
    }

    public override int GetRange()
    {
        return 0;
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(Focus));
    }
}
