using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectingController : UiElement 
{
    public Text[] slots; //0 - name //1 - description //2 - items //3 - gun //4 - cash

    void Update()
    {
        MoveObjects(0, 1, stuff[1], way, yStart, yEnd, timeToWait);

        if (opened && way == 0)
        {
            if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Inspect"))
                StartCoroutine(CloseDialogue(stuff[1].transform));
        }
    }
        
    public void StartInspecting()
    {
        StartCoroutine(OpenDialogue(true));
        target = Player._instance.target;
        sprite.sprite = target.speakerSprite;

        ShowItems();
    }

    void ShowItems()
    {       
        slots[0].text = target._name;

        slots[1].text = target.description;

        for (int i = 0; i < target.items.Count; i++)
        {
            slots[2].transform.GetChild(i).GetComponent<Text>().text = target.items[i].itemName;
        }

        for (int i = target.items.Count; i < slots[2].transform.childCount; i++)
        {
            slots[2].transform.GetChild(i).GetComponent<Text>().text = "----";
        }

        slots[3].text = target.gun.itemName;

        slots[4].text = target.cash.ToString();
    }
}