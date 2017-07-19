using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWorldMovement : MonoBehaviour 
{
    public Transform playerToFollow;
    public Vector2 boundaries;
    public Vector3 offset;

    void LateUpdate()
    {
        transform.position = playerToFollow.position + offset;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -boundaries.x, boundaries.x),
            Mathf.Clamp(transform.position.y, -boundaries.y, boundaries.y),
            transform.position.z
        );
    }
}