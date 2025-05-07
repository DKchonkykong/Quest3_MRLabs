using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Spawner : MonoBehaviour
{
    public GameObject cube; // Spawnable object you want to use
    public float spawnRate = 1f; // Time between spawns (seconds)
    private float nextSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        nextSpawnTime = Time.time + spawnRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnObject();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnObject()
    {
        if (cube != null) // Use the 'cube' variable here
        {
            Instantiate(cube, transform.position, transform.rotation);
        }
        else
        {
            Debug.LogError("Object to spawn is not assigned in the Object spawner");
        }
    }
}
