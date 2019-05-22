using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapUIController : MonoBehaviour
{
    public static MapUIController instance;
    
    [HideInInspector]
    public Vector2Int cursorPosition;

    public Tilemap tileHighlighting;
    public Tilemap pathHighlighting;
    
    [SerializeField]
    private GameObject tileSelector;
    [SerializeField]
    private TileBase movementTile;
    [SerializeField]
    private TileBase attackTile;

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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        cursorPosition = MapMath.WorldToMap(ray.origin * -10f/ray.origin.z);
        tileSelector.transform.position = MapMath.MapToWorld(cursorPosition); // ay lmao     

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
        pathHighlighting.ClearAllTiles();
    }

    public void RangeHighlight(Vector2Int mapPos)
    {
        tileHighlighting.SetTile(MapMath.MapToGrid(mapPos), MapUIController.instance.movementTile);
    }

    public void PathHighlight(Vector2Int mapPos)
    {
        pathHighlighting.SetTile(MapMath.MapToGrid(mapPos), MapUIController.instance.attackTile);
    }

    public void PathHighlight(Vector2Int a, Vector2Int b)
    {
        for (int x = Mathf.Min(a.x, b.x); x <= Mathf.Max(a.x, b.x); x++)
        {
            for (int y = Mathf.Min(a.y, b.y); y <= Mathf.Max(a.y, b.y); y++)
            {
                PathHighlight(new Vector2Int(x, y));
            }
        }
    }
}
