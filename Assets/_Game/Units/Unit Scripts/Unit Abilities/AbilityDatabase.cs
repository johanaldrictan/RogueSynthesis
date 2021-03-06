﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This is a database that stores all possible types of activatable Abilities

// If you want to create a new Ability:
// 1. Implement that ability by making an inherited member of the UnitAbility Abstract class
// 2. Make this script known of that new ability's existence by manually filling out the spaces where necessary. The areas are marked by "**********************"
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
            // add the current Ability to the result, if it isn't inferior
            if (! abilityDataBase[(int)abilities[i]].AmIInferior(result))
            { result.Add(abilityDataBase[(int)abilities[i]]); }
        }

        return result;
    }

    // The list of every member inherited from UnitAbility
    // When writing a new Ability, update the contents of this list manually. 
    // Make sure that the index where you find it is equal to the value in the enum below
    private static List<UnitAbility> abilityDataBase = new List<UnitAbility> // ***************************
    {
        new Wait(),
        new Cleave(),
        new ShoulderTackle(),
        new Abduct(),
        new Swipe(),
        new Focus(),
        new ImmobilizationBeam(),
        new WatcherGaze(),
        new Infect(),
        new Protect(),
        new Aim(),
        new Snipe(), 
        new Claymore(),
        new Launch(),
        new ConcussionGrenade(),
        new EMPGrenade()
    };
}


// the name of the ability (in the inspector), referring to the value of the index in the abilityDataBase List.
// When writing a new Ability, update this enum manually.
// Make sure that its value is the correct index in the list
public enum Ability // ****************
{
    Wait = 0,
    Cleave = 1,
    ShoulderTackle = 2,
    Abduct = 3,
    Swipe = 4,
    Focus = 5,
    ImmobilizationBeam = 6,
    WatcherGaze = 7,
    Infect = 8,
    Protect = 9,
    Aim = 10,
    Snipe = 11,
    Claymore = 12,
    Launch = 13, 
    ConcussionGrenade = 14,
    EMPGrenade = 15
};