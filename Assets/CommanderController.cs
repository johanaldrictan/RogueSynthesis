using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CommanderController : MonoBehaviour
{

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
