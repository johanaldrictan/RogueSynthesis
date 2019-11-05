using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protect : Attack
{
    public Protect()
    {
        isAOE = false;
    }
    public override void DealEffects(Unit target, Unit source)
    {
        source.damageReductionBuffed = true;
    }

    public override List<Vector2Int> GetAreaOfEffect(Vector2Int source, Direction direction)
    {
        List<Vector2Int> result = new List<Vector2Int>();

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
