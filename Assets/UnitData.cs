using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A UnitData object is a ScriptableObject that stores parameters for making a unit
// the data stored here is the core of what a Unit is made out of, and is used for building the actual unit

public delegate void SpecialEffect(Unit source, Unit target);

[CreateAssetMenu]
public class UnitData : ScriptableObject
{
    public string unitName;
    public Unit unitType;
    public Sprite sprite;
    public int moveSpeed;
    public int HP;
    public int ATK;
    public bool isRanged;

    [SerializeField] public List<SpecialEffect> Abilities;

}
