﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UNINPLEMENTED

public class Protect : UnitAbility
{
    public Protect()
    {
        abilitySoundEvent = FMODUnity.RuntimeManager.CreateInstance(GetSoundEvent());
    }
    public override void Execute(Unit source, Direction direction)
    {
        abilitySoundEvent.start();
    }

    public override List<EffectState> GetEffectState()
    {
        return new List<EffectState>() { EffectState.BUFF_DR };
    }

    public override string GetName()
    {
        return "Protect";
    }

    public override int GetRange()
    {
        return 0;
    }

    public override string GetSoundEvent()
    {
        return "event:/PRO1/PRO_Protect";
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(Protect));
    }
}
