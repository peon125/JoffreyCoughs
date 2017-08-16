using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaHandler : MonoBehaviour
{
    public Transform interactables;

    void Start()
    {
        Player._instance.interactables = interactables;
    }
}