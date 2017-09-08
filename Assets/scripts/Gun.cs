using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Item 
{
    public int hands, damage;
    public float speed, accuracy;

    public override void Use()
    { 
        Player._instance.items.Remove(this);
        Player._instance.items.Add(Player._instance.gun);
        Player._instance.gun = this;
    }
}