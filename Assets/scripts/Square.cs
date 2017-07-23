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
        } while(Player._instance.shootingController.takenKeys.Contains(expectedKey));

        Player._instance.shootingController.takenKeys.Add(expectedKey);
        key.text = expectedKey.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(expectedKey))
            ShotHit(Player._instance.gun.damage);
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
        float chance = Random.Range(0f, 1f);

        if (chance > Player._instance.gun.accuracy)
            return;

        hp -= damage;

        hpCounter.text = hp.ToString();

        if (hp <= 0)
        {
            DestroyTheSquare();
            Player._instance.shootingController.shootingEnemy.DamageTaken();
        }
    }

    void DestroyTheSquare()
    {
        Player._instance.shootingController.takenKeys.Remove(expectedKey);
        Player._instance.shootingController.SquareDestroyed(transform);
        Destroy(gameObject);
    }

    void DealDamage()
    {
        Player._instance.DamageTaken();
    }
}