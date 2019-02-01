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
    MapController mapController;

    public AlliedUnit alliedUnit;

    void Start()
    {
        GameObject mapControllerObject = GameObject.FindGameObjectWithTag("MapController");
        if (mapControllerObject != null)
        {
            mapController = mapControllerObject.GetComponent<MapController>();
        }
        if (mapControllerObject == null)
        {
            Debug.Log("Cannot find Tilemap object");
        }
        
        
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
            Debug.Log(mapController.GridToMap(mapController.grid.WorldToCell(hit.point)));
            if(lastTileLoc != null)
                mapController.walkableTiles.SetColor(lastTileLoc, Color.white);
            lastTileLoc = mapController.grid.WorldToCell(hit.point);
            mapController.walkableTiles.SetColor(mapController.grid.WorldToCell(hit.point), Color.red);
        }
        else
        {
            mapController.walkableTiles.SetColor(lastTileLoc, Color.white);
        }
        //End Tile highlighting code
    }

    public void SpawnUnit()
    {
        Instantiate(alliedUnit.gameObject)
    }

}

public enum HoverState
{
    NONE,
    HOVER
}
