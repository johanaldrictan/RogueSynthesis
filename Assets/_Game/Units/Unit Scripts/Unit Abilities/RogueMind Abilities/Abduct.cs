using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// If you want to create a new UnitAbility, refer to the comments/code on UnitAbility.cs and AbilityDatabase.cs

public class Abduct : Attack
{
    private Direction startingDirection;

    public override bool isAOE()
    {
        return false;
    }

    public override void DealEffects(Unit target, Unit source)
    {
        target.ChangeHealth(GetDamage() * (-1), source, this);
        target.Disable(1);
        NewDelayedEffectEvent.Invoke(new DelayedEffect(AbductTarget, source.globalPositionalData, 0, UnitType.AlliedUnit, false, new List<Unit> { target }, source));
        NewDelayedEffectEvent.Invoke(new DelayedEffect(Root, source.globalPositionalData, 0, UnitType.EnemyUnit, false, new List<Unit> { source }, source));
    }

    public void AbductTarget(Unit target, Unit source)
    {
        if (source.GetHealth() > 0 && GetAreaOfEffect(source.GetMapPosition(), startingDirection).Contains(target.GetMapPosition()))
        {
            target.Disable(1);
            NewDelayedEffectEvent.Invoke(new DelayedEffect(AbductTarget, source.globalPositionalData, 1, UnitType.AlliedUnit, false, new List<Unit>{ target }, source));
        }
    }

    public void Root(Unit target, Unit source)
    {
        if (target.GetHealth() > 0 && GetAreaOfEffect(source.GetMapPosition(), startingDirection).Contains(target.GetMapPosition()))
        {
            source.Immobilize(1);
            NewDelayedEffectEvent.Invoke(new DelayedEffect(Root, source.globalPositionalData, 1, UnitType.EnemyUnit, false, new List<Unit> { source }, source));
        }
    }

    public override List<Vector2Int> GetAreaOfEffect(Vector2Int source, Direction direction)
    {
        startingDirection = direction;
        return AttackHelper.GetLineAOE(source, direction, GetRange());
    }

    public override int GetDamage()
    {
        return 3;
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
