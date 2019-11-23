using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// If you want to create a new UnitAbility, refer to the comments/code on UnitAbility.cs and AbilityDatabase.cs

public class Infect : Attack
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
            //deal core damage to central target
            target.ChangeHealth((GetDamage() * (-1)), source, this);
        }
        Vector2Int origin = source.GetMapPosition();
        origin += MapMath.DirToRelativeLoc(source.GetDirection());
        List<Vector2Int> area = AttackHelper.GetCircleAOE(origin, source.GetDirection(), GetRange());
        //skip first one(center)
        for (int i = 0; i < area.Count; i++)
        {
            if (area[i] == source.GetMapPosition())
                continue;
            Unit searchResult = source.globalPositionalData.SearchLocation(area[i]);
            if (searchResult != null)
            {
                searchResult.ChangeHealth((GetDamage() * -1) / 3, source, this);
                Vector2Int diff = searchResult.GetMapPosition() - origin;
                // AttackHelper.DisplaceUnit(searchResult, source, this, 1, MapMath.LocToDirection(diff));
            }
        }
        source.ChangeHealth((GetDamage() * (-1)), source, this);
    }

    public override List<Vector2Int> GetAreaOfEffect(Vector2Int source, Direction direction)
    {
        //get tile in front
        List<Vector2Int> result = new List<Vector2Int>();
        Vector2Int origin = source;
        origin += MapMath.DirToRelativeLoc(direction);

        if (!MapMath.InMapBounds(origin))
        { return result; }

        /*
        Debug.Log( "Unit at (" + source.GetMapPosition().x + ", " + source.GetMapPosition().y + ") " 
            + source.GetDirection() + " is Cleaving at Origin Point (" + origin.x + ", " + origin.y + ")" );
        */

        result.Add(origin);
        return result;
    }

    public override int GetDamage()
    {
        return 15;
    }

    public override List<EffectState> GetEffectState()
    {
        return new List<EffectState>() { EffectState.DAMAGE, EffectState.KNOCKBACK };
    }

    public override string GetName()
    {
        return "Infect";
    }

    public override int GetRange()
    {
        return 1;
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(Infect));
    }

    public override string GetSoundEvent()
    {
        throw new System.NotImplementedException();
    }
}
