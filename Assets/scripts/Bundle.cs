using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bundle : InteractableObject
{
    public float timeToBeDestroyed;
    float timer;

    void Start()
    {
        Player._instance.interactables.Add(gameObject);

        transform.localPosition += new Vector3(0, 0, 0.6f);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeToBeDestroyed)
            Destroy(gameObject);
    }

    public override void Interact()
    {
        Debug.Log(this.name + " bundle");
    }

    new public void Death()
    {
        gameObject.SetActive(false);
    }
}