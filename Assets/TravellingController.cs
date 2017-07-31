using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravellingController : MonoBehaviour
{
    public static TravellingController _instance;
    public Transform desertInteractables;
    public Transform caveInteractables;

    void Awake()
    {
        _instance = this;
    }

    public Transform GetInteractables(string area)
    {
        switch (area)
        {
            case "desert":
                return desertInteractables;
                break;
            case "cave":
                return caveInteractables;
                break;
        }

        return null;
    }
}