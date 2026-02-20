    using JetBrains.Annotations;
    using NUnit.Framework;
    using System.Collections.Generic;
    //using Unity.Mathematics;
    using UnityEngine;
    using UnityEngine.SceneManagement;


    public class EnemySpawnIn : MonoBehaviour
    {
       

            [Header("Möjliga enemy per level")]

        public GameObject[] Level1Enemies; // inspector
        public GameObject[] Level2Enemies; // inspector
        public GameObject[] Level3Enemies; // inspector
        public GameObject[] Level4Enemies; // inspector

        private List<EnemySpawnPoint> extraSpawn = new List<EnemySpawnPoint>();
        private List<EnemySpawnPoint> alwaysSpawn = new List<EnemySpawnPoint>();



    private void Start()
        {
            EnemySpawnPoint[] allSpawnPoints = FindObjectsOfType<EnemySpawnPoint>();
            foreach(var p  in allSpawnPoints)
            {
                if (p.alwaysSpawn)
                    alwaysSpawn.Add(p);
                else
                    extraSpawn.Add(p);
        }

        //spawnPoints.AddRange(FindObjectsOfType<EnemySpawnPoint>());

            SpawnEnemies();
        }
        void SpawnEnemies()
        {
        GameObject[] Pool = GetEnemyPool(); //Pool är alla möjliga enemies för den aktuella nivån
        if (Pool.Length == 0)
            {
                Debug.LogWarning("No spawn points found for enemies.");
                return;
            }
        foreach (var p in Pool)
        {
            SpawnRandomEnemy(Pool, p.transform.position);
        }

            //float difficulty = Difficulty.CurrentDifficulty;
            float difficultyFactor = Mathf.Clamp(Difficulty.CurrentDifficulty / 2.5f, 0f, 1f);
            int amountToSpawn = Mathf.RoundToInt(extraSpawn.Count * difficultyFactor);

        //float amountToSpawn = Mathf.RoundToInt(Mathf.Clamp(difficulty * extraSpawn.Count / 3, 1, extraSpawn.Count));

        if (SceneManager.GetActiveScene().buildIndex == 2) // på nivå 1 ska bara 1 enemy spawna, oavsett svårighetsgrad
        {
                amountToSpawn = 1;
        }

        Shuffle(extraSpawn);

            for ( int i = 1; i < amountToSpawn; i++) // i = 1 så att den inte spawnar 2 nör ammaount to Spawn är 1
        {
            SpawnRandomEnemy(Pool, extraSpawn[i].transform.position);
            }

        }
        void SpawnRandomEnemy(GameObject[] pool, Vector3 position)
        {
            GameObject enemyToSpawn = pool[Random.Range(0, pool.Length)];
            Instantiate(enemyToSpawn, position, Quaternion.identity);
    }

    GameObject[] GetEnemyPool()
        {
            int level = SceneManager.GetActiveScene().buildIndex;

        switch (level)
        {
            case 1:
                return Level1Enemies;
            case 2:
                return Level2Enemies;
            case 3:
                return Level3Enemies;
            case 4:
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
