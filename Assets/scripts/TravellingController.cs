using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TravellingController : MonoBehaviour
{
    public string whereIComeFrom;

    public void Travel(string whereToGo)
    {
        //whereIComeFrom = Player._instance.areaICurrentlyAm.areaName;

        SceneManager.LoadScene(whereToGo);
    }
}