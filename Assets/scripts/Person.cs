using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour 
{
    public string _name, description;
    public List<Item> items;
    public int cash;
    public GameObject heartPrefab, markPrefab;
    public Transform heartsSpawn;
    public Sprite speakerSprite;
    public Gun gun;
    public int hp;
    public float distanceFromOrigin, speed, frequencyOfChangeDirection;
    public bool isBusy;
    public SpriteRenderer spriteRenderer;
    public string thingToSay;
    public Sprite[] moveSprites;
    public float moveSpriteSpeed;
    public int tradePricesModifier;
    float moveSpriteTimer = 0f, _moveSpriteSpeed;
    int moveI = 0;

    protected Vector3 previousPosition = Vector3.zero;

    protected Vector3 startPos;
    protected float moveTimer = 0f;
    protected Rigidbody2D rb;
    protected bool doMove = true;
    protected GameObject mark;

    void Start() 
    {
        _moveSpriteSpeed = moveSpriteSpeed;
        startPos = transform.localPosition;
        rb = GetComponent<Rigidbody2D>();
    }

    protected void ChangeSprite()
    {
        if (previousPosition.x != transform.position.x)
        {
            moveSpriteTimer += Time.deltaTime;

            _moveSpriteSpeed = Mathf.Abs(rb.velocity.x) / moveSpriteSpeed;

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

    public  void PrepareToShootout()
    {
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

    public void DamageTaken()
    {
        Destroy(heartsSpawn.GetChild(0).gameObject);
        hp--;

        if (hp <= 0)
        {
            Player._instance.shootingController.ShootoutOver(gameObject);
        }
    }
}