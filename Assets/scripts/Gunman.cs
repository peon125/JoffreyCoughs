using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunman : Person 
{
    void Update()
    {
        ChasingThePlayer();

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
        
        if (Player._instance.isBusy)
        {
            rb.velocity = Vector3.zero;
        }

        transform.localPosition = new Vector3(
            transform.localPosition.x,
            transform.localPosition.y,
            transform.localPosition.y 
        );
    }
}