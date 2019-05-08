using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A SpriteSet is a container for N,S,E,W facing sprites
[CreateAssetMenu]
public class SpriteSet : ScriptableObject
{
    public Sprite north;
    public Sprite south;
    public Sprite east;
    public Sprite west;
}
