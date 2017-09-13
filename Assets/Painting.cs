using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painting : InteractableObject
{
    public override void Interact()
    {
        Player._instance.talkingController.StartTalking("*knock*");
    }

    public override void Death()
    {
        gameObject.SetActive(false);
    }
}
