using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AreaAttack is a type of Attack that also has a point of origin.
/// the GetAreaOfEffect gets the area of tiles that, from the arbitraty origin point, are affected
/// </summary>

public abstract class AreaAttack : Attack
{
    // AreaAttacks require an origin point, selected by the Unit.
    // this origin point defines where the Ability's effects take place
    protected Vector2Int origin;

    // This function simply assigns the origin variable
    public void ChooseOrigin(Vector2Int newOrigin)
    {
        origin = newOrigin;
    }

    // Returns a List of all possible places that could be selected as an origin point
    public abstract List<Vector2Int> GetPossibleOrigins(Unit source, Direction direction);


    // get the area of effect. iterate through it, dealing effects to each unit found in the area
    public override void Execute(Unit source, Direction direction)
    {
        if (abilitySoundEvent.isValid())
            abilitySoundEvent.start();

        List<Vector2Int> area = GetAreaOfEffect(origin, direction); // <-- NOTE: this is the only difference between this function and Attack.Execute ; GetAreaOfEffect uses origin instead of the sourceUnit's position

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
