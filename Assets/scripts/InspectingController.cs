using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectingController : UiElement 
{
    public static InspectingController _instance;
    public Text[] slots; //0 - name //1 - description //2 - items //3 - gun //4 - cash

    void Awake()
    {
        _instance = this;
    }

    void Update()
    {
        if (way != 0)
        {
            MoveObjects();
        }

        if (opened && way == 0)
        {
            if (Input.GetButtonDown("Cancel"))
                StartCoroutine(CloseDialogue());
        }
    }

    void MoveObjects()
    {
        stuff[1].transform.localPosition += new Vector3(
            0,
            way * (Mathf.Abs(yStart + yEnd) / timeToWait) * Time.deltaTime,
            0
        );

        stuff[1].transform.localPosition = new Vector3(
            stuff[1].transform.localPosition.x,
            Mathf.Clamp(stuff[1].transform.localPosition.y, yStart, yEnd),
            stuff[1].transform.localPosition.z
        );
    }

    void Open(bool b)
    {
        opened = b;

        foreach (GameObject go in stuff)
            go.SetActive(b);
    }

    public void LetsSee()
    {
        StartCoroutine(OpenDialogue());
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

    IEnumerator OpenDialogue()
    {
        Open(true);
        way = 1;

        Player._instance.isBusy = true;

        yield return new WaitForSeconds(timeToWait);

        way = 0;
    }

    IEnumerator CloseDialogue()
    {
        way = -1;

        yield return new WaitForSeconds(timeToWait);

        Open(false);

        stuff[1].transform.localPosition = new Vector3(
            stuff[1].transform.localPosition.x,
            yStart,
            stuff[1].transform.localPosition.z
        );



        Player._instance.isBusy = false;
        way = 0;
    }
}
