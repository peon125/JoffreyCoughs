﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Item 
{
    public int hands, damage;
    public float speed, accuracy;
    public AudioClip shotSound;

    public override void Use()
    { 
        Player._instance.items.Remove(this);
        if (Player._instance.gun != null)
            Player._instance.items.Add(Player._instance.gun);
        Player._instance.gun = this;
    }
}