﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ShoulderTackle is an Ability derrived form the Attack Abstract Class (UnitAbility)
// ShoulderTackle Dashes the unit to a tile before the target and knocks the first target hit, back 5 tiles	

// If you want to create a new UnitAbility, refer to the comments/code on UnitAbility.cs and AbilityDatabase.cs

public class ShoulderTackle : Attack
{
    public override void DealEffects(Unit target, Unit source)
    {
        int pushback = 5;

        if (target != null)
        {
            target.ChangeHealth((GetDamage() * (-1)), source, this);
            
            Vector2Int diff = target.GetMapPosition() - source.GetMapPosition();
            Vector2Int absDiff = new Vector2Int(Mathf.Abs(diff.x), Mathf.Abs(diff.y));
            Debug.Assert(source.GetDirection() != Direction.NO_DIR);
            Vector2Int newLocation = source.GetMapPosition();
            //Debug.Log(newLocation);
            switch (source.GetDirection())
            {
                case Direction.N:
                    newLocation = new Vector2Int(source.GetMapPosition().x, source.GetMapPosition().y + (absDiff.y-1));
                    break;
                case Direction.S:
                    newLocation = new Vector2Int(source.GetMapPosition().x, source.GetMapPosition().y - (absDiff.y-1));
                    break;
                case Direction.W:
                    newLocation = new Vector2Int(source.GetMapPosition().x - (absDiff.x-1), source.GetMapPosition().y);
                    break;
                case Direction.E:
                    newLocation = new Vector2Int(source.GetMapPosition().x + (absDiff.x-1), source.GetMapPosition().y);
                    break;
            }
            //Debug.Log(newLocation);
            //check new location for issues, if so, stay at current loc
            // ********************************** source.Move(newLocation.x, newLocation.y, MovementType.DASH);
            AttackHelper.DisplaceUnit(target, source, this, pushback, source.GetDirection());
            
        }
        else
        {
            Vector2Int newLocation = source.GetMapPosition() + (MapMath.DirToRelativeLoc(source.GetDirection()) * pushback);
            // ********************************** source.Move(newLocation.x, newLocation.y, MovementType.DASH);
        }
        
        
    }

    // we're making a list of coordinates that this attack reaches
    public override List<Vector2Int> GetAreaOfEffect(Vector2Int source, Direction direction)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        Vector2Int origin = source;

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

    public override bool isAOE()
    {
        return false;
    }

    public override int GetDamage()
    {
        return 3;
    }

    public override List<EffectState> GetEffectState()
    {
        return new List<EffectState> { EffectState.DAMAGE, EffectState.KNOCKBACK };
    }

    public override string GetName()
    {
        return "Shoulder Tackle";
    }

    public override int GetRange()
    {
        return 3;
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(ShoulderTackle));
    }

    public override string GetSoundEvent()
    {
        return "event:/PRO/PRO1/PRO_ShoulderTackle";
    }
}

