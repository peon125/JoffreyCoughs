using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public List<Item> items;
    public int  maxNumberOfItems;
    public Sprite speakerSprite;
    public string thingToSay;
    public float sellingPriceModifier, buyingPriceModifier;
    public bool isBusy;

    public string _name, description;
    public bool randomizeAfterSleep;
    public int cash;
    //public GameObject heartPrefab;
    public Transform heartsSpawn;

    public Gun gun;
    public int hp, howCloseINeedToApproach;

    public bool notInteractable;

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
        if (Player._instance.gun != null)
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
                -i * (StaticValues._instance.enemyHeartPrefab.GetComponent<RectTransform>().sizeDelta.y + 0),
                0
            );

            GameObject heart = Instantiate(StaticValues._instance.enemyHeartPrefab, heartsSpawn) as GameObject;

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

    public override string ToString()
    {
        return _name;
    }

    public abstract void Interact();

    public void Death()
    {
        GameObject bundle = (GameObject)Instantiate(
                StaticValues._instance.bundlePrefab,
                transform.position,
                StaticValues._instance.bundlePrefab.transform.rotation
                );

        if (items.Count > 0)
        {
            bundle.GetComponent<Bundle>().items = items;
        }

        bundle.GetComponent<Bundle>().items.Add(new Cash(cash));

        Player._instance.interactables.Remove(gameObject);

        Destroy(gameObject);
    }
}
