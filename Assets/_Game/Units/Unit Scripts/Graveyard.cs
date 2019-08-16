using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Graveyard
{
    // the storage of dead enemy units. 
    private List<Unit> enemies;

    private List<Unit> allies;

    // constructor. simply creates the Lists
    public Graveyard()
    {
        enemies = new List<Unit>();
        allies = new List<Unit>();
    }

    // wipes the storage
    public void Wipe()
    {
        enemies.Clear();
        allies.Clear();
    }
}
