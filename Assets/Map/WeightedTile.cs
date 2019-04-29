using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class WeightedTile : Tile
{
    [SerializeField]
    public TileWeight weight;
    // WeightedTile[] neighbors = new WeightedTile[4];

    // private void doNeighborLink(WeightedTile other)
    // {
    //     this.neighbors
    // }

    // public override void RefreshTile(Vector3Int location, ITilemap tilemap)
    // {
    //     TileBase north = tilemap.GetTile(location + new Vector3Int(1, 0, 0));
    //     if (north is WeightedTile) { doNeighborLink(north as WeightedTile); }
    // }

    #if UNITY_EDITOR
    [MenuItem("Assets/Create/WeightedTile")]
    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Weighted Tile", "New Weighted Tile", "Asset", "Save Weighted Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WeightedTile>(), path);
    }
    #endif
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