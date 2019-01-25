using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CommanderController : MonoBehaviour
{
    Ray ray;
    RaycastHit2D hit;
    Grid grid;
    Tilemap walkableTiles;
    HoverState hover_state;
    Vector3Int lastTileLoc;

    void Start()
    {
        GameObject tilemapObject = GameObject.FindGameObjectWithTag("Walkable");
        if (tilemapObject != null)
        {
            walkableTiles = tilemapObject.GetComponent<Tilemap>();
        }
        if (tilemapObject == null)
        {
            Debug.Log("Cannot find Tilemap object");
        }
        grid = walkableTiles.GetComponentInParent<Grid>();
    }
    void Update()
    {
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
            Debug.Log(grid.WorldToCell(hit.point));
            if(lastTileLoc != null)
                walkableTiles.SetColor(lastTileLoc, Color.white);
            lastTileLoc = grid.WorldToCell(hit.point);
            walkableTiles.SetColor(grid.WorldToCell(hit.point), Color.red);
        }
        else
        {
            walkableTiles.SetColor(lastTileLoc, Color.white);
        }
    }

}

public enum HoverState
{
    NONE,
    HOVER
}
