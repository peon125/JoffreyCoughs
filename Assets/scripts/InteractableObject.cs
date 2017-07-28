using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public List<Item> items;
    public Sprite speakerSprite;
    public string thingToSay;
    public float tradePricesModifier;

    public string _name, description;
    public bool randomizeAfterSleep;
    public int cash;
    public GameObject heartPrefab;
    public Transform heartsSpawn;

    public Gun gun;
    public int hp, maxNumberOfItems;

}
