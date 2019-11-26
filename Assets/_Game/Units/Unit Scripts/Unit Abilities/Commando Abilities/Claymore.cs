using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claymore : UnitAbility
{
    public override void Execute(Unit source, Direction direction)
    {
        abilitySoundEvent.start();
        Vector2Int position = MapMath.DirToRelativeLoc(direction) + source.GetMapPosition();
        // make sure the place this wants to be put is not obstructed
        if (MapController.instance.weightedMap[position] != (int)TileWeight.OBSTRUCTED && TurnController.instance != null)
        {
            TurnController.instance.SetTrap(position, TrapType.Claymore, source, this);
        }
    }

    public override List<EffectState> GetEffectState()
    {
        return new List<EffectState> { EffectState.DAMAGE, EffectState.DISABLE };
    }

    public override string GetName()
    {
        return "Claymore";
    }

    public override int GetRange()
    {
        return 2;
    }

    public override string GetSoundEvent()
    {
        return "event:/SHA/SHA_ClaymorePlace";
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(Claymore));
    }
}
