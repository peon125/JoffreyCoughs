using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour 
{
    public string[] namesOfStats;
    public float[] stats;
    public static string[] statsNames;
    public string itemName;
    public string slot;
    public int value;
    public bool usable;

    public abstract void Use();
}