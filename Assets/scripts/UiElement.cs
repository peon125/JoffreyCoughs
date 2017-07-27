using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiElement : MonoBehaviour 
{
    public Image sprite;
    public GameObject[] stuff;
    protected static Color selectedColor = new Color(230f/255f, 230f/255f, 230f/255f, 1), unselectedColor = new Color(1 - 230f/255f, 1 - 230f/255f, 1 - 230f/255f, 1);
    protected float yStart = -900, yEnd = 0, timeToWait = 1f;
    protected int way = 0;
    protected bool opened = false;
    protected Person target;

    public static void MoveObjects(int axisX, int axisY, GameObject gameObject, int way, float min, float max, float time)
    {
        if (way != 0)
        {
            gameObject.transform.localPosition += new Vector3(
                axisX * (way * (Mathf.Abs(min + max) / time) * Time.deltaTime),
                axisY * (way * (Mathf.Abs(min + max) / time) * Time.deltaTime),
                0
            );

            float x = 0, y = 0;

            if (axisX != 0)
                x = Mathf.Clamp(gameObject.transform.localPosition.x, min, max);

            if (axisY != 0)
                y = Mathf.Clamp(gameObject.transform.localPosition.y, min, max);

            gameObject.transform.localPosition = new Vector3(
                gameObject.transform.localPosition.x,
                Mathf.Clamp(gameObject.transform.localPosition.y, min, max),
                gameObject.transform.localPosition.z
            );

//            if (gameObject.transform.localPosition.y >= max || gameObject.transform.localPosition.y <= min)
//                way = 0;
        }
    }

    public static int ScrollAList(Transform list, Item[] items, ref int iterator, int way, ref int currentShift)
    {
        if (way == -1 && iterator == list.childCount - 1)
        {
            currentShift += 1;

            for (int j = 0; j < list.childCount; j++)
            {
                list.GetChild(j).GetComponent<Text>().text = items[j + currentShift].itemName; 
            }

            if (currentShift == items.Length - list.childCount)
            {
                Debug.Log("0");
                return iterator + 1;
            }
            else
            {
                Debug.Log("1");
                return iterator;
            }
        }

        return iterator;
    }

    protected IEnumerator OpenDialogue()
    {
        Open(true);
        way = 1;

        Player._instance.isBusy = true;

        yield return new WaitForSeconds(timeToWait);

        way = 0;
    }

    protected IEnumerator CloseDialogue()
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

    protected void Open(bool b)
    {
        opened = b;

        foreach (GameObject go in stuff)
            go.SetActive(b);
    }
}