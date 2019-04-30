using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Unit : MonoBehaviour
{
    public bool hasAttacked;
    public bool hasMoved;
    public int moveSpeed;
    public bool isRanged;
    public Direction direction;
    protected Vector2Int mapPosition;

    protected int health;

    protected TileWeight tile;

    private void Awake()
    {
        direction = Direction.N;
        hasAttacked = false;
        hasMoved = false;
    }

    private void Start()
    {
        mapPosition = MapMath.WorldToMap(this.transform.position);
        tile = (TileWeight)MapController.instance.map[mapPosition.x, mapPosition.y];
        MapController.instance.map[mapPosition.x, mapPosition.y] = (int)TileWeight.OBSTRUCTED;
        //Debug.Log(mapPosition.x);
        //Debug.Log(mapPosition.y);
    }

    public abstract void DisplayMovementTiles();

    public Dictionary<Vector2Int, Direction> FindMoveableTiles(int[,] map)
    {
        Dictionary<Vector2Int, Direction> shortestFrom = new Dictionary<Vector2Int, Direction>();
        Dictionary<Vector2Int, int> movementCost = new Dictionary<Vector2Int, int>();

        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        PriorityQueue<Vector2Int> frontier = new PriorityQueue<Vector2Int>();
        frontier.Enqueue(mapPosition, 0); // Should only contain tiles in range
        movementCost[mapPosition] = 0; // Contains frontier and visited
        shortestFrom[mapPosition] = Direction.NO_DIR;

        while (frontier.Count != 0)
        {
            Vector2Int visiting = frontier.Dequeue();
            if (visited.Contains(visiting)) {continue;} // TODO: Implement changing priority in the PQ, and remove this.
            
            Dictionary<Vector2Int, Direction> neighbors = GetNeighbors(visiting);
            foreach (Vector2Int neighbor in neighbors.Keys)
            {
                if (visited.Contains(neighbor) || !MapMath.InMapBounds(neighbor)) { continue; }
                int nextDist = MapController.instance.map[neighbor.x, neighbor.y] + movementCost[visiting];
                if (nextDist > moveSpeed) { continue; }
                if (!movementCost.ContainsKey(neighbor) || nextDist < movementCost[neighbor])
                {
                    frontier.Enqueue(neighbor, nextDist);
                    movementCost[neighbor] = nextDist;
                    shortestFrom[neighbor] = neighbors[neighbor];
                }
            }

            visited.Add(visiting);
        }

        return shortestFrom;
    }
    public Stack<Vector2Int> GetMovementPath(Dictionary<Vector2Int, Direction> possibleMoveLocs, Vector2Int dest)
    {
        Stack<Vector2Int> path = new Stack<Vector2Int>();
        if (!possibleMoveLocs.ContainsKey(dest))
        {
            return null;
        }
        Vector2Int currLoc = dest;
        while(currLoc != mapPosition)
        {
            path.Push(currLoc);
            Direction dir = MapMath.GetOppositeDirection(possibleMoveLocs[currLoc]);
            switch (dir)
            {
                case Direction.N:
                    currLoc = currLoc + MapMath.RelativeNorth;
                    break;
                case Direction.S:
                    currLoc = currLoc + MapMath.RelativeSouth;
                    break;
                case Direction.E:
                    currLoc = currLoc + MapMath.RelativeEast;
                    break;
                case Direction.W:
                    currLoc = currLoc + MapMath.RelativeWest;
                    break;
            }
        }
        return path;
    }
    public Dictionary<Vector2Int, Direction> GetNeighbors(Vector2Int curr)
    {
        Dictionary<Vector2Int, Direction> neighbors = new Dictionary<Vector2Int, Direction>();
        //prevent current unit pos from being readded to neighbors
        if (!mapPosition.Equals(new Vector2Int(curr.x, curr.y + 1)))
        {
            neighbors.Add(new Vector2Int(curr.x, curr.y + 1), Direction.N);
        }
        if (!mapPosition.Equals(new Vector2Int(curr.x - 1, curr.y)))
        {
            neighbors.Add(new Vector2Int(curr.x - 1, curr.y), Direction.W);
        }
        if (!mapPosition.Equals(new Vector2Int(curr.x, curr.y - 1)))
        {
            neighbors.Add(new Vector2Int(curr.x, curr.y - 1), Direction.S);
        }
        if (!mapPosition.Equals(new Vector2Int(curr.x + 1, curr.y)))
        {
            neighbors.Add(new Vector2Int(curr.x + 1, curr.y), Direction.E);
        }
        return neighbors;
    }
    public virtual void Move(int x, int y)
    {
        //restore old tilevalue
        // MapController.instance.map[mapPosition.x, mapPosition.y] = (int)tile;
        mapPosition.x = x;
        mapPosition.y = y;
        // tile = (TileWeight)MapController.instance.map[mapPosition.x, mapPosition.y];
        // MapController.instance.map[mapPosition.x, mapPosition.y] = (int)TileWeight.OBSTRUCTED;
        this.transform.position = MapMath.MapToWorld(new Vector2Int(x, y));
        hasMoved = true;
        hasAttacked = true; // for testing only. CHANGE LATER
    }
}

