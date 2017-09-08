using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaHandler : MonoBehaviour
{
    public static AreaHandler areaWhereICurrentlyAm;
    public string areaName;
    public Transform portals;
    public Transform resps;

    void Start()
    {
        if (Player._instance.travellingController.whereIComeFrom != "")
        {
            int i = 0;

            for (i = 0; i < resps.childCount; i++)
            {
                if (resps.GetChild(i).name == Player._instance.travellingController.whereIComeFrom + "Resp")
                    break;
            }

            Player._instance.transform.localPosition = resps.GetChild(i).localPosition;
        }

        Player._instance.areaICurrentlyAm = this;

        Player._instance.interactables = GameObject.FindGameObjectsWithTag("interactable");

        Player._instance.travellingController.whereIComeFrom = areaName;
    }
}