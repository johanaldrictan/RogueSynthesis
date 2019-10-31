using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// If you want to create a new UnitAbility, refer to the comments/code on UnitAbility.cs and AbilityDatabase.cs

public class Infect : Attack
{
    public Infect()
    {
        isAOE = false;
    }
    // We're just doing straight damage here
    public override void DealEffects(Unit target, Unit source)
    {
        if (target != null)
        {
            //deal core damage to central target
            target.ChangeHealth((GetDamage() * (-1)), source, this);
        }
        List<Vector2Int> area = AttackHelper.GetCircleAOE(target.GetMapPosition(), source.GetDirection(), GetRange());
        //skip first one(center)
        for (int i = 1; i < area.Count; i++)
        {
            Unit searchResult = source.globalPositionalData.SearchLocation(area[i]);
            if(searchResult != null)
                searchResult.ChangeHealth((GetDamage() * -1)/3, source, this);
        }
        
        DeathData data = new DeathData(source, this, 99, target.GetMapPosition());
        source.KillMe(data);
    }

    public override void DealDelayedEffect(Unit target, Unit source)
    {
        //do nothing because this does not have a delayed effect
    }

    public override List<Vector2Int> GetAreaOfEffect(Unit source, Direction direction)
    {
        //get tile in front
        List<Vector2Int> result = new List<Vector2Int>();
        Vector2Int origin = source.GetMapPosition();
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

    public override int GetRange()
    {
        return 1;
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(Cleave));
    }
}
