using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{
    public Vector3 startPos, startScale, positionAfterOpen, scaleAfterOpen;
    public bool hasKeyInside;

    public override void Interact()
    {
        if (transform.localScale == startScale && !notInteractable)
        {
            if (hasKeyInside)
            {
                AudioController._instance.soundSource.clip = StaticValues._instance.doorOpeningSound;
                AudioController._instance.soundSource.Play();
                transform.localPosition = positionAfterOpen;
                transform.localScale = scaleAfterOpen;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180 - transform.eulerAngles.y, transform.eulerAngles.z);
            } else
            {
                AudioController._instance.soundSource.clip = StaticValues._instance.doorKnockingSound;
                AudioController._instance.soundSource.Play();
                Player._instance.talkingController.StartTalking("*knock knoc* >>you need to insert the proper key<<");
            }
        }
        else if (transform.localScale == scaleAfterOpen)
        {
            AudioController._instance.soundSource.clip = StaticValues._instance.doorClosingSound;
            AudioController._instance.soundSource.Play();
            hasKeyInside = true;
            transform.localPosition = startPos;
            transform.localScale = startScale;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180 - transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }

    new public void Death()
    {

    }
}