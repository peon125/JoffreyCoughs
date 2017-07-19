using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour 
{
    public GameObject heartPrefab;
    public Transform heartsSpawn;
    public Gun gun;
    public int hp;
    public float distanceFromOrigin, speed, frequencyOfChangeDirection;
    public bool isBusy;
    public SpriteRenderer spriteRenderer;
    public float iAmBeingMarkedTimer = 0f;

    protected Vector3 startPos;
    protected float moveTimer = 0f;
    protected Rigidbody2D rb;
    protected bool doMove = true;

    void Start() 
    {
        startPos = transform.localPosition;
        rb = GetComponent<Rigidbody2D>();
    }

    public  void PrepareToShootout()
    {
        //spriteRenderer.enabled = false;

        for (int i = 0; i < hp; i++)
        {
            Vector2 pos = new Vector2(
                -i * (heartPrefab.GetComponent<RectTransform>().sizeDelta.y + 0),
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

    protected void Move()
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

    public void OhCrapAmIMarked(bool b)
    {
        if (b)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            Player._instance.target = this;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            if (Player._instance.target == this)
                Player._instance.target = null;

        }
    }


    public void DamageTaken()
    {
        Destroy(heartsSpawn.GetChild(0).gameObject);
        hp--;

        if (hp <= 0)
        {
            ShootingController._instance.ShootoutOver(gameObject);
        }
    }
}