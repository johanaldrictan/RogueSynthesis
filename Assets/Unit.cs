using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Unit : MonoBehaviour
{
    public bool isAllied;
    public bool hasAttacked;
    public int moveSpeed;
    private Direction direction;
    private Vector2Int mapPosition;

    private void Start()
    {
        hasAttacked = false;
        mapPosition = MapMath.WorldToMap(this.transform.position);
        MapController.instance.map[mapPosition.x, mapPosition.y] = (int)TileWeight.OBSTRUCTED;
        //Debug.Log(mapPosition.x);
        //Debug.Log(mapPosition.y);
    }

    public abstract void DisplayMovementTiles();

    public Dictionary<Vector2Int, int> FindMoveableTiles(int[,] map)
    {
        //Djikstra's algorithm???
        Dictionary<Vector2Int, int> possibleMoveLocs = new Dictionary<Vector2Int, int>();
        Queue<Vector2Int> locsToVisit = new Queue<Vector2Int>();
        //dictionary of locs that
        Dictionary<Vector2Int, int> visitedLocs = new Dictionary<Vector2Int, int>();
        

        //Add player loc to the queue
        locsToVisit.Enqueue(mapPosition);
        visitedLocs.Add(mapPosition, 0);
        possibleMoveLocs.Add(mapPosition, 0);
        while (locsToVisit.Count != 0)
        {
            Vector2Int currLoc = locsToVisit.Dequeue();            
            //get tile neighbors and add them if they havent been visited yet
            foreach (Vector2Int next in GetNeighbors(currLoc))
            {
                if (MapMath.InMapBounds(next))
                {
                    //check if neighbor has been visited yet
                    if (!visitedLocs.ContainsKey(next))
                    {
                        //Debug.Log(currLoc.ToString());
                        int currDist = currLoc != mapPosition ? map[currLoc.x, currLoc.y] + visitedLocs[currLoc] : 1;
                        //skip processing if tile cant be reached
                        if (currDist > moveSpeed)
                            continue;
                        else
                        {
                            locsToVisit.Enqueue(next);
                            visitedLocs.Add(next, currDist);
                            possibleMoveLocs.Add(next, currDist);
                        }
                    }
                }
            }
        }
        return possibleMoveLocs;
    }
    public List<Vector2Int> GetNeighbors(Vector2Int curr)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        neighbors.Add(new Vector2Int(curr.x, curr.y+1));
        neighbors.Add(new Vector2Int(curr.x+1, curr.y));
        neighbors.Add(new Vector2Int(curr.x, curr.y-1));
        neighbors.Add(new Vector2Int(curr.x-1, curr.y));
        return neighbors;
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
