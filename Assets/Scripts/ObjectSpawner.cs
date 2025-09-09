using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;
    public PlayerStats playerStats;

    [Header("Obstáculos")]
    public GameObject[] obstaclePrefabs;
    public float obstacleSpawnDistance = 40f;
    public float obstacleSpawnInterval = 3f;
    public float laneOffset = 3f;

    [Header("Chão")]
    public GameObject groundPrefab;
    public float groundLength = 20f;
    public int groundAhead = 5; // quantos blocos de chão ficam na frente do player

    [Header("PowerUps")]
    public GameObject[] powerUpPrefabs; // deve ter exatamente 3 prefabs (um por pista)
    public float firstSpawnDistance = 1000f;
    public float repeatEvery = 500f;
    public float noObstacleDistance = 10f;

    [Header("Limpeza")]
    public float despawnDistance = 30f; // distância atrás do player para destruir objetos

    private float obstacleTimer;
    private float nextGroundZ;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
        // cria blocos iniciais de chão
        nextGroundZ = 0f;
        for (int i = 0; i < groundAhead; i++)
        {
            SpawnGround();
        }
        nextPowerUpScore = firstSpawnDistance;
    }

    void Update()
    {
        float score = playerStats.score; //distância percorrida

        //aumento gradual de dificuldade
        if (playerStats.speed <= 10&&playerStats.speed>=5)
        {
            obstacleSpawnInterval = 11f - playerStats.speed;
        }
        else if (playerStats.speed > 10)
        {
            obstacleSpawnInterval = 0.5f;
        }

        // spawn de powerups em milestones
        if (score >= nextPowerUpScore)
        {
            SpawnPowerUps();
            nextPowerUpScore += repeatEvery;
        }

        // spawn de obstáculos
        if (player.position.z > obstacleBlockEndZ)
        {
            obstacleTimer += Time.deltaTime;
            if (obstacleTimer >= obstacleSpawnInterval)
            {
                SpawnObstacle();
                obstacleTimer = 0f;
            }
        }

        // spawn de chão sempre que necessário
        if (player.position.z + (groundAhead * groundLength) > nextGroundZ)
        {
            SpawnGround();
        }

        // destruir objetos atrás do player
        CleanupObjects();
    }

    void SpawnObstacle()
    {
        int lane = Random.Range(-1, 2); // -1, 0, 1
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        Vector3 spawnPos = new Vector3(lane * laneOffset, 0, player.position.z + obstacleSpawnDistance);

        GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);
        spawnedObjects.Add(obj);
    }

    void SpawnGround()
    {
        //Fica spawnando o chão
        Vector3 spawnPos = new Vector3(0, -1, nextGroundZ);
        GameObject obj = Instantiate(groundPrefab, spawnPos, Quaternion.identity);
        spawnedObjects.Add(obj);
        //adicionar Destroy(groundPrefab, 20f); quando trocar o obj para um prefab

        nextGroundZ += groundLength;
    }

    void SpawnPowerUps()
    {
        float spawnZ = player.position.z + obstacleSpawnDistance;

        // uma por lane: -1, 0, 1
        for (int lane = -1; lane <= 1; lane++)
        {
            int index = lane + 1; // -1 -> 0, 0 -> 1, 1 -> 2
            if (index >= 0 && index < powerUpPrefabs.Length)
            {
                Vector3 pos = new Vector3(lane * laneOffset, 1f, spawnZ);
                GameObject obj = Instantiate(powerUpPrefabs[index], pos, Quaternion.identity);
                spawnedObjects.Add(obj);
            }
        }

        // bloquear obstáculos por 10 metros após spawn
        obstacleBlockEndZ = spawnZ + noObstacleDistance;
    }

    void CleanupObjects()
    {
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (spawnedObjects[i] == null) continue;

            if (spawnedObjects[i].transform.position.z < player.position.z - despawnDistance)
            {
                Destroy(spawnedObjects[i]);
                spawnedObjects.RemoveAt(i);
            }
        }
    }
}
