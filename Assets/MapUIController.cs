using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapUIController : MonoBehaviour
{
    public static MapUIController instance;
    Ray ray;
    RaycastHit2D hit;
    HoverState hover_state;
    Vector3Int lastTileLoc;

    public Vector3Int tilePointer;

    public Tilemap tileHighlighting;
    public Tilemap tileSelectorMap;

    public TileBase tileSelector;
    public TileBase movementTile;

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
        //Start Tile highlighting code
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hit = Physics2D.Raycast(ray.origin, ray.direction, 100f);
        if (hit)
        {
            hover_state = HoverState.HOVER;
        }
        else
        {
            hover_state = HoverState.NONE;
        }

        if (hover_state == HoverState.HOVER)
        {
            //Mouse is hovering
            //Debug.Log(mapController.GridToMap(mapController.grid.WorldToCell(hit.point)));
            //Debug.Log(MapController.instance.grid.CellToWorld(MapController.instance.grid.WorldToCell(hit.point)));
            if (lastTileLoc != null)
                tileSelectorMap.SetTile(lastTileLoc, null);
            lastTileLoc = MapController.instance.grid.WorldToCell(hit.point);
            tileSelectorMap.SetTile(MapController.instance.grid.WorldToCell(hit.point), tileSelector);
            tilePointer = MapController.instance.grid.WorldToCell(hit.point);
        }
        else
        {
            tileSelectorMap.SetTile(lastTileLoc, null);
        }
        //End Tile highlighting code
    }
    public void ResetMovementTilemap()
    {
        
    }
}
