using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EnemySpawner is an object that will spawn enemy Units to the map
/// they do so in a specified part of a turn, and spawn Units from a specified list into specified tiles
/// </summary>

public class EnemySpawner : MonoBehaviour
{
    // denotes whether the Units will spawn during the player phase or the enemy's phase
    [SerializeField] public bool spawnAtPlayerPhase;
    // denotes whether the Units will spawn at the start of the phase or the end of the phase
    [SerializeField] public bool spawnAtTurnStart;
    // denotes how many round must pass before a new wave is spawned
    // a 'round' implies that every controller/phase has passed. 
    // For example, with 1 PlayerController and 1 EnemyController, a 'round' implies that both controllers got to control their Units once
    [SerializeField] public int roundsPerWave;

    [SerializeField] public List<UnitData> units;

    [SerializeField] public List<Vector2Int> spawnPositions;


    private void OnEnable()
    {
        TurnController.NewPhaseEvent.AddListener(SpawnUnits);
    }

    private void OnDisable()
    {
        TurnController.NewPhaseEvent.RemoveListener(SpawnUnits);
    }

    // Initiates a spawning of Units, if the conditions apply
    private void SpawnUnits(bool playerPhase, bool turnStart, int round)
    {
        Debug.Log(playerPhase + ", " + turnStart + ", " + round + ", " + round % roundsPerWave);
        if (playerPhase == spawnAtPlayerPhase && turnStart == spawnAtTurnStart && round % roundsPerWave == 0)
        {
            Debug.Log("EnemySpawner: Spawning New Units...");
            foreach (Vector2Int tile in spawnPositions)
            {
                int index = new System.Random().Next(units.Count);
                Debug.Log("Spawning new Unit " + units[index].name + " at position " + tile);
                StartCoroutine(SpawnUnit(index, tile));
            }
        }
    }

    private IEnumerator SpawnUnit(int unit, Vector2Int position)
    {
        // parse the spawn location and spawn a new object there
        Vector3 playerPos = MapMath.MapToWorld(position.x, position.y);
        GameObject shell = new GameObject();
        GameObject newUnit = Instantiate(shell, playerPos, Quaternion.identity);
        Destroy(shell);
        newUnit.AddComponent<SpriteRenderer>();
        newUnit.GetComponent<SpriteRenderer>().enabled = false;
        Unit newUnitComponent = newUnit.AddComponent<EnemyUnit>() as EnemyUnit;
        newUnitComponent.StartData = units[unit];
        newUnitComponent.globalPositionalData = TurnController.instance.globalPositionalData;
        newUnitComponent.LoadData();
        newUnitComponent.SetHealth(0);

        EventsManager.instance.AddEvent("spawning");

        CameraController.instance.targetPos = playerPos;
        yield return new WaitForSecondsRealtime(0.5f);
        Graveyard.NewEnemiesEvent.Invoke(new List<Unit> { newUnitComponent });
        yield return new WaitForSecondsRealtime(0.5f);

        EventsManager.instance.RemoveEvent("spawning");
    }

}

