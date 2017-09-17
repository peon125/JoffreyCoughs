using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : InteractableObject
{
    public override void Interact()
    {
        Player._instance.talkingController.StartTalking("*knock knock*");
        AudioController._instance.soundSource.clip = StaticValues._instance.doorKnockingSound;
        AudioController._instance.soundSource.Play();
    }

    void Start ()
    {
        base.Start();
	}

	void Update ()
    {
        base.Update();
	}
}
