using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravellingController : MonoBehaviour
{
    public static TravellingController _instance;
    public int whichPortalToResp;

    void Awake()
    {
        _instance = this;
    }

    public Transform GetInteractables(string area)
    {

        return null;
    }
}