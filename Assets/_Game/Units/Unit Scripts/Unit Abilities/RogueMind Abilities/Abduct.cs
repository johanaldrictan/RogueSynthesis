using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// If you want to create a new UnitAbility, refer to the comments/code on UnitAbility.cs and AbilityDatabase.cs

public class Abduct : Attack
{
    public override bool isAOE()
    {
        return true;
    }

    // We're just doing straight damage here
    public override void DealEffects(Unit target, Unit source)
    {
        
    }

    public override List<Vector2Int> GetAreaOfEffect(Vector2Int source, Direction direction)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        
        

        return result;
    }

    public override int GetDamage()
    {
        return 5;
    }

    public override List<EffectState> GetEffectState()
    {
        return new List<EffectState> { EffectState.DAMAGE, EffectState.DISABLE };
    }

    public override string GetName()
    {
        return "Abduct";
    }

    public override int GetRange()
    {
        return 1;
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(Abduct));
    }
}
