using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{
    public Vector3 startPos, startScale, positionAfterOpen, scaleAfterOpen;
    public bool unlocked;
    bool hasKeyInside = true;

    public override void Interact()
    {
        if (hasKeyInside)
            unlocked = true;

        if (transform.localScale == startScale && unlocked)
        {
            transform.localPosition = positionAfterOpen;
            transform.localScale = scaleAfterOpen;
        }
        else if (transform.localScale == scaleAfterOpen)
        {
            transform.localPosition = startPos;
            transform.localScale = startScale;
        }
    }
}