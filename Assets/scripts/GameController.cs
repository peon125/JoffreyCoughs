using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController _instance;
    public GameObject playerToResp;

    void Awake()
    {
        if (Player._instance == null)
        {
            _instance = this;

            Instantiate(playerToResp);

            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
