using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item
{
    public int restoreHp;
	
    public override void Use()
    { 
        for (int i = 0; i < restoreHp && Player._instance.hp < Player._instance.maxHp; i++)
        {
            Player._instance.hp++;
        }

        Player._instance.items.Remove(this);
    }
}