using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public List<Item> items;
    public Sprite speakerSprite;
    public string thingToSay;
    public float sellingPriceModifier, buyingPriceModifier;
    public bool isBusy;

    public string _name, description;
    public bool randomizeAfterSleep;
    public int cash;
    public GameObject heartPrefab;
    public Transform heartsSpawn;

    public Gun gun;
    public int hp, maxNumberOfItems;

    public void DamageTaken()
    {
        Destroy(heartsSpawn.GetChild(0).gameObject);
        hp--;

        if (hp <= 0)
        {
            Player._instance.shootingController.ShootoutOver(gameObject);
        }
    }

    public void LetsShoot()
    {
        Player._instance.shootingController.StartShooting(this);
    }

    public void LetsTalk()
    {
        Quest quest = Player._instance.questsController.FindMyQuest(this);

        if (quest != null)
        {
            quest.CheckOnQuest(this);
        }

        Player._instance.talkingController.StartTalking(thingToSay);
    }

    public void LetsTrade()
    {
        Player._instance.tradingController.StartTrading();
    }

    public void LetsSee()
    {
        Player._instance.inspectingController.StartInspecting();
    }
    
    public void PrepareToShootout()
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
}
