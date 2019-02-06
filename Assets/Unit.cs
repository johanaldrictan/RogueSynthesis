using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public bool isAllied;
    public bool isSelected;
    public int moveSpeed;
    private Direction direction;
    private Vector2 position;
    private MapController mapController;

    private void Start()
    {
        GameObject mapControllerObject = GameObject.FindGameObjectWithTag("MapController");
        if (mapControllerObject != null)
        {
            mapController = mapControllerObject.GetComponent<MapController>();
        }
        else
        {
            Debug.Log("Cannot find MapController object");
        }
        isSelected = false;
    }

    public abstract void OnMouseDown();

    public List<Vector2Int> FindMoveableTiles(int[,] map)
    {
        //Djikstra's algorithm???
        List<Vector2Int> possibleMoveLocs = new List<Vector2Int>();

        return possibleMoveLocs;
    }
}
public enum Direction
{
    NW,
    NE,
    SW,
    SE
}
