﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : InteractableObject 
{
    public float distanceFromOrigin, speed, frequencyOfChangeDirection;
    public bool canMove;
    public SpriteRenderer spriteRenderer;

    public Sprite[] moveSprites;
    public float moveSpriteSpeed;
    public bool chasesThePlayer;
    public float chasingThePlayerDistance;
    public bool randomVelocity;
    protected float moveSpriteTimer = 0f;
    protected float _moveSpriteSpeed;
    protected int moveI = 0;

    protected Vector3 previousPosition = Vector3.zero;

    protected Vector3 startPos;
    protected float moveTimer = 0f;
    protected Rigidbody2D rb;
    protected bool doMove = true;
    protected GameObject mark;

    protected void Start() 
    {
        base.Start();
        _moveSpriteSpeed = moveSpriteSpeed;
        startPos = transform.localPosition;
        rb = GetComponent<Rigidbody2D>();
    }

    protected void Update()
    {
        base.Update();
    }

    protected void ChasingThePlayer()
    {
        if (!chasesThePlayer)
            return;

        bool b = Vector3.Distance(
         new Vector3(transform.position.x, transform.position.y, 0),
         new Vector3(Player._instance.transform.position.x, Player._instance.transform.position.y, 0)
         ) <= chasingThePlayerDistance;

        if (b)
        {
            randomVelocity = false;

            Vector3 move = (Player._instance.transform.position - transform.position).normalized;

            rb.velocity = move * speed;
        }
        else
        {
            randomVelocity = true;
        }
    }

    protected void ChangeSprite()
    {
        if (canMove)
        {
            if (previousPosition.x != transform.position.x)
            {
                moveSpriteTimer += Time.deltaTime;

                _moveSpriteSpeed = 
                Mathf.Abs(rb.velocity.x) /
                moveSpriteSpeed;

                if (moveSpriteTimer > _moveSpriteSpeed)
                {
                    moveI++;
                    moveI %= moveSprites.Length;

                    GetComponent<SpriteRenderer>().sprite = moveSprites[moveI];

                    moveSpriteTimer = 0f;

                    if (previousPosition.x - transform.position.x < 0)
                        transform.eulerAngles = new Vector3(0, 0, 0);
                    else if (previousPosition.x - transform.position.x > 0)
                        transform.eulerAngles = new Vector3(0, 180, 0);

                    previousPosition = transform.position;
                }
            }
        }
    }

    protected void Move()
    {
        if (randomVelocity)
        {
            float x = 0, y = 0;

            x = Random.Range(-1f, 1f);
            y = Random.Range(-1f, 1f);

            if ((Vector3.Distance(startPos, transform.localPosition + new Vector3(x, y, 0) * speed) < distanceFromOrigin))
            {
                rb.velocity = new Vector2(
                    x,
                    y
                ) * speed;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            doMove = false;
            rb.velocity = Vector2.zero;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.name == "player")
            doMove = true;
    }    

    public void IAmGivingQuest<T>(bool b)
    {
        if (b)
            GetComponent<SpriteRenderer>().color = Color.yellow;
        else
            GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void IAmReceivingQuest<T>(bool b)
    {
        if (b)
            GetComponent<SpriteRenderer>().color = Color.blue;
        else 
            GetComponent<SpriteRenderer>().color = Color.white;
    }

    public override void Interact()
    {
        Player._instance.talkingController.StartTalking("Leave me alone!");
    }
}