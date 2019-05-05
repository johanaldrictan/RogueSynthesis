using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// A SpecialEffectUnityEvent is a type of UnityEvent that takes a source Unit and a target Unit.
// It is meant for special activatable effects of units.
[System.Serializable]
public class SpecialEffectUnityEvent : UnityEvent<Unit, Unit> { }

// A UnitData object is a ScriptableObject that stores parameters for making a unit
// the data stored here is the core of what a Unit is made out of, and is used for building the actual unit
[CreateAssetMenu]
public class UnitData : ScriptableObject
{
    public string unitName;
    public UnitType unitType;
    public Sprite sprite;
    public int moveSpeed;
    public int HP;
    public int ATK;
    public bool isRanged;
    public List<UnityEvent> Abilities;


    public void test()
    {

    }
}

public enum UnitType
{
    AlliedUnit,
    EnemyUnit,
    Civilian
};