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
        int attackModifier = 0;
        if (source.attackBuffed)
            attackModifier = damageBuff;
        target.ChangeHealth(((GetDamage() + attackModifier) * (-1)), source, this);
    }

    public override List<Vector2Int> GetAreaOfEffect(Unit source, Direction direction)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        result.Add(source.GetMapPosition());
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
        return (inQuestion.GetType() == typeof(Aim));
    }
}
