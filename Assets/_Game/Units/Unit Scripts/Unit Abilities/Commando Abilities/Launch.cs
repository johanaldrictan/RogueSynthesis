using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : Attack
{
    public Launch()
    {
        abilitySoundEvent = FMODUnity.RuntimeManager.CreateInstance(GetSoundEvent());
    }
    public override void DealEffects(Unit target, Unit source)
    {
        target.ChangeHealth((GetDamage() * (-1)), source, this);
    }

    public override List<Vector2Int> GetAreaOfEffect(Vector2Int source, Direction direction)
    {
        return AttackHelper.GetLineAOE(source, direction, GetRange());
    }

    public override int GetDamage()
    {
        return 10;
    }

    public override List<EffectState> GetEffectState()
    {
        return new List<EffectState> { EffectState.DAMAGE };
    }

    public override string GetName()
    {
        return "Launch";
    }

    public override int GetRange()
    {
        return 8;
    }

    public override string GetSoundEvent()
    {
        return "event:/GRE/GRE_Launch";
    }

    public override bool isAOE()
    {
        return false;
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(Launch));
    }
}
