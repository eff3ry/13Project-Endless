using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DEPRECATED SCRIPT NOT USED
public class SpawnerManager : MonoBehaviour
{
    public List<ObjectSpawner> spawners;
    [SerializeField] [Range(0,100)] float coinChance = 20;
    [SerializeField] [Range(0, 100)] float spawnChance = 60;
    [SerializeField] float difficulty = 1;
    public float maxDifficulty = 3;

    // Start is called before the first frame update
    void Start()
    {
        foreach (ObjectSpawner spawner in spawners)
        {
            spawner.coinChance = coinChance;
            spawner.spawnChance = spawnChance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (difficulty < maxDifficulty)
        {
            difficulty = 1 + Time.time/20;
            float spawnDelay = 1/difficulty;
            foreach (ObjectSpawner spawner in spawners)
            {
                spawner.spawnDelay = spawnDelay;
            }
        }
    }
}
