using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;

    [Header("Obstáculos")]
    public GameObject[] obstaclePrefabs;
    public float obstacleSpawnDistance = 40f;
    public float obstacleSpawnInterval = 2f;
    public float laneOffset = 3f;

    [Header("Chão")]
    public GameObject groundPrefab;
    public float groundLength = 20f;
    public int groundAhead = 5; // quantos blocos de chão ficam na frente do player

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
    }

    void Update()
    {
        // spawn de obstáculos
        obstacleTimer += Time.deltaTime;
        if (obstacleTimer >= obstacleSpawnInterval)
        {
            SpawnObstacle();
            obstacleTimer = 0f;
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
        Vector3 spawnPos = new Vector3(0, 0, nextGroundZ);
        GameObject obj = Instantiate(groundPrefab, spawnPos, Quaternion.identity);
        spawnedObjects.Add(obj);
        Destroy(groundPrefab, 20f);

        nextGroundZ += groundLength;
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
