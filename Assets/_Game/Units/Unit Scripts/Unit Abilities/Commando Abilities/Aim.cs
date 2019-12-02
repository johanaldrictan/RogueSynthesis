using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Aim is a type of UnitAbility that makes its user deal more damage
// if the user, after using this Ability, uses the Snipe ability, it will do more damage
// This bonus is lost if the user moves

public class Aim : UnitAbility
{
    public override void Execute(Unit source, Direction direction)
    {
        source.attackBuffed = true;
        abilitySoundEvent.start();
    }

    public override int GetRange()
    {
        return 0;
    }

    public override List<EffectState> GetEffectState()
    {
        return new List<EffectState> { EffectState.BUFF_DMG };
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(Aim));
    }

    public override string GetName()
    {
        return "Aim";
    }

    public override string GetSoundEvent()
    {
        return "event:/SHA/SHA1/SHA_Aim";
    }
}
