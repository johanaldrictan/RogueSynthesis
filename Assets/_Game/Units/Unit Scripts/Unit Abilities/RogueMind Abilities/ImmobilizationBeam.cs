using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// If you want to create a new UnitAbility, refer to the comments/code on UnitAbility.cs and AbilityDatabase.cs

public class ImmobilizationBeam : Attack
{
    public override bool isAOE()
    {
        return false;
    }

    // We're just doing straight damage here
    public override void DealEffects(Unit target, Unit source)
    {
        if (target != null)
        {
            target.ChangeHealth((GetDamage() * (-1)), source, this);
            target.Immobilize(1);
        }
    }

    public override List<Vector2Int> GetAreaOfEffect(Vector2Int source, Direction direction)
    {
        List<Vector2Int> result = AttackHelper.GetLineAOE(source, direction, GetRange());
        return result;
    }

    public override int GetDamage()
    {
        return 5;
    }

    public override List<EffectState> GetEffectState()
    {
        return new List<EffectState>() { EffectState.DAMAGE, EffectState.IMMOBILIZE };
    }

    public override string GetName()
    {
        return "Immobilization Beam";
    }

    public override int GetRange()
    {
        return 13;
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(ImmobilizationBeam));
    }

    public override string GetSoundEvent()
    {
        return "event:/WAT/WAT_ImmoBeam";
    }
}
