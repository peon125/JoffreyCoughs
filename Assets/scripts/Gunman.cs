using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Person 
{
    void Update()
    {
        if (!isBusy && doMove)
        {
            moveTimer += Time.deltaTime;

            if (moveTimer >= frequencyOfChangeDirection)
            {
                moveTimer = 0f;

                Move();
            }
        }
        else
            rb.velocity = Vector2.zero;

        if (iAmBeingMarkedTimer > 0)
        {
            iAmBeingMarkedTimer -= Time.deltaTime;
            OhCrapAmIMarked(true);
        }
        else
        {
            OhCrapAmIMarked(false);
        }
    }

}