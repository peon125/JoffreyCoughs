using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject playerToResp;

    void Awake()
    {
        if (Player._instance == null)
        {
            Instantiate(playerToResp);

            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

}
