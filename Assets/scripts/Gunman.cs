using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunman : Person 
{
    void Update()
    {
        if (!isBusy && doMove)
        {
            ChangeSprite();

            moveTimer += Time.deltaTime;

            if (moveTimer >= frequencyOfChangeDirection)
            {
                moveTimer = 0f;

                Move();
            }
        }
        else
            rb.velocity = Vector2.zero;
        
        if (isBusy)
        {
            rb.velocity = Vector3.zero;
        }

        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.y / 10000
        );
    }
}