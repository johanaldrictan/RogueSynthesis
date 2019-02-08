using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CommanderController : MonoBehaviour
{
    Ray ray;
    RaycastHit2D hit;
    HoverState hover_state;
    Vector3Int lastTileLoc;
    public const int MAX_TEAM_SIZE = 3;

    public AlliedUnit alliedUnitPrefab;

    public AlliedUnit[] alliedUnits;

    void Start()
    {
        alliedUnits = new AlliedUnit[MAX_TEAM_SIZE];
        alliedUnits[0] = SpawnUnit(0,0);
    }
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
            //Debug.Log(MapController.instance.grid.WorldToCell(hit.point));
            if (lastTileLoc != null)
                MapController.instance.walkableTiles.SetColor(lastTileLoc, Color.white);
            lastTileLoc = MapController.instance.grid.WorldToCell(hit.point);
            MapController.instance.walkableTiles.SetColor(MapController.instance.grid.WorldToCell(hit.point), Color.red);
        }
        else
        {
            MapController.instance.walkableTiles.SetColor(lastTileLoc, Color.white);
        }
        //End Tile highlighting code
    }

    //spawn unit at x,y in map units
    public AlliedUnit SpawnUnit(int x, int y)
    {
        Vector3 playerPos = MapController.instance.grid.CellToWorld(MapMath.MapToGrid(x,y));
        return Instantiate(alliedUnitPrefab.gameObject).GetComponent<AlliedUnit>();       
    }

}

public enum HoverState
{
    NONE,
    HOVER
}
