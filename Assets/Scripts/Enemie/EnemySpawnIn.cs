    using JetBrains.Annotations;
    using System.Collections.Generic;
    //using Unity.Mathematics;
    using UnityEngine;
    using UnityEngine.SceneManagement;


public class EnemySpawnIn : MonoBehaviour
{

    [System.Serializable]
    public class WeightedEnemy
    {

        public GameObject enemyPrefab;
        [Range(1, 100)]
        [Tooltip("Higher values increase the chance of this enemy being spawned.")]
        public int weight = 50;
    }



    [Header("Möjliga enemy per level")]

    public WeightedEnemy[] Level1Enemies; // inspector
    public WeightedEnemy[] Level2Enemies; // inspector
    public WeightedEnemy[] Level3Enemies; // inspector
    public WeightedEnemy[] Level4Enemies; // inspector

    private List<EnemySpawnPoint> extraSpawn = new List<EnemySpawnPoint>();
    private List<EnemySpawnPoint> alwaysSpawn = new List<EnemySpawnPoint>();



    private void Start()
    {
        EnemySpawnPoint[] allSpawnPoints = FindObjectsOfType<EnemySpawnPoint>();
        foreach (var p in allSpawnPoints)
        {
            if (p.alwaysSpawn)
                alwaysSpawn.Add(p);
            else
                extraSpawn.Add(p);
        }

        SpawnEnemies();
    }
    void SpawnEnemies()
    {

        WeightedEnemy[] pool = GetEnemyPool();
        if (pool == null || pool.Length == 0)
        {
            Debug.LogWarning("No enemies found for the current level.");
            return;
        }

        foreach (var p in alwaysSpawn)
        {
            SpawnWeightedEnemy(pool, p);
        }

        float difficultyFactor = Mathf.Clamp(Difficulty.CurrentDifficulty / 2.5f, 0f, 1f);
        int amountToSpawn = Mathf.RoundToInt(extraSpawn.Count * difficultyFactor);
        if (SceneManager.GetActiveScene().buildIndex == 2) // på nivå 1 ska bara 1 enemy spawna, oavsett svårighetsgrad
        {
            amountToSpawn = 1;
        }

        amountToSpawn = Mathf.Min(amountToSpawn, extraSpawn.Count); // säkerställer att vi inte försöker spawna fler än vad som finns tillgängliga spawn points

        Shuffle(extraSpawn);

        for (int i = 0; i < amountToSpawn; i++) // ändrat i = 1 till i = 0 så att den spawnar rätt antal enemies
        {
            SpawnWeightedEnemy(pool, extraSpawn[i]);
        }



      

    }
    //GameObject[] Pool = GetEnemyPool(); //Pool är alla möjliga enemies för den aktuella nivån
    //if (Pool.Length == 0)
    //    {
    //        Debug.LogWarning("No spawn points found for enemies.");
    //        return;
    //    }
    //foreach (var p in alwaysSpawn)
    //{
    //    SpawnRandomEnemy(Pool, p);
    //}

    //    //float difficulty = Difficulty.CurrentDifficulty;
    //    float difficultyFactor = Mathf.Clamp(Difficulty.CurrentDifficulty / 2.5f, 0f, 1f);
    //    int amountToSpawn = Mathf.RoundToInt(extraSpawn.Count * difficultyFactor);


    //if (SceneManager.GetActiveScene().buildIndex == 2) // på nivå 1 ska bara 1 enemy spawna, oavsett svårighetsgrad
    //{
    //        amountToSpawn = 1;
    //}

    //Shuffle(extraSpawn);

    //    for ( int i = 1; i < amountToSpawn; i++) // i = 1 så att den inte spawnar 2 nör ammaount to Spawn är 1
    //{
    //    SpawnRandomEnemy(Pool, extraSpawn[i]);
    //    }

    void SpawnWeightedEnemy(WeightedEnemy[] pool, EnemySpawnPoint spawnPoint)
    {
        GameObject prefab = PickWeightedRandom(pool);
        if (prefab == null)
        {
            Debug.LogWarning("No enemy prefab selected from the weighted pool.");
            return;
        }

        GameObject spawnedEnemy = Instantiate(prefab, spawnPoint.transform.position, Quaternion.identity);
        SetPortalOnDeath(spawnedEnemy, spawnPoint);
    }

    GameObject PickWeightedRandom(WeightedEnemy[] pool)
    {
        int totalWeight = 0;
        foreach (var enemy in pool)
        {
            totalWeight += enemy.weight;
        }
        int roll = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;
        foreach (var enemy in pool)
        {
            cumulativeWeight += enemy.weight;
            if (roll < cumulativeWeight)
            {
                return enemy.enemyPrefab;
            }
        }
        return pool[pool.Length - 1].enemyPrefab; // fallback, should never reach here if weights are set correctly
    }

    //    void SpawnRandomEnemy(GameObject[] pool, EnemySpawnPoint spawnPoint)
    //    {
    //        GameObject enemyToSpawn = pool[Random.Range(0, pool.Length)];
    //        GameObject spawnedEnemy = Instantiate(enemyToSpawn, spawnPoint.transform.position, Quaternion.identity);

    //        Enemy enemyComponent = spawnedEnemy.GetComponent<Enemy>();
    //        vampire vampireComponent = spawnedEnemy.GetComponent<vampire>();


    //    if (enemyComponent == null && vampireComponent != null)
    //    {
    //        Debug.LogError("Spawned object does not have an Enemy component.");
    //        return;
    //    }



    //    if (spawnPoint.spawnPortal)
    //        enemyComponent.portalActiveOnDeath = true;
    //    else 
    //        enemyComponent.portalActiveOnDeath = false;
    //}       

    void SetPortalOnDeath(GameObject spawnedEnemy, EnemySpawnPoint spawnPoint)
        {
            Enemy enemyComponent = spawnedEnemy.GetComponent<Enemy>();
            vampire vampireComponent = spawnedEnemy.GetComponent<vampire>();

            if (enemyComponent == null && vampireComponent == null)
            {
                Debug.LogError("Spawned object does not have an Enemy or vampire component.");
                return;
            }

            if (enemyComponent != null)
                enemyComponent.portalActiveOnDeath = spawnPoint.spawnPortal;
            if (vampireComponent != null)
                vampireComponent.portalActiveOnDeath = spawnPoint.spawnPortal;
        }

        WeightedEnemy[] GetEnemyPool()
        {
        int level = SceneManager.GetActiveScene().buildIndex;

            switch (level)
            {
                case 2:
                    return Level1Enemies;
                case 3:
                    return Level2Enemies;
                case 4:
                    return Level3Enemies;
                case 5:
                    return Level4Enemies;
                default:
                    return Level1Enemies;
            }
        }



        void Shuffle(List<EnemySpawnPoint> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            EnemySpawnPoint temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
