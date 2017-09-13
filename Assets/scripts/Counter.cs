using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour 
{
    public ShootingController shootingController;
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
        if (Input.anyKeyDown && timer - 1 > 0)
            timer -= 1;

        timer -= Time.deltaTime;

        counter.text = ((int)timer).ToString();

        if (timer <= 0)
        {
            foreach (GameObject go in stuffToDisable)
                go.SetActive(true);

            shootingController.PermissionWasGiven();

            timer = waitTime;

            gameObject.SetActive(false);
        }
    }
}