﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapUIController : MonoBehaviour
{
    public static MapUIController instance;
    
    [HideInInspector]
    public Vector2Int cursorPosition;

    //private caching to save on garbage collection passes
    [HideInInspector]
    Ray ray;

    public Tilemap tileHighlighting;
    public Tilemap pathHighlightingNS;
    public Tilemap pathHighlightingEW;

    [SerializeField]
    private GameObject tileSelector;
    [SerializeField]
    private TileBase movementTile;
    [SerializeField]
    private TileBase attackTile;
    [SerializeField]
    private TileBase pathingTileNS;
    [SerializeField]
    private TileBase pathingTileEW;

    private void Awake()
    {
        //there should only be one mapuicontroller
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        cursorPosition = MapMath.WorldToMap(ray.origin * -10f/ray.origin.z);
        //Debug.Log("Cursor Position(World Pos): " + ray.origin);
        //Debug.Log("Cursor Position(Grid Pos): " + MapController.instance.walkableTiles.WorldToCell(ray.origin));
        Debug.Log("Cursor Position(Map Pos): " + cursorPosition);
        //Debug.Log("Cursor Position(Grid Pos 2): " + MapMath.MapToGrid(cursorPosition));
        tileSelector.transform.position = MapMath.MapToWorld(cursorPosition);    

    }

    public void ClearRangeHighlight()
    {
        tileHighlighting.ClearAllTiles();
    }

    public void ClearPathHighlight()
    {
        pathHighlightingNS.ClearAllTiles();
        pathHighlightingEW.ClearAllTiles();
    }

    public void RangeHighlight(Vector2Int mapPos)
    {
        tileHighlighting.SetTile(MapMath.MapToGrid(mapPos), MapUIController.instance.movementTile);
    }

    public void PathHighlight(Vector2Int mapPos, bool northSouth)
    {
        if (northSouth)
        {
            pathHighlightingNS.SetTile(MapMath.MapToGrid(mapPos), MapUIController.instance.pathingTileNS);
        }
        else
        {
            pathHighlightingEW.SetTile(MapMath.MapToGrid(mapPos), MapUIController.instance.pathingTileEW);
        }
    }

    public void PathHighlight(Vector2Int a, Vector2Int diff)
    {
        //for (int x = Mathf.Min(a.x, b.x); x <= Mathf.Max(a.x, b.x); x++)
        //{
        //    for (int y = Mathf.Min(a.y, b.y); y <= Mathf.Max(a.y, b.y); y++)
        //    {
        //        PathHighlight(new Vector2Int(x, y));
        //    }
        //}
    }
}
