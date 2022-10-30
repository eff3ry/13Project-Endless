using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

// DEPRECATED SCRIPT NOT USED
public class ObjectSpawner : MonoBehaviour 
{ 
    public GameObject coin;
    public GameObject obstacle;
    [SerializeField] [Range(0,100)] public float coinChance;
    [SerializeField] [Range(0, 100)] public float spawnChance;
    public GameObject cloneParent;
    private float timer; 
    public float spawnDelay = 1;

    // Start is called before the first frame update 
    void Start() 
    { 
        timer = Random.Range(1f, 3f); 
    } 

    // Update is called once per frame 
    void Update() 
    { 
        timer = timer - Time.deltaTime; 
        if (timer < 0) 
        { 
            int chance1 = Random.Range(0,100);
            if (chance1 < spawnChance)
            {
                int chance2 = Random.Range(0,100);
                if (chance2 < coinChance)
                {
                    //spawn coin
                    Instantiate(coin, transform.position, transform.rotation, cloneParent.transform); 
                } else {
                    //spawn obstacle
                    Instantiate(obstacle,transform.position,transform.rotation,cloneParent.transform); 
                }
            }
            timer = spawnDelay; 
        } 
    } 
} 