using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Cleave is an Ability derrived form the Attack Abstract Class (UnitAbility)
// Cleave hits opponents from an origin based on the unit's facing direction and range, 
// and also damages tiles in an 'X' shaped pattern from the origin

// If you want to create a new UnitAbility, refer to the comments/code on UnitAbility.cs and AbilityDatabase.cs

public class Cleave : Attack
{
    public override void DealEffects(Unit target)
    {
        target.ChangeHealth( GetDamage() * (-1) );
    }

    public override List<Vector2Int> GetAreaOfEffect(Unit source)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        Vector2Int origin = source.GetMapPosition();

        for (int i = 0; i < GetRange(); i++)
        {
            origin += MapMath.DirToRelativeLoc(source.GetDirection());
        }

        if (! MapMath.InMapBounds(origin))
        { return result; }

        result.Add(origin);
        if (MapMath.InMapBounds(new Vector2Int(origin.x + 1, origin.y + 1))) { result.Add(new Vector2Int(origin.x + 1, origin.y + 1)); }
        if (MapMath.InMapBounds(new Vector2Int(origin.x + 1, origin.y - 1))) { result.Add(new Vector2Int(origin.x + 1, origin.y - 1)); }
        if (MapMath.InMapBounds(new Vector2Int(origin.x - 1, origin.y + 1))) { result.Add(new Vector2Int(origin.x - 1, origin.y + 1)); }
        if (MapMath.InMapBounds(new Vector2Int(origin.x - 1, origin.y - 1))) { result.Add(new Vector2Int(origin.x - 1, origin.y - 1)); }

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
        return ( base.InferiorComparator(inQuestion) || inQuestion.GetType() == typeof(Attack) );
    }
}
