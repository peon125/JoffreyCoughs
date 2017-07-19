using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour 
{
    public static Player _instance;
    public Talking talking;
    public Gun gun;
    public GameObject heartPrefab, arrow;
    public Transform heartsSpawn;
    public int hp;
    public bool isBusy;
    public SpriteRenderer spriteRenderer;
    public Person target;

    public float x = 0, y = 0;
    public float speed;
    Vector2 startPosDrag, currentPosDrag, outcomePosDrag;
    //bool moving = false;
    //float arrowTimer = 0f;
    Rigidbody2D rb; 
    Vector3 arrowOffset = new Vector3(0, 2.8f, 0);

    void Awake()
    {
        _instance = this; 
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

	void Update()
    {
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
            rb.velocity = new Vector2(
                Input.GetAxis("Horizontal") * speed,
                Input.GetAxis("Vertical") * speed
            );

            ShowArrow(Input.GetButton("Aim"));

            if (Input.GetButtonDown("Challenge") && target != null)
                ShootingController._instance.ShallWeBegin(target);

            if (Input.GetButtonDown("Talk") && !isBusy)
                talking.Says("uga buga uga buga uga buga uga buga uga buga uga buga uga buga uga buga uga buga uga buga uga buga uga buga uga buga uga buga uga buga");
        }
        else
            rb.velocity = Vector2.zero;
	}   

    public void PrepareToShootout()
    {
        //spriteRenderer.enabled = false;

        for (int i = 0; i < hp; i++)
        {
            Vector2 pos = new Vector2(
                              i * (heartPrefab.GetComponent<RectTransform>().sizeDelta.y + 0),
                              0               
                          );

            GameObject heart = Instantiate(heartPrefab, heartsSpawn) as GameObject;

            heart.transform.localPosition = pos;
        }

        isBusy = true;
    }

    public void ShootoutOver()
    {
        isBusy = false;

        foreach (Transform heart in heartsSpawn)
            Destroy(heart.gameObject);
    }

    void ShowArrow(bool b)
    {
        arrow.SetActive(b);

        //arrowTimer += Time.deltaTime;

        int h = 0, v = 0;
        if (Input.GetAxis("Horizontal") > 0)
            h = 1;
        else if (Input.GetAxis("Horizontal") < 0)
            h = -1;

        if (Input.GetAxis("Vertical") > 0)
            v = 1;
        else if (Input.GetAxis("Vertical") < 0)
            v = -1;

        if (h != 0 || v != 0)
            arrowOffset = new Vector3(h * 2.8f, v * 2.8f, 0);

        if (Input.GetAxis("Horizontal") > 0 && Input.GetAxis("Vertical") > 0)
            arrow.transform.eulerAngles = new Vector3(0, 0, -45);
        else if (Input.GetAxis("Horizontal") > 0 && Input.GetAxis("Vertical") < 0)
            arrow.transform.eulerAngles = new Vector3(0, 0, -135);
        else if (Input.GetAxis("Horizontal") < 0 && Input.GetAxis("Vertical") < 0)
            arrow.transform.eulerAngles = new Vector3(0, 0, -225);
        else if (Input.GetAxis("Horizontal") < 0 && Input.GetAxis("Vertical") > 0)
            arrow.transform.eulerAngles = new Vector3(0, 0, -315);
        else if (Input.GetAxis("Horizontal") > 0 && Input.GetAxis("Vertical") == 0)
            arrow.transform.eulerAngles = new Vector3(0, 0, 270);
        else if (Input.GetAxis("Horizontal") < 0 && Input.GetAxis("Vertical") == 0)
            arrow.transform.eulerAngles = new Vector3(0, 0, 90);
        else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") > 0)
            arrow.transform.eulerAngles = new Vector3(0, 0, 0);
        else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") < 0)
            arrow.transform.eulerAngles = new Vector3(0, 0, 180);

        if (b)
        {

            arrow.transform.position = transform.position + arrowOffset;
        }
    }

    public void DamageTaken()
    {
        Destroy(heartsSpawn.GetChild(heartsSpawn.childCount - 1).gameObject);
        hp--;

        if (hp <= 0)
        {
            //UnityEngine.SceneManagement.SceneManager.LoadScene(Application.loadedLevel);

            ShootingController._instance.ShootoutOver(gameObject);
        }
    }


}