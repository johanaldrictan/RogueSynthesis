using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

// This is quite literally the code from https://docs.unity3d.com/Manual/Tilemap-ScriptableTiles-Example.html.
// There's not really a better way to do it sooooo.
// (although i did have to remove the rotation and stuff.
public class NeighborMergedTile : Tile
{
    public Sprite[] m_Sprites;
    public Sprite m_Preview;
    public static int calls = 0;

    public override void RefreshTile(Vector3Int location, ITilemap tilemap)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if ((x == 0) || (y == 0))
                {
                    Vector3Int position = new Vector3Int(location.x + x, location.y + y, location.z);
                    //Debug.Log(tilemap.GetTile(position));
                    if (tilemap.GetTile(position) != null) { tilemap.RefreshTile(position); }
                }
            }
        }
    }

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        int mask = tilemap.GetTile(location + Vector3Int.up) != null ? 1 : 0;
        mask += tilemap.GetTile(location + Vector3Int.right) != null ? 2 : 0;
        mask += tilemap.GetTile(location + Vector3Int.down) != null ? 4 : 0;
        mask += tilemap.GetTile(location + Vector3Int.left) != null ? 8 : 0;
        tileData.sprite = m_Sprites[mask];
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/NeighborMergedTile")]
    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Neighbor Merged Tile", "New Neighbor Merged Tile", "Asset", "Save Neighbor Merged Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<NeighborMergedTile>(), path);
    }
#endif

}