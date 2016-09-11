using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float timeToStartWave;
    public Text waveCountdownText;

    List<GameObject> enemys;
    Vector3 spawnPoint;
    public static int waveIndex = 0;
    float countdown;

    void Start()
    {
    	waveIndex = 0;
        Invoke("LateStart", 1f);
        countdown = timeToStartWave;
        enemys = new List<GameObject>();
    }

    void LateStart()
    {
        spawnPoint = LevelGenerator.spawnPoint + Vector3.up * 0.4f;
    }

    void Update()
    {
        if (enemys.Count > 0)
        {
            for (int i = 0; i < enemys.Count; i++)
            {
                if (enemys[i] == null)
                    enemys.RemoveAt(i);
            }
        }

        if(enemys.Count <= 0)
        {
            countdown -= Time.deltaTime;
        }
        else
        {
            waveCountdownText.text = "W";
            return;
        }

        if(countdown <= 0)
        {
            StartCoroutine(SpawnWave());
            countdown = timeToStartWave;
        }

        waveCountdownText.text = Mathf.Round(countdown).ToString();
    }

    IEnumerator SpawnWave()
    {
        waveIndex++;

        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.3f);
        }
    }

    void SpawnEnemy()
    {
        GameObject enemy = (GameObject)Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        enemys.Add(enemy);
    }
}