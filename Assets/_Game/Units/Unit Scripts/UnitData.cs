using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// A UnitData object is a ScriptableObject that stores parameters for making a unit
// the data stored here is the core of what a Unit is made out of, and is used for building the actual unit
[CreateAssetMenu]
public class UnitData : ScriptableObject
{
    public string unitName;
    public UnitType unitType;
    public SpriteSet sprites;
    public int moveSpeed;
    public int health;
    public int attack;

    public List<Ability> abilities;
}

public enum UnitType
{
    AlliedUnit,
    EnemyUnit,
    Civilian
};


