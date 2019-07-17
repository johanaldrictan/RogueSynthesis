using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Unit : MonoBehaviour
{
    // Unit's core RPG stats
    [SerializeField] protected string unitName;
    [SerializeField] protected int health;
    [SerializeField] protected int moveSpeed;

    // booleans
    [SerializeField] public bool hasActed;
    [SerializeField] public bool hasMoved;

    // positional data
    [SerializeField] protected Direction direction;
    [SerializeField] protected Vector2Int mapPosition;
    [System.NonSerialized] public UnitPositionStorage globalPositionalData;

    public SpriteSet sprites;
    protected SpriteRenderer m_SpriteRenderer;

    protected TileWeight tile;

    // storage of the unit's UnitData object
    // given on instantiation by a UnitController
    // used to initialize the Unit, kept for future reference (if needed)
    [System.NonSerialized] public UnitData unitData;

    // the set of abilities that this unit can use on its turn
    [System.NonSerialized] public List<UnitAbility> availableAbilities;


    

    public virtual void Awake()
    {
        hasActed = false;
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

    public abstract void DisplayMovementTiles();

    // parses unitData to set own variables
    public void loadData()
    {
        // set parameters
        unitName = unitData.unitName;
        sprites = unitData.sprites;
        health = unitData.health;
        moveSpeed = unitData.moveSpeed;

        // convert the unitData's list of ability enums into real abilities, and store them
        availableAbilities = AbilityDatabase.GetAbilities(unitData.abilities);

        /*
        for (int i = 0; i < availableAbilities.Count; i++)
        {
            Debug.Log(availableAbilities[i]);
        }
        */

        // set the direction to itself (in order to set the sprite)
        ChangeDirection(direction);
    }


    // a Unit needs to be able to choose its Ability after it has moved but before its turn has ended
    public abstract void chooseAbility();


    public Dictionary<Vector2Int, Direction> FindMoveableTiles(int[,] map, int moveSpeed = -100)
    {
        if (moveSpeed == -100) { moveSpeed = this.moveSpeed; }
        return Unit.FindMoveableTiles(map, this.mapPosition, moveSpeed);
    }

    public static Dictionary<Vector2Int, Direction> FindMoveableTilesStraight(int[,] map, Vector2Int mapPosition, int moveSpeed)
    {
        Dictionary<Vector2Int, Direction> shortestFrom = new Dictionary<Vector2Int, Direction>();
        Dictionary<Vector2Int, int> movementCost = new Dictionary<Vector2Int, int>();
        
        Stack<Vector2Int> frontier = new Stack<Vector2Int>();
        frontier.Push(mapPosition); // Should only contain tiles in range
        movementCost[mapPosition] = 0; // Contains frontier and visited
        shortestFrom[mapPosition] = Direction.NO_DIR;

        while (frontier.Count != 0)
        {
            Vector2Int visiting = frontier.Pop();

            Dictionary<Vector2Int, Direction> neighbors = GetNeighbors(visiting);
            foreach (Vector2Int neighbor in neighbors.Keys)
            {
                if (shortestFrom[visiting] != Direction.NO_DIR && neighbors[neighbor] != shortestFrom[visiting]) { continue; }
                if (!MapMath.InMapBounds(neighbor)) { continue; }
                int nextDist = MapController.instance.map[neighbor.x, neighbor.y] + movementCost[visiting];
                if (nextDist > moveSpeed) { continue; }
                frontier.Push(neighbor);
                movementCost[neighbor] = nextDist;
                shortestFrom[neighbor] = neighbors[neighbor];
            }
        }

        return shortestFrom;
    }

    public static Dictionary<Vector2Int, Direction> FindMoveableTiles(int[,] map, Vector2Int mapPosition, int moveSpeed)
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

    public static Dictionary<Vector2Int, Direction> GetNeighbors(Vector2Int curr)
    {
        Dictionary<Vector2Int, Direction> neighbors = new Dictionary<Vector2Int, Direction>();
        neighbors.Add(new Vector2Int(curr.x, curr.y + 1), Direction.N);
        neighbors.Add(new Vector2Int(curr.x - 1, curr.y), Direction.W);
        neighbors.Add(new Vector2Int(curr.x, curr.y - 1), Direction.S);
        neighbors.Add(new Vector2Int(curr.x + 1, curr.y), Direction.E);
        return neighbors;
    }

    public virtual void Move(int x, int y)
    {
        // remove old coordinates from globalPositionalData
        globalPositionalData.RemoveUnit(mapPosition);

        // restore old tilevalue
        // MapController.instance.map[mapPosition.x, mapPosition.y] = (int)tile;

        // set new coordinates
        mapPosition.x = x;
        mapPosition.y = y;

        // update the globalPositionalData
        globalPositionalData.AddUnit(mapPosition, this);

        // tile = (TileWeight)MapController.instance.map[mapPosition.x, mapPosition.y];
        // MapController.instance.map[mapPosition.x, mapPosition.y] = (int)TileWeight.OBSTRUCTED;
        this.transform.position = MapMath.MapToWorld(new Vector2Int(x, y));
        hasMoved = true;
    }

    public void ChangeDirection(Direction newDirection)
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


    


    // HELPER FUNCTIONS

    public string GetName()
    { return unitName; }

    public void SetName(string newName)
    { unitName = newName; }

    public int GetHealth()
    { return health; }

    public void ChangeHealth(int amount)
    { health += amount; }

    public int GetMoveSpeed()
    { return moveSpeed; }

    public void ChangeMoveSpeed(int amount)
    { moveSpeed += amount; }

    public Direction GetDirection()
    { return direction; }

    public void SetDirection(Direction newDirection)
    { direction = newDirection; }

    public Vector2Int GetMapPosition()
    { return mapPosition; }

}

