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

    public override List<Vector2Int> GetAreaOfEffect(Unit source, Direction direction)
    {
        List<Vector2Int> result = AttackHelper.GetLineAOE(source.GetMapPosition(), direction, GetRange());
        return result;
    }

    public override int GetDamage()
    {
        return 20;
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
