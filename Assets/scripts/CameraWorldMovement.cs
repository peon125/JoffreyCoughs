using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWorldMovement : MonoBehaviour 
{
    public Vector3 offset;
    public bool moveCamera;

    void LateUpdate()
    {
        if (moveCamera)
            transform.position = Player._instance.transform.position + offset;
    }
}