using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ShoulderTackle is an Ability derrived form the Attack Abstract Class (UnitAbility)
// ShoulderTackle Dashes the unit to a tile before the target and knocks the first target hit, back 5 tiles	

// If you want to create a new UnitAbility, refer to the comments/code on UnitAbility.cs and AbilityDatabase.cs

public class ShoulderTackle : Attack
{
    // We're just doing straight damage here
    public override void DealEffects(Unit target, Unit source)
    {
        target.ChangeHealth((GetDamage() * (-1)), source, this);
    }

    // we're making a list of coordinates that this attack reaches
    // Cleave has an origin point range tiles in front of the Unit (default 1)
    // it also hits the two adjacent squares on either side of the origin
    public override List<Vector2Int> GetAreaOfEffect(Unit source, Direction direction)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        Vector2Int origin = source.GetMapPosition();

        for (int i = 0; i < GetRange(); i++)
        {
            origin += MapMath.DirToRelativeLoc(direction);
        }

        if (!MapMath.InMapBounds(origin))
        { return result; }

        /*
        Debug.Log( "Unit at (" + source.GetMapPosition().x + ", " + source.GetMapPosition().y + ") " 
            + source.GetDirection() + " is Cleaving at Origin Point (" + origin.x + ", " + origin.y + ")" );
        */

        result.Add(origin);

        if (direction == Direction.N || direction == Direction.S)
        {
            if (MapMath.InMapBounds(new Vector2Int(origin.x + 1, origin.y))) { result.Add(new Vector2Int(origin.x + 1, origin.y)); }
            if (MapMath.InMapBounds(new Vector2Int(origin.x - 1, origin.y))) { result.Add(new Vector2Int(origin.x - 1, origin.y)); }
        }
        else if (direction == Direction.E || direction == Direction.W)
        {
            if (MapMath.InMapBounds(new Vector2Int(origin.x, origin.y + 1))) { result.Add(new Vector2Int(origin.x, origin.y + 1)); }
            if (MapMath.InMapBounds(new Vector2Int(origin.x, origin.y - 1))) { result.Add(new Vector2Int(origin.x, origin.y - 1)); }
        }


        return result;
    }

    public override int GetDamage()
    {
        return 3;
    }

    public override int GetRange()
    {
        return 3;
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(Cleave));
    }
}

