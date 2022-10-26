using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    // Update is called once per frame
    // constantly rotates the coin 
    void Update()
    {
        Vector3 rot =transform.localEulerAngles;
        rot.y += rotationSpeed * Time.deltaTime;
        transform.localEulerAngles = rot;
    }
}
