using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour 
{
    public Text hpCounter, key;
    public RectTransform imageTimer;
    public int maxHp;
    public float howLongLasts;
    KeyCode expectedKey;
    int hp = 1;
    float timer = 0f;

    void Start()
    {
        hp = Random.Range(1, maxHp);
        hpCounter.text = hp.ToString();
        timer = howLongLasts;

        do
        {
            expectedKey = Keys.RandomKey();
        } while(ShootingController._instance.takenKeys.Contains(expectedKey));

        ShootingController._instance.takenKeys.Add(expectedKey);
        key.text = expectedKey.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(expectedKey))
            ShotHit(1);
        else if (Input.anyKeyDown)
            DealDamage();

        timer -= Time.deltaTime;

        imageTimer.sizeDelta = new Vector2(imageTimer.sizeDelta.x, (timer / howLongLasts) * 200);

        if (timer <= 0)
        {
            DealDamage();
            DestroyTheSquare();
        }
    }
	
    public void ShotHit(int damage)
    {
        hp -= damage;

        hpCounter.text = hp.ToString();

        if (hp <= 0)
        {
            DestroyTheSquare();
            ShootingController._instance.shootingEnemy.DamageTaken();
        }
    }

    void DestroyTheSquare()
    {
        ShootingController._instance.takenKeys.Remove(expectedKey);
        ShootingController._instance.SquareDestroyed(transform);
        Destroy(gameObject);
    }

    void DealDamage()
    {
        Player._instance.DamageTaken();
    }
}