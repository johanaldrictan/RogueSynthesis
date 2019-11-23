using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// If you want to create a new UnitAbility, refer to the comments/code on UnitAbility.cs and AbilityDatabase.cs

public class Swipe : Attack
{
    public override bool isAOE()
    {
        return true;
    }

    public override void DealEffects(Unit target, Unit source)
    {
        DelayedEffect delayedEffect = new DelayedEffect(SwipeAttack, source.globalPositionalData, 1, UnitType.EnemyUnit, true, GetAreaOfEffect(source.GetMapPosition(), source.GetDirection()), source);
        NewDelayedEffectEvent.Invoke(delayedEffect);
    }

    public void SwipeAttack(Unit target, Unit source)
    {
        target.ChangeHealth((GetDamage() * (-1)), source, this);
    }

    public override List<Vector2Int> GetAreaOfEffect(Vector2Int source, Direction direction)
    {
        List<Vector2Int> result = AttackHelper.GetTShapedAOE(source, direction, GetRange());

        return result;
    }

    public override int GetDamage()
    {
        return 5;
    }

    public override int GetRange()
    {
        return 1;
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(Swipe));
    }

    public override List<EffectState> GetEffectState()
    {
        return new List<EffectState> { EffectState.DAMAGE };
    }

    public override string GetName()
    {
        return "Swipe";
    }
}
