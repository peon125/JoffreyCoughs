using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bundle : InteractableObject
{
    public float timeToBeDestroyed;
    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeToBeDestroyed)
            Destroy(gameObject);
    }
}