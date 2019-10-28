using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attack is an Abstract UnitAbility that deals damage, plus maybe more. Hits opponents in the corresponding direction/area

// If you want to create a new UnitAbility, refer to the comments/code on UnitAbility.cs and AbilityDatabase.cs

public abstract class Attack : UnitAbility
{
    public bool isAOE;
    // Attacks deal damage, and need a variable for how much damage it does
    public abstract int GetDamage();

    // Attacks have an area of effect, and need a way to get that effect for multiple purposes like applying damage or visual effects
    public abstract List<Vector2Int> GetAreaOfEffect(Unit source, Direction direction);

    // This funciton takes a Unit that is to be hit with the Attack. The function deals the associated effects of the attack to the given Unit
    public abstract void DealEffects(Unit target, Unit source);

    public abstract void DealDelayedEffect(Unit target, Unit source);

    // get the area of effect. iterate through it, dealing effects to each unit found
    public override void Execute(Unit source, Direction direction)
    {
        List<Vector2Int> area = GetAreaOfEffect(source, direction);
        //Debug.Log(direction);
        //if it is an aoe attack, deal effects to every unit 
        if (isAOE)
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
            //deal damage to first unit found
            if (searchResult != null)
            {
                DealEffects(searchResult, source);
            }
        }
    }

}
