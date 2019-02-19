using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Priority_Queue;

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

    public Dictionary<Vector2Int, Direction> FindMoveableTiles(int[,] map)
    {
        SimplePriorityQueue<Vector2Int> locsToVisit = new SimplePriorityQueue<Vector2Int>();
        //int is a direction flag going to the minimal path
        Dictionary<Vector2Int, Direction> possibleMoveLocs = new Dictionary<Vector2Int, Direction>();
        Dictionary<Vector2Int, int> visitedLocs = new Dictionary<Vector2Int, int>();

        //Add player loc to the queue
        locsToVisit.Enqueue(mapPosition,0);
        visitedLocs.Add(mapPosition, 0);
        possibleMoveLocs.Add(mapPosition, Direction.NO_DIR);
        while (locsToVisit.Count != 0)
        {
            Vector2Int currLoc = locsToVisit.Dequeue();
            //dont process neighbors if you reach the edge
            if(visitedLocs[currLoc] >= moveSpeed)
            {
                continue;
            }
            //get tile neighbors and add them if they havent been visited yet
            Dictionary<Vector2Int, Direction> neighbors = GetNeighbors(currLoc);
            foreach (Vector2Int next in neighbors.Keys)
            {
                if (MapMath.InMapBounds(next))
                {
                    //check cost
                    int currDist = currLoc != mapPosition ? map[next.x, next.y] + visitedLocs[currLoc] : 1;
                    if(!visitedLocs.ContainsKey(next) || currDist < visitedLocs[next])
                    {
                        visitedLocs.Add(next, currDist);
                        locsToVisit.Enqueue(next, currDist);
                        possibleMoveLocs.Add(next, neighbors[next]);
                    }
                }
            }
        }
        return possibleMoveLocs;
    }
    public List<Vector2Int> GetTilePath(Dictionary<Vector2Int, int> possibleMoveLocs, Vector2Int dest)
    {
        List<Vector2Int> tilePath = new List<Vector2Int>();
        //if for some reason destination is not in the location list return empty
        if (!possibleMoveLocs.ContainsKey(dest))
        {
            return null;
        }
        else
        {
            
        }
        return tilePath;
    }
    public Dictionary<Vector2Int, Direction> GetNeighbors(Vector2Int curr)
    {
        Dictionary<Vector2Int, Direction> neighbors = new Dictionary<Vector2Int, Direction>();
        neighbors.Add(new Vector2Int(curr.x, curr.y+1), Direction.N);
        neighbors.Add(new Vector2Int(curr.x+1, curr.y), Direction.W);
        neighbors.Add(new Vector2Int(curr.x, curr.y-1), Direction.S);
        neighbors.Add(new Vector2Int(curr.x-1, curr.y), Direction.E);
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
    N,
    W,
    E,
    S,
    NO_DIR
}
