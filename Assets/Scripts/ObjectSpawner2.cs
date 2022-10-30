using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner2 : MonoBehaviour
{
    [SerializeField] List<GameObject> spawnPoints;
    [SerializeField] [Range(0,100)] float coinChance = 0;

    public float spawnTime = 1;
    public float obstacleSpeed = 15f;
    public GameObject cloneParent;
    public GameObject[] rewards;
    public GameObject obstacle;
    float timer = 0;

    // Spawns items when timer is 0
    void Update()
    {
        // Keep track of time
        timer += Time.deltaTime;

        // When timer is done
        if (timer > spawnTime)
        {
            timer = 0;

            // Generate a list of objects to spawn
            List<int> spawnList = generateSpawnList();
            // Make sure there is an object which lets you pass (make sure it not impossible)
            // Keep generating until there is a valid list
            while (!(spawnList.Contains(0) || spawnList.Contains(2)))
            {
                spawnList = generateSpawnList();
            }

            // Loop throught the list to insntantiate each object
            for (int i = 0; i < spawnList.Count; i++)
            {
                if (spawnList[i] == 1)
                {
                    // Spawn obstacle
                    GameObject obj = Instantiate(obstacle, spawnPoints[i].transform.position, transform.rotation, cloneParent.transform); 
                    obj.GetComponent<ObstacleMovement>().speed = obstacleSpeed;
                    Debug.Log("Spawn Obstacle");
                } else if (spawnList[i] == 2)
                {
                    // Pick random reward
                    int reward = Random.Range(0,rewards.Length);

                    GameObject obj = Instantiate(rewards[reward], spawnPoints[i].transform.position, transform.rotation, cloneParent.transform); 
                    obj.GetComponent<ObstacleMovement>().speed = obstacleSpeed;
                    Debug.Log("Spawn coin");
                }
            }
        }
    }

    

    // Makes a list of ints for deciding the object to spawn and in what position
    // Based off of the following values,
    // 0 is empty
    // 1 is obstacle
    // 2 is reward
    List<int> generateSpawnList()
    {
        List<int> spawns = new List<int>();
        foreach (GameObject point in spawnPoints)
        {
            int random = Random.Range(0,2);
            spawns.Add(random);
        }

        // Chance for an empty to turn into a coin
        for (int i = 0; i < spawns.Count; i++)
        {
            if (spawns[i] == 0 && Random.Range(0,101) < coinChance)
            {
                spawns[i] = 2;
            }
        }
        return spawns;
    }
}
