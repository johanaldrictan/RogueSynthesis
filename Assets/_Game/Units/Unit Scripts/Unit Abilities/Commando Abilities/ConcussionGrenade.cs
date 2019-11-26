using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Concussion Grenade: 
    The player selects a tile in a line within a given range
    Once the tile has been selected. Damage is received by the 
    selected tile and the tiles adjacent to it. Additionally, the
    affected units are pushed back away from the center tile
*/
public class ConcussionGrenade : Attack
{
    public override void DealEffects(Unit target, Unit source)
    {
        if (target != null)
        {
            Vector2Int origin = target.GetMapPosition();
            target.ChangeHealth((GetDamage() * (-1)), source, this);
            foreach (Vector2Int tile in MapMath.GetNeighbors(origin).Keys)
            {
                Unit searchResult = source.globalPositionalData.SearchLocation(tile);
                if (searchResult != null)
                {
                    Vector2Int diff = tile - origin;
                    AttackHelper.DisplaceUnit(searchResult, source, this, 1, MapMath.LocToDirection(diff));
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
        return 5;
    }

    public override List<EffectState> GetEffectState()
    {
        return new List<EffectState> { EffectState.DAMAGE, EffectState.KNOCKBACK };
    }

    public override string GetName()
    {
        return "Concussion Grenade";
    }

    public override int GetRange()
    {
        return 6;
    }

    public override string GetSoundEvent()
    {
        return "event:/GRE/GRE_Concussion";
    }

    public override bool isAOE()
    {
        return false;
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(ConcussionGrenade));
    }
}
