using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// If you want to create a new UnitAbility, refer to the comments/code on UnitAbility.cs and AbilityDatabase.cs

public class WatcherGaze : Attack
{
    public WatcherGaze()
    {
        abilitySoundEvent = FMODUnity.RuntimeManager.CreateInstance(GetSoundEvent());
    }
    public override bool isAOE()
    {
        return true;
    }


    public override void DealEffects(Unit target, Unit source)
    {
        target.ChangeHealth( (GetDamage()*(-1)), source, this);
    }

    public override List<Vector2Int> GetAreaOfEffect(Vector2Int source, Direction direction)
    {
        List<Vector2Int> result = AttackHelper.GetLineAOE(source, direction, GetRange());
        return result;
    }

    public override int GetDamage()
    {
        return 18;
    }

    public override List<EffectState> GetEffectState()
    {
        return new List<EffectState> { EffectState.DAMAGE};
    }

    public override string GetName()
    {
        return "Watcher's Gaze";
    }

    public override int GetRange()
    {
        return 3;
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(WatcherGaze));
    }

    public override string GetSoundEvent()
    {
        return "event:/WAT/WAT_WatcherEye";
    }
}
