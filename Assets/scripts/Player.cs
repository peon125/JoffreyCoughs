﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour 
{
    public static Player _instance;

    public TradingController tradingController;
    public InspectingController inspectingController;
    public TalkingController talkingController;
    public ShootingController shootingController;

    public List<Item> items;
    public Gun gun;
    public int cash;

    public Transform enemies;
    public float radius;
    public bool isBusy;
    public Person target;
    public float speed;

    public Sprite[] moveSprites;
    public float moveSpriteSpeed;
    float moveSpriteTimer = 0f;
    int moveI = 0;

    public Transform heartsSpawn;
    public GameObject heartPrefab;
    public int hp;

    public Transform feedTransform;
    public GameObject feedPrefab;
    public int feed;
    public float feedTime;
    float feedTimer;

    List<Transform> nearbyObjects = new List<Transform>();
    Vector2 startPosDrag, currentPosDrag, outcomePosDrag;
    Rigidbody2D rb; 

    void Awake()
    {
        _instance = this; 
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        for (int i = 0; i < hp; i++)
        {
            heartsSpawn.GetChild(i).gameObject.SetActive(true);
        }

        for (int i = 0; i < feed; i++)
        {
            feedTransform.GetChild(i).gameObject.SetActive(true);
        }
    }

	void Update()
    {
        Feed();

//        if ((Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) || moving)
//        {
//            timer += Time.deltaTime;
//
//            if (Input.GetAxis("Horizontal") > 0)
//                x = 1;
//            else if (Input.GetAxis("Horizontal") < 0)
//                x = -1;
//
//            if (Input.GetAxis("Vertical") > 0)
//                y = 1;
//            else if (Input.GetAxis("Vertical") < 0)
//                y = -1;
//
//            if (timer - secondTimer > 0.25f)
//            {
//                transform.localPosition += new Vector3(
//                    x * speed,
//                    y * speed,
//                    0
//                );
//
//                secondTimer = timer;
//            }
//        }
//        else
//        {
//            timer = 1f;
//            secondTimer = 0f;
//            x = 0;
//            y = 0;
//        }

        if (!isBusy)
        {
            ChangeSprite();

            rb.velocity = new Vector2(
                Input.GetAxis("Horizontal1") * speed,
                Input.GetAxis("Vertical1") * speed
            );

            if (!isBusy)
                LookingForTheNearestInteractiveObject();

            if (target != null)
            {
                if (Input.GetButtonDown("Challenge"))
                    shootingController.ShallWeBegin(target);

                if (Input.GetButtonDown("Talk"))
                    talkingController.LetsTalk(target.thingToSay);

                if (Input.GetButtonDown("Trade"))
                    tradingController.LetsTrade();

                if (Input.GetButtonDown("Inspect"))
                    inspectingController.LetsSee();
            }
        }
        else
            rb.velocity = Vector2.zero;

        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.y / 10000
        );
	} 

    void Feed()
    {
        feedTimer += Time.deltaTime;

        if (feedTimer >= feedTime)
        {
            if (feed > 0)
            {
                feed--;

                feedTimer = 0f;

                feedTransform.GetChild(feedTransform.childCount - 1).gameObject.SetActive(false);
            }
            else
                GoToHospital();
        }
    }

    public void EatFood(int _i)
    {
        if (feed < 10)
        {
            feed += _i;

            for (int i = 0; i < _i; i++)
            {
                feedTransform.GetChild(feedTransform.childCount).gameObject.SetActive(true);
            }
        }
    }

    public void DrinkAlkohol(int _i)
    {
        if (hp < 10)
        {
            hp += _i;

            for (int i = 0; i < _i; i++)
            {
                heartsSpawn.GetChild(heartsSpawn.childCount).gameObject.SetActive(true);
            }
        }
    }

    void GoToHospital()
    {
        Start();
    }

    void ChangeSprite()
    {
        if (Input.GetAxis("Horizontal1") != 0 || Input.GetAxis("Vertical1") != 0)
        {
            moveSpriteTimer += Time.deltaTime;

            if (moveSpriteTimer > moveSpriteSpeed)
            {
                moveI++;
                moveI %= moveSprites.Length;

                GetComponent<SpriteRenderer>().sprite = moveSprites[moveI];

                moveSpriteTimer = 0f;

                if (Input.GetAxis("Horizontal1") > 0)
                    transform.eulerAngles = new Vector3(0, 0, 0);
                else if (Input.GetAxis("Horizontal1") < 0)
                    transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
    }

    public void PrepareToShootout()
    {
        isBusy = true;
    }

    void LookingForTheNearestInteractiveObject()
    {
        foreach (Transform enemy in enemies)
        {
            if (Vector3.Distance(enemy.position, transform.position) < radius)
                nearbyObjects.Add(enemy);               
        }

        if (nearbyObjects.Count != 0)
        {
            Transform nearest = nearbyObjects[0];

            if (nearbyObjects.Count > 0)
            {
                for (int i = 1; i < nearbyObjects.Count; i++)
                {
                    if (Vector3.Distance(nearbyObjects[i].position, transform.position) < Vector3.Distance(nearest.position, transform.position))
                        nearest = nearbyObjects[i];
                }
            }

            if (target != nearest.GetComponent<Person>() && target != null)
            {
                target.GetComponent<SpriteRenderer>().color = Color.white;
               
            }
            target = nearest.GetComponent<Person>();
            target.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
            if (target != null)
            {
                target.GetComponent<SpriteRenderer>().color = Color.white;
                target = null;
            }
        }

        nearbyObjects.Clear();
    }

    public void ShootoutOver()
    {
        isBusy = false;

        foreach (Transform heart in heartsSpawn)
            Destroy(heart.gameObject);
    }

    public void DamageTaken()
    {
        Destroy(heartsSpawn.GetChild(heartsSpawn.childCount - 1).gameObject);
        hp--;

        if (hp <= 0)
        {
            shootingController.ShootoutOver(gameObject);
        }
    }
}