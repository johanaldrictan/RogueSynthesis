using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    EMP Grenade: 
    The player selects a tile in a line within a given range
    Once the tile has been selected. Damage is received by the 
    selected tile and the tiles adjacent to it. Addtionally, those
    units become disabled for 1 turn
*/
public class EMPGrenade : Attack
{
    public override void DealEffects(Unit target, Unit source)
    {
        if (target != null)
        {
            Vector2Int origin = target.GetMapPosition();
            target.ChangeHealth((GetDamage() * (-1)), source, this);
            target.Disable(1);
            foreach (Vector2Int tile in MapMath.GetNeighbors(origin).Keys)
            {
                Unit searchResult = source.globalPositionalData.SearchLocation(tile);
                if (searchResult != null)
                {
                    searchResult.ChangeHealth((GetDamage() * (-1)), source, this);
                    searchResult.Disable(1);
                }
            }
        }
    }

    public override List<Vector2Int> GetAreaOfEffect(Vector2Int source, Direction direction)
    {
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
        return "EMP Grenade";
    }

    public override int GetRange()
    {
        return 7;
    }

    public override string GetSoundEvent()
    {
        return "event:/GRE/GRE_EMP";
    }

    public override bool isAOE()
    {
        return false;
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(EMPGrenade));
    }
}
