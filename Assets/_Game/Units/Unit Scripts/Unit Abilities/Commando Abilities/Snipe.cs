using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Snipe is an Attack that deals damage in a long-range straight line
// it attacks the first Unit that is in the way of the attack; it does not go through that Unit

public class Snipe : Attack
{
    public Snipe()
    {
        abilitySoundEvent = FMODUnity.RuntimeManager.CreateInstance(GetSoundEvent());
    }
    public override bool isAOE()
    {
        return false;
    }

    public override void DealEffects(Unit target, Unit source)
    {
        int bonusDamage = 0;
        if (source.attackBuffed)
            bonusDamage = 10;
        
        target.ChangeHealth(((GetDamage() + bonusDamage) * (-1)), source, this);
    }

    public override List<Vector2Int> GetAreaOfEffect(Vector2Int source, Direction direction)
    {
        return AttackHelper.GetLineAOE(source, direction, GetRange());
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

    public override string GetSoundEvent()
    {
        return "event:/SHA/SHA_Snipe";
    }
}
