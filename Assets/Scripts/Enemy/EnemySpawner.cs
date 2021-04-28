﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [SerializeField] BasicEnemy enemy;

    [Header("Spawn zone")]
    [SerializeField] float spawnZ = 7f;
    [SerializeField] float spawnXMin = -3f;
    [SerializeField] float spawnXMax = 3f;

    [Header("Timing")]
    [SerializeField] float initialWait = 7f;
    [SerializeField] float spawnIntervalMin = 0.5f;
    [SerializeField] float spawnIntervalMax = 3f;

    //Status
    float timer = 0f;
    float speedUpMod = 1f;

    //Cache
    Quaternion rotation;
    List<BasicEnemy> spawned = new List<BasicEnemy>();

    public void MoveAll ()
    {

        for (int i = 0; i < Random.Range(1, 3); i++)
        {
            SpawnEnemy();
        }

        for (int i = spawned.Count - 1; i >= 0; i--)
        {
            StartCoroutine(spawned[i].DoMove());
        }
    }


    public void RemoveEnemy (BasicEnemy enemy)
    {
        if (spawned.Contains(enemy))
        {
            spawned.Remove(enemy);
            Destroy(enemy.gameObject);
        }
    }

    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        rotation = Quaternion.LookRotation(-Vector3.forward, Vector3.up);

        //timer = spawnIntervalMax;
        //yield return new WaitForSeconds(initialWait);
        //StartCoroutine(DoSpawn());
    }

    //IEnumerator DoSpawn()
    //{
    //    while (true)
    //    {
    //        if (timer > 0f)
    //        {
    //            timer -= Time.deltaTime;
    //        }
    //        else
    //        {
    //            SpawnEnemy();
    //            RefreshTimer();
    //        }
    //        yield return null;
    //    }
    //}

    void RefreshTimer()
    {
        timer = Random.Range(spawnIntervalMin, spawnIntervalMax);
        if (speedUpMod > 0.1f)
        {
            speedUpMod -= Time.deltaTime;
        }
        timer *= speedUpMod;
    }

    void SpawnEnemy()
    {
        BasicEnemy e = Instantiate(enemy, RandomSpawnPos, rotation);
        spawned.Add(e);
        e.Initialize(this);
    }

    void DecrementTimer() => timer = Mathf.Clamp(timer - Time.deltaTime, spawnIntervalMin, spawnIntervalMax);
    Vector3 RandomSpawnPos => new Vector3(Random.Range(spawnXMin, spawnXMax), 0f, spawnZ);
}
