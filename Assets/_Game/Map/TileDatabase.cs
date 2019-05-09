// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class TileDatabase : MonoBehaviour
// {
//     public static TileDatabase instance;
//     public Dictionary<string, TileWeight> tileDB;

//     private void Awake()
//     {
//         if(instance == null)
//         {
//             instance = this;
//         }
//         else
//         {
//             Destroy(this);
//         }
//         DontDestroyOnLoad(this);
//         //then add to tiledb
//         tileDB = new Dictionary<string, TileWeight>();
//         //should contain a list of all tiles in the game.
//         //probably should look into scriptable objects?
//         tileDB.Add("Grass", TileWeight.UNOBSTRUCTED);
//         tileDB.Add("landscapeTiles_064", TileWeight.LIGHT_OBS);
//     }
// }