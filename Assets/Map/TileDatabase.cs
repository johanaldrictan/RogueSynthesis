using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDatabase : MonoBehaviour
{
    public static TileDatabase instance;
    public Dictionary<string, TileWeight> tileDB;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
        //then add to tiledb
        tileDB = new Dictionary<string, TileWeight>();
        //should contain a list of all tiles in the game.
        //probably should look into scriptable objects?
        tileDB.Add("landscapeTiles_067", TileWeight.UNOBSTRUCTED);
    }
}
public enum TileWeight
{
    //grass
    UNOBSTRUCTED = 1,
    LIGHT_OBS = 2,
    MEDIUM_OBS = 3,
    HEAVY_OBS = 4,
    //mountains, other units
    OBSTRUCTED = 99
}

