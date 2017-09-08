using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{
    public Vector3 startPos, startScale, positionAfterOpen, scaleAfterOpen;
    bool hasKeyInside = true;

    public override void Interact()
    {
        if (transform.localScale == startScale && !notInteractable)
        {
            transform.localPosition = positionAfterOpen;
            transform.localScale = scaleAfterOpen;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180 - transform.eulerAngles.y, transform.eulerAngles.z);
        }
        else if (transform.localScale == scaleAfterOpen)
        {
            transform.localPosition = startPos;
            transform.localScale = startScale;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180 - transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }

    void Update()
    {
        if (hasKeyInside)
            notInteractable = false;
    }

}