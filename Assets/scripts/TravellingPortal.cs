using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravellingPortal : MonoBehaviour
{
    public TravellingController myController;
    public Vector3 whereToResp;
    public bool playerWantsToTravel;

    void OnTriggerEnter2D(Collider2D collider)
    {
        playerWantsToTravel = true;
    }
}