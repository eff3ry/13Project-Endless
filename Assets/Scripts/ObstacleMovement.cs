using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

public class ObstacleMovement : MonoBehaviour 
{ 
    public float speed = 15f; 
    // Update is called once per frame 
    void Update() 
    { 
        transform.position = transform.position + new Vector3(0, 0, -1 * speed * Time.deltaTime); 

        if (transform.position.z < -10f) 
        { 
            Destroy(gameObject); 
            Debug.Log("Destroy Obstacle"); 
        } 
    } 
} 