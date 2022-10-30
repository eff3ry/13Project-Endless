using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

public class ObstacleMovement : MonoBehaviour 
{ 
    public float speed = 15f; 

    void Update() 
    { 
        // Constatly moves object on the z axis towards player
        transform.position = transform.position + new Vector3(0, 0, -1 * speed * Time.deltaTime); 

        // Destroy past -10 z
        if (transform.position.z < -10f) 
        { 
            Destroy(gameObject); 
            //Debug.Log("Destroy Obstacle"); 
        } 
    } 
} 