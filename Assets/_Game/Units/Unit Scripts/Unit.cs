using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

// A Unit is the base abstract form of, well, a Unit. Every kind of Unit does what's defined here

public abstract class Unit : MonoBehaviour
{
    // General Unit Information
    Sprite portrait;

    // Unit's core RPG stats
    [SerializeField] protected string unitName;
    [SerializeField] protected int health;
    [SerializeField] protected int moveSpeed;
    [SerializeField] protected int damageReduction;

    // booleans
    [SerializeField] public bool hasActed;
    [SerializeField] public bool hasMoved;
    [SerializeField] public bool hasPivoted;
    [SerializeField] public bool attackBuffed;
    [SerializeField] public bool damageReductionBuffed;

    // conditions
    [SerializeField] protected int immobilizedDuration;
    [SerializeField] protected int disabledDuration;

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
    [System.NonSerialized] public UnitData StartData;

    // the set of abilities that this unit can use on its turn
    [System.NonSerialized] public List<UnitAbility> AvailableAbilities;

    // a Stack of death data. Every time the Unit dies, it creates a new one
    [System.NonSerialized] public Stack<DeathData> Deaths;

    // This event fires whenever a Unit dies. 
    // It passes a reference to itself so that other scripts can do what they need to do
    public static UnitUnityEvent DeathEvent = new UnitUnityEvent();

    public EventInstance moveSoundEvent;
    public EventInstance deathSoundEvent;
    public EventInstance selectSoundEvent;


    // a Unit needs to be able to choose its Ability after it has moved but before its turn has ended
    public abstract void ChooseAbility();

    public virtual void Awake()
    {
        hasActed = false;
        hasMoved = false;
        immobilizedDuration = 0;
        disabledDuration = 0;
        attackBuffed = false;
        damageReductionBuffed = false;
        m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
        m_SpriteRenderer.sortingOrder = 99;
        if (Deaths.Count == 0)
        { Deaths = new Stack<DeathData>(); }
    }

    public virtual void Start()
    {
        mapPosition = MapMath.WorldToMap(this.transform.position);
        tile = (TileWeight)MapController.instance.weightedMap[mapPosition];
        MapController.instance.weightedMap[mapPosition] = (int)TileWeight.OBSTRUCTED;
    }

    // parses unitData to set own variables
    public void LoadData()
    {
        // set parameters
        portrait = StartData.portrait;
        unitName = StartData.unitName;
        sprites = StartData.sprites;
        health = StartData.health;
        moveSpeed = StartData.moveSpeed;
        damageReduction = 0;

        // convert the unitData's list of ability enums into real abilities, and store them
        AvailableAbilities = AbilityDatabase.GetAbilities(StartData.abilities);

        // positional setup
        ChangeDirection(Direction.S);
        if(StartData.moveSoundEventName != "" || StartData.moveSoundEventName == null)
            moveSoundEvent = FMODUnity.RuntimeManager.CreateInstance(StartData.moveSoundEventName);
        if (!moveSoundEvent.isValid()) { Debug.LogWarning("Move Event for " + unitName + " invalid."); }

        if(StartData.deathSoundEventName != "" || StartData.deathSoundEventName == null)
            deathSoundEvent = FMODUnity.RuntimeManager.CreateInstance(StartData.deathSoundEventName);
        if (!deathSoundEvent.isValid()) { Debug.LogWarning("Death Event for " + unitName + " invalid."); }

        if(StartData.selectSoundEventName != "" || StartData.selectSoundEventName == null)
            selectSoundEvent = FMODUnity.RuntimeManager.CreateInstance(StartData.selectSoundEventName);
        if (!moveSoundEvent.isValid()) { Debug.LogWarning("Select Event for " + unitName + " invalid."); }
    }


    public Dictionary<Vector2Int, Direction> FindMoveableTiles(Dictionary<Vector2Int, int> weightedMap, int moveSpeed = -100)
    {
        if (moveSpeed == -100) { moveSpeed = this.moveSpeed; }
        return Unit.FindMoveableTiles(weightedMap, this.mapPosition, moveSpeed);
    }
    /*
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
        
        
    }*/

    public static Dictionary<Vector2Int, Direction> FindMoveableTiles(Dictionary<Vector2Int, int> map, Vector2Int mapPosition, int moveSpeed)
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
            
            Dictionary<Vector2Int, Direction> neighbors = MapMath.GetNeighbors(visiting);
            foreach (Vector2Int neighbor in neighbors.Keys)
            {
                if (visited.Contains(neighbor) || !MapMath.InMapBounds(neighbor)) { continue; }
                int nextDist = MapController.instance.weightedMap[neighbor] + movementCost[visiting];
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


    public IEnumerator Move(Queue<Vector2Int> path, MovementType type)
    {
        // I am moving. do not let any controllers do anything
        EventManager.instance.AddEvent("move");

        if(moveSoundEvent.isValid())
        {
            if (this is AlliedUnit)
            {
                moveSoundEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                MapUIController.instance.ClearPathHighlight();
                MapUIController.instance.ClearRangeHighlight();
                (this as AlliedUnit).plannedPath.Clear();
            }
            moveSoundEvent.start();
        }
            
        attackBuffed = false;

        Vector2Int currentTile = GetMapPosition();
        while (path.Count > 0)
        {
            // get the next tile to move to
            currentTile = path.Dequeue();

            // remove old coordinates from globalPositionalData
            globalPositionalData.RemoveUnit(mapPosition);
            // restore old tilevalue
            MapController.instance.weightedMap[mapPosition] = (int)tile;
            // set new coordinates
            mapPosition = currentTile;
            // update the globalPositionalData
            globalPositionalData.AddUnit(mapPosition, this);
            // remember the weight of the tile being occupied
            tile = (TileWeight)MapController.instance.weightedMap[mapPosition];
            // set the occupied tile to OBSTRUCTED
            MapController.instance.weightedMap[mapPosition] = (int)TileWeight.OBSTRUCTED;

            // visualize the movement made from one tile to another, based on the type of movement
            switch (type)
            {
                case MovementType.DASH:
                    this.transform.position = MapMath.MapToWorld(currentTile);
                    yield return new WaitForSecondsRealtime(0.1f);
                    break;

                case MovementType.KNOCKBACK:
                    this.transform.position = MapMath.MapToWorld(currentTile);
                    yield return new WaitForSecondsRealtime(0.1f);
                    break;

                case MovementType.TELEPORT:
                    this.transform.position = MapMath.MapToWorld(currentTile);
                    yield return new WaitForSecondsRealtime(0.1f);
                    break;

                case MovementType.WALK:
                    this.transform.position = MapMath.MapToWorld(currentTile);
                    yield return new WaitForSecondsRealtime(0.1f);
                    break;

                default:
                    this.transform.position = MapMath.MapToWorld(currentTile);
                    yield return new WaitForSecondsRealtime(0.1f);
                    break;
            }

            if (TurnController.instance != null)
            {
                TurnController.instance.trapPositionalData.CheckTraps();
            }
        }

        this.transform.position = MapMath.MapToWorld(currentTile);
        hasMoved = true;
        EventManager.instance.RemoveEvent("move");
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


    // "Kill me."
    // "Later."
    // this function essentially destroys the Unit (but not actually) by doing 3 things:
    // - hide the sprite renderer
    // - remove the Unit from the Global Positional Data
    // - reset the tile that it was occupying
    // it then calls out that it's dying, so that other scripts can do what they're supposed to
    public virtual void KillMe(DeathData data)
    {
        if(deathSoundEvent.isValid())
            deathSoundEvent.start();
        health = 0;
        Deaths.Push(data);
        hasMoved = true;
        hasActed = true;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        globalPositionalData.RemoveUnit(mapPosition);
        MapController.instance.weightedMap[mapPosition] = (int)tile;

        // "Hey guys, I'm dying. If anyone needs a reference to me, here it is."
        DeathEvent.Invoke(this);
    }

    // This takes a 'dead' unit and gets it back in the world
    // refreshes stats, health to full, etc
    // NOT DONE
    public virtual void Revive(Vector2Int position)
    {
        //if (globalPositionalData.SearchLocation(position) != null || )
        health = StartData.health;
        hasMoved = false;
        hasActed = false;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        globalPositionalData.AddUnit(position, this);
        tile = (TileWeight)MapController.instance.weightedMap[position];
        MapController.instance.weightedMap[position] = (int)TileWeight.OBSTRUCTED;
        SetDirection(Direction.S);
    }

    public Sprite GetPortrait()
    { return portrait; }

    public string GetName()
    { return unitName; }

    public void SetName(string newName)
    { unitName = newName; }

    public int GetHealth()
    { return health; }

    public void SetHealth(int newHealth)
    { health = newHealth; }

    public void ChangeHealth(int amount, Unit source, UnitAbility attack)
    {
        int newDamage = 0;
        if (amount > 0)
        {
            newDamage = amount;
        }
        else
        {
            newDamage = amount - damageReduction;
        }

        health += (newDamage);

        if (health <= 0)
        {
            DeathData data = new DeathData(source, attack, amount, mapPosition);
            data.DebugLog();
            KillMe(data);
        }
    }

    public int GetImmobilizedDuration()
    { return immobilizedDuration; }

    public void SetImmobilizedDuration(int amount)
    {
        if (amount > immobilizedDuration)
            immobilizedDuration = amount;
    }

    public void Immobilize(int duration)
    {
        hasMoved = true;
        immobilizedDuration = duration - 1;
    }

    public int GetDisabledDuration()
    { return disabledDuration; }

    public void SetDisabledDuration(int amount)
    {
        if (amount > disabledDuration)
            disabledDuration = amount;
    }

    public void Disable(int duration)
    {
        hasActed = true;
        hasMoved = true;
        disabledDuration = duration - 1;
    }

    public int GetMoveSpeed()
    {
        if (GetImmobilizedDuration() > 0 || GetDisabledDuration() > 0)
            return 0;
        return moveSpeed;
    }

    public void SetMoveSpeed(int amount)
    { moveSpeed += amount; }

    public int GetDamageReduction()
    { return damageReduction; }

    public void SetDamageReduction(int amount)
    { damageReduction += amount; }

    public Direction GetDirection()
    { return direction; }

    public void SetDirection(Direction newDirection)
    { direction = newDirection; }

    public Vector2Int GetMapPosition()
    { return mapPosition; }

    public void SetMapPosition(Vector2Int newPosition)
    { mapPosition = newPosition; }


    //TODO: SHOULD HAVE SOME BETTER WAY TO INDICATE WHAT UNIT A PLAYER IS CURRENTLY CONTROLLING
    public void HighlightUnit()
    { m_SpriteRenderer.color = Color.red; }

    public void UnhighlightUnit()
    { m_SpriteRenderer.color = Color.white; }

}

public enum MovementType
{
    WALK,
    DASH,
    KNOCKBACK,
    TELEPORT
}

