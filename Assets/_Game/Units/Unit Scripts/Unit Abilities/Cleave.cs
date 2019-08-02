using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Cleave is an Ability derrived form the Attack Abstract Class (UnitAbility)
// Cleave hits opponents from an origin based on the unit's facing direction and range, 
// and also damages tiles so that the area of effect forms a perpendicular line of 3 tiles long
// simply put, deals damage in front of the unit, in a 3 tile wide perpendicular line

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

        Debug.Log( "Unit at (" + source.GetMapPosition().x + ", " + source.GetMapPosition().y + ") " 
            + source.GetDirection() + " is Cleaving at Origin Point (" + origin.x + ", " + origin.y + ")" );

        result.Add(origin);

        if (source.GetDirection() == Direction.N || source.GetDirection() == Direction.S)
        {
            if (MapMath.InMapBounds(new Vector2Int(origin.x + 1, origin.y))) { result.Add(new Vector2Int(origin.x + 1, origin.y)); }
            if (MapMath.InMapBounds(new Vector2Int(origin.x - 1, origin.y))) { result.Add(new Vector2Int(origin.x - 1, origin.y)); }
        }
        else if (source.GetDirection() == Direction.E || source.GetDirection() == Direction.W)
        {
            if (MapMath.InMapBounds(new Vector2Int(origin.x, origin.y + 1))) { result.Add(new Vector2Int(origin.x, origin.y + 1)); }
            if (MapMath.InMapBounds(new Vector2Int(origin.x, origin.y - 1))) { result.Add(new Vector2Int(origin.x, origin.y - 1)); }
        }
        

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
        return (inQuestion.GetType() == typeof(Cleave));
    }
}
