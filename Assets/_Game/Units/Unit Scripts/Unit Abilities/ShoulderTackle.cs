using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ShoulderTackle is an Ability derrived form the Attack Abstract Class (UnitAbility)
// ShoulderTackle Dashes the unit to a tile before the target and knocks the first target hit, back 5 tiles	

// If you want to create a new UnitAbility, refer to the comments/code on UnitAbility.cs and AbilityDatabase.cs

public class ShoulderTackle : Attack
{
    // TODO: NEED TO FIX CASE WHEN THERE IS SOMETHING BEHIND TARGET
    public override void DealEffects(Unit target, Unit source)
    {
        int pushback = 3;

        if (target != null)
        {
            target.ChangeHealth((GetDamage() * (-1)), source, this);
            Vector2Int diff = target.GetMapPosition() - source.GetMapPosition();
            Debug.Assert(source.GetDirection() != Direction.NO_DIR);
            Vector2Int newLocation = source.GetMapPosition();
            if (source.GetDirection() == Direction.N || source.GetDirection() == Direction.S)
            {
                newLocation = new Vector2Int(source.GetMapPosition().x, source.GetMapPosition().y + (diff.y-1));
            }
            else
            {
                newLocation = new Vector2Int(source.GetMapPosition().x + (diff.x-1), source.GetMapPosition().y);
            }
            //check new location for issues, if so, stay at current loc
            source.Move(newLocation.x, newLocation.y);
            Vector2Int pushbackLoc = target.GetMapPosition() + (MapMath.DirToRelativeLoc(source.GetDirection()) * pushback);
            //look for collisions
            List<Vector2Int> pushbackArea = GetAreaOfEffect(target, source.GetDirection());
            Vector2Int? searchResult = null;
            foreach (Vector2Int tile in pushbackArea)
            {
                if (MapController.instance.map[tile.x, tile.y] == (int)TileWeight.OBSTRUCTED)
                {
                    searchResult = tile;
                    break;
                }
            }
            if(searchResult != null)
            {
                Debug.Log(searchResult);
                Vector2Int newPushDiff = (Vector2Int)searchResult - target.GetMapPosition();
                Debug.Log(newPushDiff);
                //change pushbackLoc in some way
                if (source.GetDirection() == Direction.N || source.GetDirection() == Direction.S)
                {
                    pushbackLoc = new Vector2Int(target.GetMapPosition().x, target.GetMapPosition().y + (newPushDiff.y));
                }
                else
                {
                    pushbackLoc = new Vector2Int(target.GetMapPosition().x + (newPushDiff.x), target.GetMapPosition().y);
                }
                Debug.Log(pushbackLoc);
            }
            //maybe do out of bounds checks here to kill unit if pushed out of bounds or on certain tiles
            if (MapMath.InMapBounds(pushbackLoc))
                target.Move(pushbackLoc.x, pushbackLoc.y);
            //else if()
            else
            {
                DeathData data = new DeathData(source, this, GetDamage(), target.GetMapPosition());
                target.KillMe(data);
            }
        }
        else
        {
            Vector2Int newLocation = source.GetMapPosition() + (MapMath.DirToRelativeLoc(source.GetDirection()) * pushback);
            source.Move(newLocation.x, newLocation.y);
        }
        
        
    }
    public override void DealDelayedEffect(Unit target, Unit source)
    {
        //this has no delayed effect
    }

    // we're making a list of coordinates that this attack reaches
    // Cleave has an origin point range tiles in front of the Unit (default 1)
    // it also hits the two adjacent squares on either side of the origin
    public override List<Vector2Int> GetAreaOfEffect(Unit source, Direction direction)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        Vector2Int origin = source.GetMapPosition();

        origin += MapMath.DirToRelativeLoc(direction);

        if (!MapMath.InMapBounds(origin))
        { return result; }

        /*
        Debug.Log( "Unit at (" + source.GetMapPosition().x + ", " + source.GetMapPosition().y + ") " 
            + source.GetDirection() + " is Cleaving at Origin Point (" + origin.x + ", " + origin.y + ")" );
        */
        //one tile forward
        result.Add(origin);

        for (int i = 0; i < GetRange() - 1; i++)
        {
            origin += MapMath.DirToRelativeLoc(direction);
            result.Add(origin);
        }
        return result;
    }
    public List<Vector2Int> GetAreaOfEffect(Unit source, Direction direction, int pushbackDist)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        Vector2Int origin = source.GetMapPosition();

        origin += MapMath.DirToRelativeLoc(direction);

        if (!MapMath.InMapBounds(origin))
        { return result; }

        /*
        Debug.Log( "Unit at (" + source.GetMapPosition().x + ", " + source.GetMapPosition().y + ") " 
            + source.GetDirection() + " is Cleaving at Origin Point (" + origin.x + ", " + origin.y + ")" );
        */
        //one tile forward
        result.Add(origin);

        for (int i = 0; i < pushbackDist - 1; i++)
        {
            origin += MapMath.DirToRelativeLoc(direction);
            result.Add(origin);
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

