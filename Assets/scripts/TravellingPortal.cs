using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravellingPortal : MonoBehaviour
{
    public string place;
    public Vector3 whereToResp;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Player>())
        {
            collider.GetComponent<Player>().interactables = TravellingController._instance.GetInteractables(place);
            collider.transform.position = whereToResp;
        }
    }
}