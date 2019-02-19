using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CommanderController : MonoBehaviour
{

    public const int MAX_TEAM_SIZE = 3;

    public AlliedUnit alliedUnitPrefab;

    public AlliedUnit[] alliedUnits;

    public int activeUnit;

    void Start()
    {
        alliedUnits = new AlliedUnit[MAX_TEAM_SIZE];
        alliedUnits[0] = SpawnUnit(0,0);
        alliedUnits[1] = SpawnUnit(5, 5);
        alliedUnits[2] = SpawnUnit(2, 2);
        activeUnit = 0;
    }
    void Update()
    {
        //check for tab input
        //select next unit
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MapUIController.instance.tileHighlighting.ClearAllTiles();
            MapUIController.instance.pathHighlighting.ClearAllTiles();
            GetNextUnit();
        }
        if (Input.GetMouseButtonDown(0))
        {
            //Start Tile highlighting code
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100f);
            Vector2Int dest = MapMath.GridToMap(MapController.instance.grid.WorldToCell(hit.point));
            alliedUnits[activeUnit].DisplayPath(dest);
        }
    }

    //spawn unit at x,y in map units
    public AlliedUnit SpawnUnit(int x, int y)
    {
        Vector3 playerPos = MapMath.MapToWorld(x, y);
        return Instantiate(alliedUnitPrefab.gameObject, playerPos, Quaternion.identity).GetComponent<AlliedUnit>();  
    }

    private void GetNextUnit()
    {
        if (alliedUnits.Length != 0)
        {
            int i = activeUnit + 1;
            int n = i % MAX_TEAM_SIZE;
            bool unitFound = false;
            while (!unitFound)
            {
                if (alliedUnits[i % MAX_TEAM_SIZE] != null && alliedUnits[i % MAX_TEAM_SIZE].gameObject.activeInHierarchy)
                {
                    //Debug.Log("Switch");
                    activeUnit = i % MAX_TEAM_SIZE;
                    alliedUnits[activeUnit].DisplayMovementTiles();
                    unitFound = true;
                }
                i++;
                if(alliedUnits[i % MAX_TEAM_SIZE] == null && i % MAX_TEAM_SIZE == n)
                {
                    return;
                }
            }
        }
    }

}

public enum HoverState
{
    NONE,
    HOVER
}
