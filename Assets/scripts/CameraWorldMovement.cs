using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWorldMovement : MonoBehaviour 
{
    public Vector3 offset;
    public int zoomingStep;
    public bool moveCamera;

    void LateUpdate()
    {
        if (moveCamera)
            transform.position = Player._instance.transform.position + offset;

        if (Input.GetButtonDown("ZoomIn"))
        {
            Camera.main.orthographicSize -= zoomingStep;
        } else if(Input.GetButtonDown("ZoomOut"))
        {
            Camera.main.orthographicSize += zoomingStep;
        }
    }
}