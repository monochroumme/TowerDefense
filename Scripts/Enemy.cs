using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    Vector3 target;
    int targetIndex = 0;
    Vector3 dir;

    void Start()
    {
        speed = LevelGenerator.targets.Count / 20f + LevelGenerator.maxNodesInARow / 10f + WaveSpawner.waveIndex * 0.1f;
        target = LevelGenerator.targets[targetIndex];
        dir = target - transform.position;

        InvokeRepeating("CheckEnemysPos", 10f, 10f);
    }

    void Update()
    {
        dir = target - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime);

        if(Vector3.Distance(target, transform.position) <= 0.15f)
        {
            GetNextTarget();
        }
    }

    void GetNextTarget()
    {
        if(targetIndex >= LevelGenerator.targets.Count - 1)
        {
            Destroy(gameObject);
            TowerManager.health--;
            return;
        }

        targetIndex++;
        target = LevelGenerator.targets[targetIndex];
    }

    void CheckEnemysPos()
    {
        if (gameObject.transform.position.x > 30f || gameObject.transform.position.x < -30f || gameObject.transform.position.z > 30f || gameObject.transform.position.z < -30f)
            Destroy(gameObject);
    }
}