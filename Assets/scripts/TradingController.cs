using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradingController : UiElement 
{
    public GameObject itemListElementPrefab;
    public Image spriteRenderer;
    public Item[] allTradersItems, allPlayersItems;
    public Text tradersCash, playersCash, tradersItemCounter, playersItemCounter;
    Item selectedTradersItem, selectedPlayersItem;
    int left = 0, right = 0, leftShift = 0, rightShift = 0;
    bool leftVerticalAxisInUse = false, rightVerticalAxisInUse = false, leftHorizontalAxisInUse = false, rightHorizontalAxisInUse = false;

    public Transform tradersList, playersList, tradersStats, playersStats;

    //Vector3 traderStartingPos, playerStartingPos;
    public float diffrence;

    void Start()
    {
        //traderStartingPos = tradersList.transform.localPosition;
        //playerStartingPos = playersList.transform.localPosition;
    }

    void Update()
    {
        MoveObjects(0, 1, stuff[1], way, yStart, yEnd, timeToWait);

        if (opened && way == 0)
        {
            if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Trade"))
            {
                StartCoroutine(CloseDialogue(stuff[1].transform));
            }
        }

        if (opened)
            SelectingObjects();
    }

    void SelectingObjects()
    {
        if (Input.GetAxisRaw("Horizontal1") != 0)
        {
            if (!leftHorizontalAxisInUse)
            {
                if (Input.GetAxisRaw("Horizontal1") > 0)
                {
                    Trade(target, Player._instance, left, leftShift, target.sellingPriceModifier, Player._instance.buyingPriceModifier);
                    //BuyItem();
                }

                leftHorizontalAxisInUse = true;
            }
        }
        else
            leftHorizontalAxisInUse = false;
        
        if (Input.GetAxisRaw("Vertical1") != 0)
        {
            if (!leftVerticalAxisInUse)
            {
                if (Input.GetAxisRaw("Vertical1") > 0)
                {
                    left--;

                    ScrollAList(tradersList, target.items.ToArray(), ref left, 1, ref leftShift);
                }
                else if (Input.GetAxisRaw("Vertical1") < 0)
                {
                    left++;

                    ScrollAList(tradersList, target.items.ToArray(), ref left, -1, ref leftShift);
                }

                leftVerticalAxisInUse = true;
            }
        }
        else
            leftVerticalAxisInUse = false;


        if (Input.GetAxisRaw("Horizontal2") != 0)
        {
            if (!rightHorizontalAxisInUse)
            {
                if (Input.GetAxisRaw("Horizontal2") < 0)
                {
                    Trade(Player._instance, target, right, rightShift, Player._instance.sellingPriceModifier, target.buyingPriceModifier);
                    //SellItem();
                }

                rightHorizontalAxisInUse = true;
            }
        }
        else
            rightHorizontalAxisInUse = false;

        if (Input.GetAxisRaw("Vertical2") != 0)
        {
            if (!rightVerticalAxisInUse)
            {
                if (Input.GetAxisRaw("Vertical2") > 0)
                {
                    right--;

                    ScrollAList(playersList, Player._instance.items.ToArray(), ref right, 1, ref rightShift);
                }
                else if (Input.GetAxisRaw("Vertical2") < 0)
                {
                    right++;

                    ScrollAList(playersList, Player._instance.items.ToArray(), ref right, -1, ref rightShift);
                }

                rightVerticalAxisInUse = true;
            }
        }
        else
            rightVerticalAxisInUse = false;

        if (leftHorizontalAxisInUse || leftVerticalAxisInUse || rightHorizontalAxisInUse || rightVerticalAxisInUse || leftHorizontalAxisInUse)
        {
            TakeCareOfIteratorAndShowStats();
        }

        for (int i = 0; i < tradersList.childCount; i++)
        {
            if (i == left)
                tradersList.GetChild(i).GetComponent<Text>().color = selectedColor;
            else
                tradersList.GetChild(i).GetComponent<Text>().color = unselectedColor;
        }


        for (int i = 0; i < playersList.childCount; i++)
        {
            if (i == right)
                playersList.GetChild(i).GetComponent<Text>().color = selectedColor;
            else
                playersList.GetChild(i).GetComponent<Text>().color = unselectedColor;
        }
    }

    void ViewStats(Item item, Transform transform, string[] names, float[] values, string slot, int value, float sellerModifier, float buyerModifier)
    {
        if (item != null)
        {
            transform.GetChild(4).GetChild(0).GetComponent<Text>().text = item.slot;
            transform.GetChild(5).GetChild(0).GetComponent<Text>().text = (item.value * sellerModifier * buyerModifier).ToString();

            for (int i = 0; i < names.Length; i++)
            {
                transform.GetChild(i).GetComponent<Text>().text = item.namesOfStats[i];
                transform.GetChild(i).GetChild(0).GetComponent<Text>().text = item.stats[i].ToString();
            }
        }

        for (int i = names.Length; i < transform.childCount - 2; i++)
        {
            transform.GetChild(i).GetComponent<Text>().text = "----";
            transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "----";
        }
    }

    public void StartTrading()
    {
        StartCoroutine(OpenDialogue(true));
        target = Player._instance.target;
        spriteRenderer.sprite = target.speakerSprite;

        ShowItems();
    }

    void TakeCareOfIteratorAndShowStats()
    {
        left += leftShift;

        if (target.items.Count <= tradersList.childCount)
            left %= tradersList.childCount;

        tradersItemCounter.text = (left + 1).ToString() + "/" + target.items.Count.ToString();

        if (target.items.Count != 0)    
            ViewStats(
                target.items[left],
                tradersStats, 
                target.items[left].namesOfStats, 
                target.items[left].stats,
                target.items[left].slot, 
                target.items[left].value, 
                target.sellingPriceModifier,
                Player._instance.buyingPriceModifier
            );
        else
            ViewStats(
                null,
                tradersStats, 
                new string[0], 
                new float[0],
                "----", 
                0, 
                0,
                0
            );

        left -= leftShift;

        //---------------------------------------

        right += rightShift;

        if (Player._instance.items.Count <= playersList.childCount)
            right %= playersList.childCount;

        playersItemCounter.text = (right + 1) + "/" + Player._instance.items.Count;

        if (Player._instance.items.Count != 0)
            ViewStats(
                Player._instance.items[right], 
                playersStats, 
                Player._instance.items[right].namesOfStats, 
                Player._instance.items[right].stats,
                Player._instance.items[right].slot,
                Player._instance.items[right].value,
                Player._instance.buyingPriceModifier,
                target.sellingPriceModifier
            );
        else
            ViewStats(
                null,
                playersStats, 
                new string[0], 
                new float[0],
                "----", 
                0, 
                0,
                0
            );

        right -= rightShift;
    }

    void ShowItems()
    {       
        UnshowItems();

        for (int i = 0; i < tradersList.childCount && i < target.items.Count; i++)
        {
            tradersList.GetChild(i).GetComponent<Text>().text = target.items[i].itemName;
            if (tradersList.GetChild(i).GetComponent<Text>().text.Length > 20)
                tradersList.GetChild(i).GetComponent<Text>().text = tradersList.GetChild(i).GetComponent<Text>().text.Substring(0, 10) + "...";
        }

        for (int i = target.items.Count; i < tradersList.childCount; i++)
        {
            tradersList.GetChild(i).GetComponent<Text>().text = "---";
        }

        leftShift = 0;
        left = 0;

        for (int i = 0; i < playersList.childCount && i < Player._instance.items.Count; i++)
        {
            playersList.GetChild(i).GetComponent<Text>().text = Player._instance.items[i].itemName;
            if (playersList.GetChild(i).GetComponent<Text>().text.Length > 20)
                playersList.GetChild(i).GetComponent<Text>().text = playersList.GetChild(i).GetComponent<Text>().text.Substring(0, 10) + "...";
        }

        for (int i = Player._instance.items.Count; i < playersList.childCount; i++)
        {
            playersList.GetChild(i).GetComponent<Text>().text = "---";
        }

        rightShift = 0;
        right = 0;

        tradersCash.text = target.cash.ToString();
        playersCash.text = Player._instance.cash.ToString();

        TakeCareOfIteratorAndShowStats();
    }

    void UnshowItems()
    {
        foreach (Transform item in tradersList)
        {
            item.GetComponent<Text>().text = "---";
        }

        foreach (Transform item in playersList)
        {
            item.GetComponent<Text>().text = "---";
        }
    }

    void Trade(InteractableObject seller, InteractableObject buyer, int sellerIterator, int selletIteratorShift, float sellerModifier, float buyerModifier)
    {
        sellerIterator += selletIteratorShift;

        if (buyer.items.Count == buyer.maxNumberOfItems || seller.items.Count == 0)
            return;

        if (buyer.cash - seller.items[sellerIterator].value * sellerModifier * buyerModifier >= 0 && seller.cash + seller.items[sellerIterator].value * sellerModifier * buyerModifier <= 99999999)
        {
            buyer.cash -= Mathf.FloorToInt(seller.items[sellerIterator].value * sellerModifier * buyerModifier);
            seller.cash += Mathf.FloorToInt(seller.items[sellerIterator].value * sellerModifier * buyerModifier);
        }
        else
            return;

        buyer.items.Add(seller.items[sellerIterator]);
        seller.items.Remove(seller.items[sellerIterator]);

        ShowItems();
    }
}