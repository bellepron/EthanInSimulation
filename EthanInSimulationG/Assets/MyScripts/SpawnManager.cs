using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    private float spawnRange = 20;
    public int enemyCount;
    public int waveNumber = 1;

    public GameObject powerupPrefab;


    void Start()
    {
        Vector3 randPosPowerup = new Vector3(Random.Range(-5, 5), 1, Random.Range(-5, 5));
        Instantiate(powerupPrefab, randPosPowerup, powerupPrefab.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = GameObject.FindObjectsOfType<FollowAI>().Length
        + GameObject.FindObjectsOfType<FollowAIInst>().Length;

        if (enemyCount == 0)
        {
            enemyCount++;
            SpawnEnemyWave(waveNumber);
            Instantiate(powerupPrefab, new Vector3(Random.Range(-5, 5), 1, Random.Range(-5, 5)), powerupPrefab.transform.rotation);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        //Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        Vector3 randomPos = new Vector3(35, 0, 30 + spawnPosZ);
        return randomPos;
    }

    public void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }

}
