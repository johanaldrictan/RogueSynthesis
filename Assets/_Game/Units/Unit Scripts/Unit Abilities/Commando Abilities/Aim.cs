using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : Attack
{
    public Aim()
    {
        isAOE = false;
    }
    public override void DealEffects(Unit target, Unit source)
    {
        source.attackBuffed = true;
    }

    public override List<Vector2Int> GetAreaOfEffect(Vector2Int source, Direction direction)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        result.Add(source);
        return result;
    }

    public override int GetDamage()
    {
        return 0;
    }

    public override List<EffectState> GetEffectState()
    {
        return new List<EffectState> { EffectState.BUFF_DR };
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
}
