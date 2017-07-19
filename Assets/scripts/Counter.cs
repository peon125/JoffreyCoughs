using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour 
{
    public GameObject[] stuffToDisable;
    public Text counter;
    public float waitTime;
    float timer;

    void Awake()
    {
        foreach (GameObject go in stuffToDisable)
            go.SetActive(false);

        timer = waitTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        counter.text = ((int)timer).ToString();

        if (timer <= 0)
        {
            foreach (GameObject go in stuffToDisable)
                go.SetActive(true);

            ShootingController._instance.PermissionWasGiven();

            timer = waitTime;

            gameObject.SetActive(false);
        }
    }
}