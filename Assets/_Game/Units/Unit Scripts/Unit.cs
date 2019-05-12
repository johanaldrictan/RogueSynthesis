using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Unit : MonoBehaviour
{
    public string unitName;
    public int health;
    public int attack;
    public int moveSpeed;

    public bool hasAttacked;
    public bool hasMoved;
    
    public Direction direction;
    protected Vector2Int mapPosition;
    public SpriteSet sprites;
    protected SpriteRenderer m_SpriteRenderer;


    protected TileWeight tile;

    // storage of the unit's UnitData object
    // given on instantiation by a UnitController
    // used to initialize the Unit, kept for future reference (if needed)
    [System.NonSerialized] public UnitData unitData;

    public virtual void Awake()
    {
        hasAttacked = false;
        hasMoved = false;
        m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
        m_SpriteRenderer.sortingOrder = 99;
    }

    public virtual void Start()
    {
        mapPosition = MapMath.WorldToMap(this.transform.position);
        tile = (TileWeight)MapController.instance.map[mapPosition.x, mapPosition.y];
        MapController.instance.map[mapPosition.x, mapPosition.y] = (int)TileWeight.OBSTRUCTED;
        //Debug.Log(mapPosition.x);
        //Debug.Log(mapPosition.y);
    }

    public abstract void DisplayMovementTiles(Direction targetFacing);

    // parses unitData to set own variables
    public void loadData()
    {
        // set parameters
        unitName = unitData.unitName;
        sprites = unitData.sprites;
        health = unitData.health;
        attack = unitData.attack;
        moveSpeed = unitData.moveSpeed;

        // set the direction to itself (in order to set the sprite)
        changeDirection(direction);
    }

    // public Dictionary<Vector2Int, Direction> FindMoveableTiles(int[,] map)
    // {
    //     Dictionary<Vector2Int, Direction> shortestFrom = new Dictionary<Vector2Int, Direction>();
    //     Dictionary<Vector2Int, int> movementCost = new Dictionary<Vector2Int, int>();

    //     HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
    //     PriorityQueue<Vector2Int> frontier = new PriorityQueue<Vector2Int>();
    //     frontier.Enqueue(mapPosition, 0); // Should only contain tiles in range
    //     movementCost[mapPosition] = 0; // Contains frontier and visited
    //     shortestFrom[mapPosition] = Direction.NO_DIR;

    //     while (frontier.Count != 0)
    //     {
    //         Vector2Int visiting = frontier.Dequeue();
    //         if (visited.Contains(visiting)) {continue;} // TODO: Implement changing priority in the PQ, and remove this.
            
    //         HashSet<Vector3Int> neighbors = GetNeighbors(visiting.x, visiting.y);
    //         foreach (Vector2Int neighbor in neighbors)
    //         {
    //             if (visited.Contains(neighbor) || !MapMath.InMapBounds(neighbor)) { continue; }
    //             int nextDist = MapController.instance.map[neighbor.x, neighbor.y] + movementCost[visiting];
    //             if (nextDist > moveSpeed) { continue; }
    //             if (!movementCost.ContainsKey(neighbor) || nextDist < movementCost[neighbor])
    //             {
    //                 frontier.Enqueue(neighbor, nextDist);
    //                 movementCost[neighbor] = nextDist;
    //                 shortestFrom[neighbor] = (Direction)neighbor.y;
    //             }
    //         }

    //         visited.Add(visiting);
    //     }

    //     return shortestFrom;
    // }

    public Dictionary<Vector3Int, Vector3Int> FindMoveableWithFacing(int[,] map)
    {
        // to future contributors: im sorry
        Dictionary<Vector3Int, Vector3Int> shortestFrom = new Dictionary<Vector3Int, Vector3Int>();
        Dictionary<Vector3Int, int> movementCost = new Dictionary<Vector3Int, int>();
        Vector3Int mapPosWithFacing = new Vector3Int(mapPosition.x, mapPosition.y, (int)direction);

        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
        PriorityQueue<Vector3Int> frontier = new PriorityQueue<Vector3Int>();
        frontier.Enqueue(mapPosWithFacing, 0); // Should only contain tiles in range
        movementCost[mapPosWithFacing] = 0; // Contains frontier and visited
        shortestFrom[mapPosWithFacing] = mapPosWithFacing;

        while (frontier.Count != 0)
        {
            Vector3Int visiting = frontier.Dequeue();
            if (visited.Contains(visiting)) {continue;} // TODO: Implement changing priority in the PQ, and remove this.
            
            HashSet<Vector3Int> neighbors = GetNeighbors(visiting.x, visiting.y);
            foreach (Vector3Int neighbor in neighbors)
            {
                if (visiting.z == (int)MapMath.GetOppositeDirection((Direction)neighbor.z)) {continue;} // cant backtrack
                if (visited.Contains(neighbor) || !MapMath.InMapBounds(neighbor.x, neighbor.y)) { continue; }
                int nextDist = MapController.instance.map[neighbor.x, neighbor.y] + movementCost[visiting];
                if (nextDist > moveSpeed) { continue; }
                if (!movementCost.ContainsKey(neighbor) || nextDist < movementCost[neighbor])
                {
                    frontier.Enqueue(neighbor, nextDist);
                    movementCost[neighbor] = nextDist;
                    shortestFrom[neighbor] = visiting;
                }
            }

            visited.Add(visiting);
        }

        return shortestFrom;
    }


    public Stack<Vector3Int> GetMovementPath(Dictionary<Vector3Int, Vector3Int> possibleMoveLocs, Vector2Int dest, Direction targetDir)
    {
        return GetMovementPath(possibleMoveLocs, new Vector3Int(dest.x, dest.y, (int)targetDir));
    }

    public Stack<Vector3Int> GetMovementPath(Dictionary<Vector3Int, Vector3Int> possibleMoveLocs, Vector3Int dest)
    {
        Stack<Vector3Int> path = new Stack<Vector3Int>();
        if (!possibleMoveLocs.ContainsKey(dest))
        {
            return null;
        }
        Vector3Int currLoc = dest;
        while (currLoc != possibleMoveLocs[currLoc])
        {
            path.Push(currLoc);
            currLoc = possibleMoveLocs[currLoc];
        }
        return path;
    }

    public HashSet<Vector3Int> GetNeighbors(int x, int y)
    {
        HashSet<Vector3Int> neighbors = new HashSet<Vector3Int>();
        neighbors.Add(new Vector3Int(x, y - 1, (int)Direction.N));
        neighbors.Add(new Vector3Int(x - 1, y, (int)Direction.W));
        neighbors.Add(new Vector3Int(x, y + 1, (int)Direction.S));
        neighbors.Add(new Vector3Int(x + 1, y, (int)Direction.E));
        return neighbors;
    }

    public virtual void Move(int x, int y, Direction targetFacing)
    {
        //restore old tilevalue
        // MapController.instance.map[mapPosition.x, mapPosition.y] = (int)tile;
        mapPosition.x = x;
        mapPosition.y = y;
        changeDirection(targetFacing);                                 
        // tile = (TileWeight)MapController.instance.map[mapPosition.x, mapPosition.y];
        // MapController.instance.map[mapPosition.x, mapPosition.y] = (int)TileWeight.OBSTRUCTED;
        this.transform.position = MapMath.MapToWorld(new Vector2Int(x, y));
        hasMoved = true;
        hasAttacked = true; // for testing only. CHANGE LATER
    }

    public void changeDirection(Direction newDirection)
    {
        direction = newDirection;

        if (newDirection == Direction.N)
        { m_SpriteRenderer.sprite = sprites.north; }
        else if (newDirection == Direction.S)
        { m_SpriteRenderer.sprite = sprites.south; }
        else if (newDirection == Direction.E)
        { m_SpriteRenderer.sprite = sprites.east; }
        else if (newDirection == Direction.W)
        { m_SpriteRenderer.sprite = sprites.west; }
    }
}

