using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTransform : MonoBehaviour
{
    public Transform transformToCopy;
    public bool copyPosition = false;
    public Vector3 positionOffset = new Vector3();
    public bool copyRotation = false;
    public bool useFixedUpdate = false;

    void Update()
    {
        if (copyPosition)
        {
            transform.position = transformToCopy.position + positionOffset;
        }

        if (copyRotation)
        {
            transform.rotation = transformToCopy.rotation;
        }
    }

}
