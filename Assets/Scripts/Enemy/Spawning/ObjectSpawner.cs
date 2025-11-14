using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor.Search;

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
    float obstacleBlockEndZ = 0f;

    [Header("Chão")]
    public GameObject groundPrefab;
    public GameObject leftSide;
    public GameObject rightSide;
    private float groundLength = 8f;
    private int groundAhead = 10; // quantos blocos de chão ficam na frente do player
    private Quaternion groundRotation = Quaternion.Euler(new Vector3 (-90, 0, 0));

    [Header("PowerUps")]
    public GameObject[] powerUpPrefabs; // deve ter exatamente 3 prefabs (um por pista)
    private float firstSpawnDistance = 270f;
    private float repeatEvery = 300f;
    private float noObstacleDistance = 30f;
    private float nextPowerUpDistance = 500f;


    [Header("Armas")]
    public GameObject[] weaponPrefabs;
    private float spawnDistance = 100f;

    [Header("Ultimates")]
    public GameObject[] ultPrefabs;
    private float spawnUlt = 500f;
    private bool canSpawnUlt = true;

    [Header("Inimigos")]
    public GameObject[] enemyPrefabs;
    private float enemySpawnInterval = 7f; // a cada X segundos aparece um
    private float timeEnemy = 0f;
    private int blockedLane = int.MinValue; // lane que deve ficar sem obstáculo


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
        nextPowerUpDistance = firstSpawnDistance;
        canSpawnUlt = true;
    }

    void Update()
    {
        float distance = playerStats.distance; //distância percorrida
        float score = playerStats.score;//pontuação

        //aumento gradual de dificuldade
        if (playerStats.speed <= 10 && playerStats.speed >= 5)
        {
            obstacleSpawnInterval = 11f - playerStats.speed;
        }
        else if (playerStats.speed > 10)
        {
            obstacleSpawnInterval = 0.4f;
        }
        if (playerStats.speed >= 12)
        {
            enemySpawnInterval = 3f;
        }

    if (score < 1900f)
    {

        
        // spawn de powerups em milestones
        if (distance >= nextPowerUpDistance)
        {
            SpawnPowerUps();
            nextPowerUpDistance += repeatEvery;
        }

        //spawn de armas
        if (distance >= spawnDistance && spawnDistance > 5f)
        {
            SpawnWeapon();
            spawnDistance = 4f;
        }

        if (distance >= spawnUlt && canSpawnUlt&&PlayerUltimate.haveUlt==false)
        {
           SpawnUlt();
           canSpawnUlt= false;
        }

            // Inimigos
            if (player.position.z > obstacleBlockEndZ)
            {
                if (Time.time >= timeEnemy)
                {
                    SpawnEnemy();
                    timeEnemy = Time.time+enemySpawnInterval;
                }
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
    //Carrega tela de vitoria ao chegar na distância determinada. Para de spawnar tudo um pouco antes
    }else if (score >= 2000)
        {

            SceneManager.LoadScene("Victory");
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
        // se for a lane bloqueada por inimigo, pula
        if (lane == blockedLane) return;
        int objIndex = Random.Range(0, obstaclePrefabs.Length);
        GameObject prefab = obstaclePrefabs[objIndex];
        if (objIndex == 6)
        {
            lane = 0;
        }
        Vector3 spawnPos = new Vector3(lane * laneOffset, 0, player.position.z + obstacleSpawnDistance);

        GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);

        //ajuste para tocar no chão
        SetObjectOnGround(obj);
        spawnedObjects.Add(obj);
    }

    void SpawnGround()
    {
        //Fica spawnando o chão
        Vector3 spawnPos = new Vector3(0, -0.7f, nextGroundZ);
        GameObject obj = Instantiate(groundPrefab, spawnPos, groundRotation);
        spawnedObjects.Add(obj);
        spawnPos = new Vector3(-24f, -0.7f, nextGroundZ);
        obj = Instantiate(leftSide, spawnPos, groundRotation);
        spawnedObjects.Add(obj);
        spawnPos = new Vector3(26f, -0.7f, nextGroundZ);
        obj = Instantiate(rightSide, spawnPos, groundRotation);
        spawnedObjects.Add(obj);

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
                Vector3 pos = new Vector3(lane * laneOffset, 0f, spawnZ+10f);
                GameObject obj = Instantiate(powerUpPrefabs[index], pos, Quaternion.identity);
                spawnedObjects.Add(obj);
            }
        }

        // bloquear obstáculos após spawn
        obstacleBlockEndZ = spawnZ + noObstacleDistance;
    }

    void SpawnWeapon()
    {
        float spawnZ = player.position.z + obstacleSpawnDistance;

        // uma por lane: -1, 0, 1
        for (int lane = -1; lane <= 1; lane++)
        {
            int index = lane + 1; // -1 -> 0, 0 -> 1, 1 -> 2
            if (index >= 0 && index < weaponPrefabs.Length)
            {
                Vector3 pos = new Vector3(lane * laneOffset, 1f, spawnZ+10f);
                GameObject obj = Instantiate(weaponPrefabs[index], pos, Quaternion.identity);
                spawnedObjects.Add(obj);
            }
        }

        // bloquear obstáculos após spawn
        obstacleBlockEndZ = spawnZ + noObstacleDistance;
    }

    void SpawnUlt()
    {
        float spawnZ = player.position.z + obstacleSpawnDistance;

        // uma por lane: -1, 0, 1
        for (int lane = -1; lane <= 1; lane++)
        {
            int index = lane + 1; // -1 -> 0, 0 -> 1, 1 -> 2
            if (index >= 0 && index < ultPrefabs.Length)
            {
                Vector3 pos = new Vector3(lane * laneOffset, 2f, spawnZ + 10f);
                GameObject obj = Instantiate(ultPrefabs[index], pos, Quaternion.identity);
                spawnedObjects.Add(obj);
            }
        }

        // bloquear obstáculos após spawn
        obstacleBlockEndZ = spawnZ + noObstacleDistance;
    }

    void SpawnEnemy()
    {
        int lane = Random.Range(-1, 2);
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);
        Vector3 spawnPos = new Vector3(lane * laneOffset, 0.5f, player.position.z + obstacleSpawnDistance);
        GameObject prefab = enemyPrefabs[enemyIndex];
        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

        //Ajusta o inimigo para tocar o chão
        if (enemyIndex == 0)
        {
            SetEnemyOnGround(enemy);
        }
        else
        {
            SetObjectOnGround(enemy);
        }


        // não spawna obstáculo nessa lane
        blockedLane = lane;
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

    void SetObjectOnGround(GameObject obj)
    {
        // Obtém o ponto mais baixo do objeto (inclui todos os MeshRenderers/Colliders)
        float lowestY = float.MaxValue;

        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            if (r.bounds.min.y < lowestY)
                lowestY = r.bounds.min.y;
        }

        // Caso não tenha Renderer, tenta usar Collider
        if (renderers.Length == 0)
        {
            Collider[] colliders = obj.GetComponentsInChildren<Collider>();
            foreach (Collider c in colliders)
            {
                if (c.bounds.min.y < lowestY)
                    lowestY = c.bounds.min.y;
            }
        }

        // Altura do chão
        float groundY = -0.7f;

        // Diferença necessária para encostar no chão
        float offsetY = groundY - lowestY;

        // Aplica o deslocamento
        obj.transform.position += new Vector3(0, offsetY, 0);
    }

    void SetEnemyOnGround(GameObject obj)
    {
        // Obtém o ponto mais baixo do objeto (inclui todos os MeshRenderers/Colliders)
        float lowestY = float.MaxValue;

        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            if (r.bounds.min.y < lowestY)
                lowestY = r.bounds.min.y;
        }

        // Caso não tenha Renderer, tenta usar Collider
        if (renderers.Length == 0)
        {
            Collider[] colliders = obj.GetComponentsInChildren<Collider>();
            foreach (Collider c in colliders)
            {
                if (c.bounds.min.y < lowestY)
                    lowestY = c.bounds.min.y;
            }
        }

        // Altura do chão
        float groundY = -1.9f;

        // Diferença necessária para encostar no chão
        float offsetY = groundY - lowestY;

        // Aplica o deslocamento
        obj.transform.position += new Vector3(0, offsetY, 0);
    }
}
