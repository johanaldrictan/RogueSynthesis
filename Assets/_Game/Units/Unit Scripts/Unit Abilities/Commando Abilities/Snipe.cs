using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snipe : Attack
{
    public Snipe()
    {
        isAOE = false;
        damageBuff = 10;
    }
    public override void DealEffects(Unit target, Unit source)
    {
        int attackModifier = 0;
        if (source.attackBuffed)
            attackModifier = damageBuff;
        target.ChangeHealth(((GetDamage() + attackModifier) * (-1)), source, this);
    }

    public override List<Vector2Int> GetAreaOfEffect(Vector2Int source, Direction direction)
    {
        List<Vector2Int> result = AttackHelper.GetLineAOE(source, direction, GetRange());
        return result;
    }

    public override int GetDamage()
    {
        return 20;
    }

    public override List<EffectState> GetEffectState()
    {
        return new List<EffectState> { EffectState.DAMAGE };
    }

    public override string GetName()
    {
        return "Snipe";
    }

    public override int GetRange()
    {
        return 16;
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(Snipe));
    }
}
