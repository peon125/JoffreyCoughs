using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradingController : UiElement 
{
    public static TradingController _instance;
    public Image spriteRenderer;
    public Item[] allTradersItems, allPlayersItems;
    public Color selectedColor, unselectedColor;
    public Text tradersCash, playersCash;
    Item selectedTradersItem, selectedPlayersItem;
    int left = 0, right = 0;
    bool leftVerticalAxisInUse = false, rightVerticalAxisInUse = false, leftHorizontalAxisInUse = false, rightHorizontalAxisInUse = false;

    public Transform tradersList, playersList, tradersStats, playersStats;

    void Awake()
    {
        _instance = this;
    }

    void Update()
    {
        MoveObjects();

        if (opened && way == 0)
        {
            if (Input.GetButtonDown("Cancel"))
                StartCoroutine(CloseDialogue());
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
                    BuyItem();
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
                    if (left - 1 >= 0)
                        left--;
                    else
                        left = target.items.Count - 1;
                }
                else if (Input.GetAxisRaw("Vertical1") < 0)
                {
                    left++;
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
                    SellItem();
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
                    if (right - 1 >= 0)
                        right--;
                    else
                        right = Player._instance.items.Count - 1;
                }
                else if (Input.GetAxisRaw("Vertical2") < 0)
                {
                    right++;
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

            if (i == right)
                playersList.GetChild(i).GetComponent<Text>().color = selectedColor;
            else
                playersList.GetChild(i).GetComponent<Text>().color = unselectedColor;
        }
    }

    void ViewStats(Item item, Transform transform, string[] names, float[] values, string slot, int value, int modifier)
    {
        transform.GetChild(4).GetChild(0).GetComponent<Text>().text = item.slot;
        transform.GetChild(5).GetChild(0).GetComponent<Text>().text = (item.value * modifier).ToString();

        for (int i = 0; i < names.Length; i++)
        {
            transform.GetChild(i).GetComponent<Text>().text = item.namesOfStats[i];
            transform.GetChild(i).GetChild(0).GetComponent<Text>().text = item.stats[i].ToString();
        }

        for (int i = names.Length; i < transform.childCount - 2; i++)
        {
            transform.GetChild(i).GetComponent<Text>().text = "----";
            transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "----";
        }
    }

    public void LetsTrade()
    {
        StartCoroutine(OpenDialogue());
        target = Player._instance.target;
        spriteRenderer.sprite = target.speakerSprite;

        ShowItems();
    }

    void TakeCareOfIteratorAndShowStats()
    {
        if (target.items.Count != 0)
            left %= target.items.Count;
        else
            left = 8;
        if (target.items.Count != 0)
            ViewStats(
                target.items[left],
                tradersStats, 
                target.items[left].namesOfStats, 
                target.items[left].stats,
                target.items[left].slot, 
                target.items[left].value, 
                target.tradePricesModifier
            );
        else
            ViewStats(
                new Item(),
                tradersStats, 
                new string[0], 
                new float[0],
                "----", 
                0, 
                0
            );

        if (Player._instance.items.Count != 0)
            right %= Player._instance.items.Count;
        else
            right = 8;
        if (Player._instance.items.Count != 0)
            ViewStats(
                Player._instance.items[right], 
                playersStats, 
                Player._instance.items[right].namesOfStats, 
                Player._instance.items[right].stats,
                Player._instance.items[right].slot,
                Player._instance.items[right].value, 
                1
            );
        else
            ViewStats(
                new Item(),
                playersStats, 
                new string[0], 
                new float[0],
                "----", 
                0, 
                0
            );
    }

    void ShowItems()
    {       
        for (int i = 0; i < target.items.Count; i++)
        {
            tradersList.GetChild(i).GetComponent<Text>().text = target.items[i].itemName;
        }

        for (int i = 0; i < Player._instance.items.Count; i++)
        {
            playersList.GetChild(i).GetComponent<Text>().text = Player._instance.items[i].itemName;
        }

        for (int i = target.items.Count; i < tradersList.childCount; i++)
        {
            tradersList.GetChild(i).GetComponent<Text>().text = "----";
        }

        for (int i = Player._instance.items.Count; i < playersList.childCount; i++)
        {
            playersList.GetChild(i).GetComponent<Text>().text = "----";
        }

        tradersCash.text = target.cash.ToString();
        playersCash.text = Player._instance.cash.ToString();

        TakeCareOfIteratorAndShowStats();
    }

    void BuyItem()
    {
        if (Player._instance.items.Count == 7 || target.items.Count == 0)
            return;

        // sprawdz czy na pewno chcesz

        if (Player._instance.cash - target.items[left].value >= 0 && target.cash + target.items[left].value <= 99999999)
        {
            Player._instance.cash -= target.items[left].value * target.tradePricesModifier;
            target.cash += target.items[left].value * target.tradePricesModifier;
        }
        else
            return;

        Player._instance.items.Add(target.items[left]);
        target.items.Remove(target.items[left]);

        ShowItems();
    }

    void SellItem()
    {
        if (target.items.Count == 7 || Player._instance.items.Count == 0)
            return;

        // sprawdz czy na pewno chcesz

        if (target.cash - Player._instance.items[right].value >= 0 && Player._instance.cash + Player._instance.items[right].value <= 99999999)
        {
            target.cash -= Player._instance.items[right].value;
            Player._instance.cash += Player._instance.items[right].value;
        }
        else
            return;

        target.items.Add(Player._instance.items[right]);
        Player._instance.items.Remove(Player._instance.items[right]);

        ShowItems();
    }
}