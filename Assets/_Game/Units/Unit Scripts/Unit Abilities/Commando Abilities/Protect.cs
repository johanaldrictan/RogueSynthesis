using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protect : UnitAbility
{
    public override void Execute(Unit source, Direction direction)
    {
        
    }

    public override List<EffectState> GetEffectState()
    {
        return new List<EffectState>() { EffectState.BUFF_DR };
    }

    public override int GetRange()
    {
        return 0;
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(Protect));
    }
}
