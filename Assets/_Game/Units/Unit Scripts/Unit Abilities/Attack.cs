using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attack is an Abstract type of UnitAbility that deals damage, plus maybe more. Hits opponents in the corresponding direction/area

// If you want to create a new UnitAbility, refer to the comments/code on UnitAbility.cs and AbilityDatabase.cs

public abstract class Attack : UnitAbility
{
    // signifies whether or not this Attack is an Area-of-Effect (AOE)
    // AOE's will deal effects to all Units found in the Area
    // Non-AOE's will deal the effect to the first Unit found only
    public abstract bool isAOE();

    // Attacks deal damage, and need a variable for how much damage it does
    public abstract int GetDamage();

    // Attacks have an area of effect, and need a way to get that effect for multiple purposes like applying damage or visual effects
    public abstract List<Vector2Int> GetAreaOfEffect(Vector2Int source, Direction direction);

    // This funciton takes a Unit that is to be hit with the Attack. The function deals the associated effects of the attack to the given Unit
    public abstract void DealEffects(Unit target, Unit source);

    // Takes a Unit as a parameter and returns whether or not this Ability would kill that target
    public virtual bool LethalAttack(Unit target)
    {
        return (target.GetHealth() + target.GetDamageReduction() <= this.GetDamage());
    }

    // get the area of effect. iterate through it, dealing effects to each unit found in the area
    public override void Execute(Unit source, Direction direction)
    {
        List<Vector2Int> area = GetAreaOfEffect(source.GetMapPosition(), direction);

        //if it is an aoe attack, deal effects to every unit 
        if (isAOE())
        {
            foreach (Vector2Int tile in area)
            {
                Unit searchResult = source.globalPositionalData.SearchLocation(tile);
                if (searchResult != null)
                {
                    DealEffects(searchResult, source);
                }
            }
        }

        //if it is not an aoe attack, deal effects to the first unit
        else
        {
            Unit searchResult = null;
            foreach (Vector2Int tile in area)
            {
                searchResult = source.globalPositionalData.SearchLocation(tile);
                if (searchResult != null)
                {
                    break;
                }
            }
            DealEffects(searchResult, source);
        }
    }

}
