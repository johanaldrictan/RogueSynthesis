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
    public List<UnitAbility> Abilities;
}

public enum UnitType
{
    AlliedUnit,
    EnemyUnit,
    Civilian
};

public enum Direction
{
    N,
    W,
    E,
    S,
    NO_DIR
}

// A SpriteSet is a container for N,S,E,W facing sprites
[System.Serializable]
public class SpriteSet
{
    public Sprite north;
    public Sprite south;
    public Sprite east;
    public Sprite west;
}

