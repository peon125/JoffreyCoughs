using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : InteractableObject
{
    public override void Interact()
    {
        Player._instance.talkingController.StartTalking("*swish*");
    }

    public override void Death()
    {
        gameObject.SetActive(false);
    }
}
