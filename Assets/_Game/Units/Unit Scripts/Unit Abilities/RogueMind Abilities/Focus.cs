using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// If you want to create a new UnitAbility, refer to the comments/code on UnitAbility.cs and AbilityDatabase.cs

public class Focus : Attack
{
    public Focus()
    {
        isAOE = false;
    }
    // We're just doing straight damage here
    public override void DealEffects(Unit target, Unit source)
    {
        if (source.GetDamageReduction() < 5)
            source.SetDamageReduction(1);
    }

    public override List<Vector2Int> GetAreaOfEffect(Unit source, Direction direction)
    {
        List<Vector2Int> result = new List<Vector2Int>();

        return result;
    }

    public override int GetDamage()
    {
        return 0;
    }

    public override int GetRange()
    {
        return 0;
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(Focus));
    }
}
