using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bundle : InteractableObject
{
    public float timeToBeDestroyed;
    float timer;

    void Start()
    {
        GameObject[] array = new GameObject[Player._instance.interactables.Length + 1];

        for (int i = 0; i < Player._instance.interactables.Length; i++)
        {
            array[i] = Player._instance.interactables[i];
        }

        array[Player._instance.interactables.Length] = gameObject;

        Player._instance.interactables = array;

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