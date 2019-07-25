using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapUIController : MonoBehaviour
{
    public static MapUIController instance;
    private bool verboseMode = false;
    
    [HideInInspector]
    public Vector2Int cursorPosition;

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

    private void OnDrawGizmos()
    {
        if (!verboseMode)
            verboseMode = true;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        cursorPosition = MapMath.WorldToMap(ray.origin * -10f/ray.origin.z);
        //Debug.Log("Cursor Position(World Pos): " + ray.origin);
        Debug.Log("Cursor Position(Grid Pos): " + MapMath.GridToMap(MapMath.MapToGrid(cursorPosition)));
        Debug.Log("Cursor Position(Map Pos): " + cursorPosition);
        tileSelector.transform.position = MapMath.MapToWorld(cursorPosition);    

        // if (hit)
        // {
        //     hover_state = HoverState.HOVER;
        // }
        // else
        // {
        //     hover_state = HoverState.NONE;
        // }

        // if (hover_state == HoverState.HOVER)
        // {
        //     //Mouse is hovering
        //     //Debug.Log(mapController.GridToMap(mapController.grid.WorldToCell(hit.point)));
        //     //Debug.Log(MapController.instance.grid.CellToWorld(MapController.instance.grid.WorldToCell(hit.point)));
        //     if (lastTileLoc != null)
        //         tileSelectorMap.SetTile(lastTileLoc, null);
        //     lastTileLoc = MapController.instance.grid.WorldToCell(hit.point);
        //     tilePointer = MapController.instance.grid.WorldToCell(hit.point);
        // }
        // else
        // {
        //     tileSelectorMap.SetTile(lastTileLoc, null);
        // }
        //End Tile highlighting code
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
