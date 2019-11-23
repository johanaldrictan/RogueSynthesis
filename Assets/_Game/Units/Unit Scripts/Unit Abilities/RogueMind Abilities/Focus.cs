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

    public override List<EffectState> GetEffectState()
    {
        return new List<EffectState>() { EffectState.BUFF_DR };
    }

    public override string GetName()
    {
        return "Focus";
    }

    public override int GetRange()
    {
        return 0;
    }

    public override string GetSoundEvent()
    {
        return "event:/WAT/WAT_Focus";
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(Focus));
    }
}
