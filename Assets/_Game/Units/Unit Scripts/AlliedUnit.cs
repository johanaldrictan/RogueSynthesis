using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlliedUnit : Unit
{
    [System.NonSerialized] public List<Vector3Int> plannedPath;

    public override void Awake()
    {
        hasAttacked = false;
        hasMoved = false;
        plannedPath = new List<Vector3Int>();
        m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
        m_SpriteRenderer.sortingOrder = 99;
    }
    public override void DisplayMovementTiles(Direction targetFacing)
    {
        //clear tilemap
        MapUIController.instance.tileHighlighting.ClearAllTiles();
        if (!hasAttacked)
        {
            Dictionary<Vector3Int, Vector3Int>.KeyCollection moveLocs = FindMoveableWithFacing(MapController.instance.map).Keys;
            foreach (Vector3Int loc in moveLocs)
            {
                if (loc.z != (int)targetFacing) {continue;}
                MapUIController.instance.tileHighlighting.SetTile(MapMath.MapToGrid(loc.x, loc.y), MapUIController.instance.movementTile);
            }
        }
    }
    public void DisplayPath(Vector2Int dest, Direction targetFacing)
    {
        MapUIController.instance.pathHighlighting.ClearAllTiles();
        Stack<Vector3Int> path = GetMovementPath(FindMoveableWithFacing(MapController.instance.map), new Vector3Int(dest.x, dest.y, (int)targetFacing));
        plannedPath.Clear();

        if (path == null) { return; }
        
        plannedPath.AddRange(path);
        while(path.Count != 0)
        {
            Vector3Int loc = path.Pop();
            print((Direction)loc.z);
            MapUIController.instance.pathHighlighting.SetTile(MapMath.MapToGrid(loc.x, loc.y), MapUIController.instance.attackTile);
        }
    }

    public override void Move(int x, int y, Direction targetFacing)
    {
        // base.Move(x, y, targetFacing);
        MapUIController.instance.pathHighlighting.ClearAllTiles();
        MapUIController.instance.tileHighlighting.ClearAllTiles();
        //restore old tilevalue
        MapController.instance.map[mapPosition.x, mapPosition.y] = (int)tile;
        mapPosition.x = x;
        mapPosition.y = y;
        this.changeDirection(targetFacing);

        tile = (TileWeight)MapController.instance.map[mapPosition.x, mapPosition.y];
        MapController.instance.map[mapPosition.x, mapPosition.y] = (int)TileWeight.OBSTRUCTED;
        this.transform.position = MapMath.MapToWorld(x,y);
        plannedPath.Clear();
        hasMoved = true;
        hasAttacked = true; // for testing only. CHANGE LATER
    }
}
