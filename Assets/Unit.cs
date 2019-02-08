using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public bool isAllied;
    public bool isSelected;
    public int moveSpeed;
    private Direction direction;
    private Vector2Int mapPosition;

    private void Start()
    {
        isSelected = false;
        mapPosition = MapMath.WorldToMap(this.transform.position);
        //Debug.Log(mapPosition.x);
        //Debug.Log(mapPosition.y);
    }

    public abstract void OnMouseDown();

    public List<Vector2Int> FindMoveableTiles(int[,] map)
    {
        //Djikstra's algorithm???
        List<Vector2Int> possibleMoveLocs = new List<Vector2Int>();

        return possibleMoveLocs;
    }
    public void Move(int x, int y)
    {
        mapPosition.x = x;
        mapPosition.y = y;
        MapController.instance.map[mapPosition.x, mapPosition.y] = (int)TileWeight.OBSTRUCTED;
        this.transform.position = MapController.instance.grid.CellToWorld(MapMath.MapToGrid(mapPosition));
    }
}
public enum Direction
{
    NW,
    NE,
    SW,
    SE
}
