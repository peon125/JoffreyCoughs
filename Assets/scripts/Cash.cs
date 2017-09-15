using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cash : Item 
{

    public Cash(int value)
    {
        namesOfStats = new string[0];
        stats = new float[0];
        statsNames = new string[0];
        slot = "---";
        itemName = "cash";
        usable = true;

        this.value = value;
    }

    public override void Use()
    { 
        Player._instance.cash += value;
        Player._instance.items.Remove(this);
    }
}