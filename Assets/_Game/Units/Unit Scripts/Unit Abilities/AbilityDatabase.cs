using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This is a database that stores all possible types of UnitAbility objects

// If you want to create a new Ability:
// 1. Implement that ability by making an inherited member of the UnitAbility Abstract class
// 2. Make this script known of that new ability's existence by manually filling out the spaces where necessary. follow the comments.
// 3. Make sure that the steps from #2 are all done in the same order

public static class AbilityDatabase
{

    // This method takes a List of Ability enums and returns a List of real Abilities
    public static List<UnitAbility> GetAbilities(List<Ability> abilities)
    {
        List<UnitAbility> result = new List<UnitAbility>();

        // iterate through the List of Abilities given as a parameter
        for (int i = 0; i < abilities.Count; i++)
        {
            // remove inferior versions of the current Ability from the result
            abilityDataBase[(int)abilities[i]].RemoveInferiors(result);
            // add the current Ability to the result
            result.Add(abilityDataBase[(int)abilities[i]]);
        }

        return result;
    }

    // The list of every member inherited from UnitAbility
    // When writing a new Ability, update the contents of this list manually. 
    // Make sure that the index where you find it is equal to the value in the enum below
    private static List<UnitAbility> abilityDataBase = new List<UnitAbility>
    {
        new Wait()
    };
}


// the name of the ability (in the inspector), referring to the value of the index in the abilityDataBase List.
// When writing a new Ability, update this enum manually.
// Make sure that its value is the correct index in the list
public enum Ability
{
    Wait = 0
};