using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attack is an Abstract UnitAbility that deals damage, plus maybe more. Hits opponents in the corresponding direction/area

// If you want to create a new UnitAbility, refer to the comments/code on UnitAbility.cs and AbilityDatabase.cs

public abstract class Attack : UnitAbility
{
    // Attacks deal damage, and need a variable for how much damage it does
    public abstract int GetDamage();

    // Attacks have an area of effect, and need a way to get that effect for multiple purposes like applying damage or visual effects
    public abstract List<Vector2Int> GetAreaOfEffect(Unit source);

    public abstract void DealEffects(Unit target);


    // get the area of effect. iterate through it, dealing effects to each unit found
    public override void Execute(Unit source)
    {
        List<Vector2Int> area = GetAreaOfEffect(source);
        foreach (Vector2Int tile in area)
        {
            Unit searchResult = source.globalPositionalData.SearchLocation(tile);
            if (searchResult != null)
            {
                DealEffects(searchResult);
            }
        }
    }

    
    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(Attack));
    }
}
