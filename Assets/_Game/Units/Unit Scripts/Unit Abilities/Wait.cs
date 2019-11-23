using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Wait is a UnitAbility that does nothing

// If you want to create a new UnitAbility, refer to the comments/code on UnitAbility.cs and AbilityDatabase.cs

public class Wait : UnitAbility
{
    public override void Execute(Unit source, Direction direction)
    {
        return;
    }

    public override List<EffectState> GetEffectState()
    {
        return new List<EffectState>();
    }

    public override string GetName()
    {
        return "Wait";
    }

    public override int GetRange()
    {
        return 0;
    }

    public override string GetSoundEvent()
    {
        return "";
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(Wait));
    }
}
