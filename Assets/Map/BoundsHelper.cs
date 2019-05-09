using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class BoundsHelper : MonoBehaviour
{
    public TileBase previewTile;
    private Tilemap tilemap;
    private MapController parent;

    void Awake()
    {
        tilemap = this.GetComponent<Tilemap>();
        parent = this.GetComponentInParent<MapController>();
 
        Debug.Log("EYYYYY LMAO");
        tilemap.ClearAllTiles();
        // tilemap.BoxFill(Vector3Int.zero, previewTile, 3, 3, -3, -3);
        for (int x = parent.mapWidthOffset; x < parent.mapWidth + parent.mapWidthOffset; x++)
        {
            for (int y = parent.mapHeightOffset - parent.mapHeight + 1; y <= parent.mapHeightOffset; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), previewTile);
            }
        }
        
        // tilemap.BoxFill(Vector3Int.zero, previewTile, parent.mapWidthOffset, parent.mapHeightOffset - parent.mapHeight, parent.mapWidth + parent.mapWidthOffset, parent.mapHeightOffset);
        // Debug.Log(new Vector4(parent.mapWidthOffset, parent.mapHeightOffset - parent.mapHeight, parent.mapWidth + parent.mapWidthOffset, parent.mapHeightOffset));
        // tilemap.ResizeBounds();
    }
}