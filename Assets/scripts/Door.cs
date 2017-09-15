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
                transform.localPosition = positionAfterOpen;
                transform.localScale = scaleAfterOpen;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180 - transform.eulerAngles.y, transform.eulerAngles.z);
            } else
            {
                Player._instance.talkingController.StartTalking("*knock knoc* >>you need to insert the proper key<<");
            }
        }
        else if (transform.localScale == scaleAfterOpen)
        {
            hasKeyInside = true;
            transform.localPosition = startPos;
            transform.localScale = startScale;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180 - transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }

    void Update()
    {
        //if (hasKeyInside)
        //    notInteractable = true;
    }

    new public void Death()
    {

    }
}