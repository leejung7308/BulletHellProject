using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemy;
    private float nextSpawn;
    public float spawnDelay = 10f;
    public GameObject lifeObject;
    private void FixedUpdate()
    {
        if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time + spawnDelay;
            GameObject life = Instantiate(lifeObject, new Vector2(-30, 0), Quaternion.identity);
            GameObject enemy = Instantiate(Enemy, new Vector2(0, 0), Quaternion.identity);
        }
    }
}
