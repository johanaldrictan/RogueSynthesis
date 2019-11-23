using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class containing a list of attack patterns 
/// </summary>
public static class AttackHelper
{
    public static List<Vector2Int> GetTShapedAOE(Vector2Int source, Direction direction, int abilityRange)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        Vector2Int origin = source;

        for (int i = 0; i < abilityRange; i++)
        {
            origin += MapMath.DirToRelativeLoc(direction);
        }

        if (!MapMath.InMapBounds(origin))
        { return result; }

        result.Add(origin);

        if (direction == Direction.N || direction == Direction.S)
        {
            if (MapMath.InMapBounds(new Vector2Int(origin.x + 1, origin.y))) { result.Add(new Vector2Int(origin.x + 1, origin.y)); }
            if (MapMath.InMapBounds(new Vector2Int(origin.x - 1, origin.y))) { result.Add(new Vector2Int(origin.x - 1, origin.y)); }
        }
        else if (direction == Direction.E || direction == Direction.W)
        {
            if (MapMath.InMapBounds(new Vector2Int(origin.x, origin.y + 1))) { result.Add(new Vector2Int(origin.x, origin.y + 1)); }
            if (MapMath.InMapBounds(new Vector2Int(origin.x, origin.y - 1))) { result.Add(new Vector2Int(origin.x, origin.y - 1)); }
        }
        return result;
    }


    public static List<Vector2Int> GetLineAOE(Vector2Int source, Direction direction, int abilityRange)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        Vector2Int origin = source;

        origin += MapMath.DirToRelativeLoc(direction);

        if (!MapMath.InMapBounds(origin))
        { return result; }

        //one tile forward
        result.Add(origin);

        for (int i = 0; i < abilityRange - 1; i++)
        {
            origin += MapMath.DirToRelativeLoc(direction);
            result.Add(origin);
        }
        return result;
    }


    //variation of the findmovetiles alg to save time in devising an algorithm. 
    public static List<Vector2Int> GetCircleAOE(Vector2Int source, Direction direction, int abilityRadius)
    {
        List<Vector2Int> circleList = new List<Vector2Int>();
        Dictionary<Vector2Int, int> movementCost = new Dictionary<Vector2Int, int>();

        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        PriorityQueue<Vector2Int> frontier = new PriorityQueue<Vector2Int>();
        frontier.Enqueue(source, 0); // Should only contain tiles in range
        movementCost[source] = 0; // Contains frontier and visited
        circleList.Add(source);

        while (frontier.Count != 0)
        {
            Vector2Int visiting = frontier.Dequeue();
            if (visited.Contains(visiting)) { continue; }

            Dictionary<Vector2Int, Direction> neighbors = MapMath.GetNeighbors(visiting);
            foreach (Vector2Int neighbor in neighbors.Keys)
            {
                if (visited.Contains(neighbor) || !MapMath.InMapBounds(neighbor)) { continue; }
                int nextDist = 1 + movementCost[visiting];
                if (nextDist > abilityRadius) { continue; }
                if (!movementCost.ContainsKey(neighbor) || nextDist < movementCost[neighbor])
                {
                    frontier.Enqueue(neighbor, nextDist);
                    movementCost[neighbor] = nextDist;
                    circleList.Add(neighbor);
                }
            }

            visited.Add(visiting);
        }

        return circleList;
    }


    /// <summary>
    /// Knocks target unit in a line with a given direction.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="knockback"></param>
    /// <param name="kbDirection"></param>
    public static void DisplaceUnit(Unit target, Unit source, Attack ability, int knockback, Direction kbDirection)
    {
        //look for collisions
        Vector2Int pushbackLoc = target.GetMapPosition() + (MapMath.DirToRelativeLoc(kbDirection) * knockback);
        List<Vector2Int> pushbackArea = GetLineAOE(target.GetMapPosition(), kbDirection, knockback);
        Vector2Int? searchResult = null;
        foreach (Vector2Int tile in pushbackArea)
        {
            if (MapController.instance.weightedMap[tile] == (int)TileWeight.OBSTRUCTED)
            {
                searchResult = tile;
                break;
            }
        }
        if (searchResult != null)
        {
            //deal damage when we cant go full knockback dist
            target.ChangeHealth(-1, source, ability);
            Vector2Int diff = (Vector2Int)searchResult - target.GetMapPosition();
            Vector2Int absDiff = new Vector2Int(Mathf.Abs(diff.x), Mathf.Abs(diff.y));
            Debug.Assert(source.GetDirection() != Direction.NO_DIR);
            //Debug.Log(pushbackLoc);
            switch (source.GetDirection())
            {
                case Direction.N:
                    pushbackLoc = new Vector2Int(target.GetMapPosition().x, target.GetMapPosition().y + (absDiff.y - 1));
                    break;
                case Direction.S:
                    pushbackLoc = new Vector2Int(target.GetMapPosition().x, target.GetMapPosition().y - (absDiff.y - 1));
                    break;
                case Direction.W:
                    pushbackLoc = new Vector2Int(target.GetMapPosition().x - (absDiff.x - 1), target.GetMapPosition().y);
                    break;
                case Direction.E:
                    pushbackLoc = new Vector2Int(target.GetMapPosition().x + (absDiff.x - 1), target.GetMapPosition().y);
                    break;
            }
            //Debug.Log(newLocation);
        }
        if (MapMath.InMapBounds(pushbackLoc))
        {
            target.Move(pushbackLoc.x, pushbackLoc.y);
            target.hasMoved = false;
        }
        //else if()
        else
        {
            DeathData data = new DeathData(source, ability, ability.GetDamage(), target.GetMapPosition());
            target.KillMe(data);
        }
    }
}
