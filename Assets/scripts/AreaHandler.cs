using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaHandler : MonoBehaviour
{
    public GameObject[] interactables;
    public Transform[] portals;

    void Start()
    {
        interactables = GameObject.FindGameObjectsWithTag("interactable");

        //Player._instance.transform.position = portals[Player._instance.travellingController.whichPortalToResp].position;
        Player._instance.interactables = interactables;
    }
}