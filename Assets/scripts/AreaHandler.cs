using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaHandler : MonoBehaviour
{
    public static AreaHandler areaWhereICurrentlyAm;
    public string areaName;
    public Transform resps;
    public AudioClip soundtrack;

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

            if(i >= resps.childCount)
            {
                for (i = 0; i < resps.childCount; i++)
                {
                    if (resps.GetChild(i).name == "defaultResp")
                        break;
                }
            }

            Player._instance.transform.localPosition = resps.GetChild(i).localPosition;
        }

        Player._instance.EndOfTravel(Player._instance.travellingController.whereIComeFrom, areaName);

        Player._instance.areaICurrentlyAm = this;

        Player._instance.interactables.Clear();
        GameObject[] array = GameObject.FindGameObjectsWithTag("interactable");
        foreach (GameObject go in array)
            Player._instance.interactables.Add(go);

        Player._instance.travellingController.whereIComeFrom = areaName;

        if (soundtrack != null)
            AudioController._instance.PlaySoundtrack(
                soundtrack);
    }
}