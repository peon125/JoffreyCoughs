using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentController : UiElement
{
    public Transform itemsList, statsList;
    public Text itemCounter, itemName;
    int left = 0, leftShift = 0;
    bool leftVerticalAxisInUse = false;

    void Update()
    {
        MoveObjects(0, 1, stuff[1], way, -900, 0, timeToWait);
       

        if (opened && way == 0)
        {
            if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Equipment"))
            {
                StartCoroutine(CloseDialogue(stuff[1].transform));
            }

            if (Input.GetButtonDown("Interact"))
                UseItem(Player._instance.items[left]);
        }

        if (opened)
            SelectingObjects();
    }

    public void StartEquiping()
    {
        StartCoroutine(OpenDialogue(true));

        ShowItems();
    }

    void ShowItems()
    {
        UnshowItems();

        for (int i = 0; i < itemsList.childCount && i < Player._instance.items.Count; i++)
        {
            itemsList.GetChild(i).GetComponent<Text>().text = Player._instance.items[i].itemName;
            if (itemsList.GetChild(i).GetComponent<Text>().text.Length > 20)
                itemsList.GetChild(i).GetComponent<Text>().text = itemsList.GetChild(i).GetComponent<Text>().text.Substring(0, 10) + "...";
        }

        for (int i = Player._instance.items.Count; i < itemsList.childCount; i++)
        {
            itemsList.GetChild(i).GetComponent<Text>().text = "---";
        }

        leftShift = 0;
        left = 0;

        TakeCareOfIteratorAndShowStats();
    }

    private void UnshowItems()
    {
        foreach (Transform item in itemsList)
        {
            item.GetComponent<Text>().text = "---";
        }
    }

    void UseItem(Item item)
    {
        if (item.usable)
        {
            item.Use();

            ShowItems();
        }
    }

    void ThrowItemAway(Item item)
    {
        Player._instance.items.Remove(item);

        Instantiate(StaticValues._instance.bundlePrefab, Player._instance.transform.position, StaticValues._instance.bundlePrefab.transform.rotation);
    }

    void SelectingObjects()
    {
        if (Input.GetAxisRaw("Vertical1") != 0)
        {
            if (!leftVerticalAxisInUse)
            {
                if (Input.GetAxisRaw("Vertical1") > 0)
                {
                    left--;

                    ScrollAList(itemsList, Player._instance.items.ToArray(), ref left, 1, ref leftShift);
                }
                else if (Input.GetAxisRaw("Vertical1") < 0)
                {
                    left++;

                    ScrollAList(itemsList, Player._instance.items.ToArray(), ref left, -1, ref leftShift);
                }

                leftVerticalAxisInUse = true;
            }
        }
        else
            leftVerticalAxisInUse = false;


        if (leftVerticalAxisInUse)
        {
            TakeCareOfIteratorAndShowStats();
        }

        for (int i = 0; i < itemsList.childCount; i++)
        {
            if (i == left)
                itemsList.GetChild(i).GetComponent<Text>().color = selectedColor;
            else
                itemsList.GetChild(i).GetComponent<Text>().color = unselectedColor;
        }
    }

    void TakeCareOfIteratorAndShowStats()
    {
        left += leftShift;

        if (Player._instance.items.Count <= itemsList.childCount)
            left %= itemsList.childCount;

        //itemCounter.text = (left + 1).ToString() + "/" + Player._instance.items.Count.ToString();

        if (Player._instance.items.Count != 0)
            ViewStats(
                Player._instance.items[left],
                Player._instance.items[left].namesOfStats,
                Player._instance.items[left].stats,
                Player._instance.items[left].slot,
                Player._instance.items[left].value
            );
        else
            ViewStats(
                null,
                new string[0],
                new float[0],
                "----",
                0
            );

        left -= leftShift;
    }

    void ViewStats(Item item, string[] names, float[] values, string slot, int value)
    {
        itemName.text = item.itemName;

        if (item != null)
        {
            statsList.GetChild(4).GetChild(0).GetComponent<Text>().text = item.slot;

            for (int i = 0; i < names.Length; i++)
            {
                statsList.GetChild(i).GetComponent<Text>().text = item.namesOfStats[i];
                statsList.GetChild(i).GetChild(0).GetComponent<Text>().text = item.stats[i].ToString();
            }
        }

        for (int i = names.Length; i < statsList.childCount - 1; i++)
        {
            statsList.GetChild(i).GetComponent<Text>().text = "----";
            statsList.GetChild(i).GetChild(0).GetComponent<Text>().text = "----";
        }
    }
}
