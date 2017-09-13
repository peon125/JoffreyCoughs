using System.Collections;
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
    protected InteractableObject target;

    public static void MoveObjects(float axisX, float axisY, GameObject gameObject, int way, float min, float max, float time)
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
                x,
                y,
                gameObject.transform.localPosition.z
            );
        }
    }

    public static void ScrollAList(Transform list, Item[] items, ref int iterator, int way, ref int currentShift)
    {
        if (items.Length <= list.childCount)
        {
            if (items.Length != 0)
            {
                if (iterator < 0)
                    iterator = items.Length - 1;
                iterator %= items.Length;
            }
            else
                iterator = items.Length + 1;

            return;
        }

        if (way == -1)
        {
            if (currentShift != items.Length - list.childCount && iterator == list.childCount - 1)
            {
                currentShift += 1;

                iterator -= 1;
            }
            else if (currentShift == items.Length - list.childCount && iterator == list.childCount)
            {
                currentShift = 0;

                iterator = 0;
            }
        }
        else if (way == 1)
        {
            if (currentShift != 0 && iterator == 0)
            {
                currentShift -= 1;

                iterator += 1;
            }
            else if (currentShift == 0 && iterator == -1)
            {
                currentShift = items.Length - list.childCount;

                iterator = list.childCount - 1;
            }
        }

        for (int j = 0; j < list.childCount; j++)
        {
            list.GetChild(j).GetComponent<Text>().text = items[j + currentShift].itemName; 
        }
    }

    public static void ScrollAList(Transform list, string[] items, ref int iterator, int way, ref int currentShift)
    {
        if (items.Length <= list.childCount)
        {
            if (items.Length != 0)
            {
                if (iterator < 0)
                    iterator = items.Length - 1;
                iterator %= items.Length;
            }
            else
                iterator = items.Length + 1;

            return;
        }

        if (way == -1)
        {
            if (currentShift != items.Length - list.childCount && iterator == list.childCount - 1)
            {
                currentShift += 1;

                iterator -= 1;
            }
            else if (currentShift == items.Length - list.childCount && iterator == list.childCount)
            {
                currentShift = 0;

                iterator = 0;
            }
        }
        else if (way == 1)
        {
            if (currentShift != 0 && iterator == 0)
            {
                currentShift -= 1;

                iterator += 1;
            }
            else if (currentShift == 0 && iterator == -1)
            {
                currentShift = items.Length - list.childCount;

                iterator = list.childCount - 1;
            }
        }

        for (int j = 0; j < items.Length; j++)
        {
            list.GetChild(j).GetComponent<Text>().text = items[j + currentShift];
        }

        for (int i = items.Length; i < list.childCount; i++)
        {
            list.GetChild(i).GetComponent<Text>().text = "---";
        }
    }

    // jebnąc toStringem i typem generycznym 
    public static void ScrollAList(Transform list, Quest[] items, ref int iterator, int way, ref int currentShift)
    {
        if (items.Length <= list.childCount)
        {
            if (items.Length != 0)
            {
                if (iterator < 0)
                    iterator = items.Length - 1;
                iterator %= items.Length;
            }
            else
                iterator = items.Length + 1;

            return;
        }

        if (way == -1)
        {
            if (currentShift != items.Length - list.childCount && iterator == list.childCount - 1)
            {
                currentShift += 1;

                iterator -= 1;
            }
            else if (currentShift == items.Length - list.childCount && iterator == list.childCount)
            {
                currentShift = 0;

                iterator = 0;
            }
        }
        else if (way == 1)
        {
            if (currentShift != 0 && iterator == 0)
            {
                currentShift -= 1;

                iterator += 1;
            }
            else if (currentShift == 0 && iterator == -1)
            {
                currentShift = items.Length - list.childCount;

                iterator = list.childCount - 1;
            }
        }

        for (int j = 0; j < list.childCount; j++)
        {
            list.GetChild(j).GetComponent<Text>().text = items[j + currentShift].questName;
        }
    }

    public static void ScrollAList<T>(Transform list, T[] items, ref int iterator, int way, ref int currentShift)
    {
        if (items.Length == 0)
            return;

        if (items.Length <= list.childCount)
        {
            if (items.Length != 0)
            {
                if (iterator < 0)
                    iterator = items.Length - 1;
                iterator %= items.Length;
            }
            else
                iterator = items.Length + 1;

            return;
        }

        if (way == -1)
        {
            if (currentShift != items.Length - list.childCount && iterator == list.childCount - 1)
            {
                currentShift += 1;

                iterator -= 1;
            }
            else if (currentShift == items.Length - list.childCount && iterator == list.childCount)
            {
                currentShift = 0;

                iterator = 0;
            }
        }
        else if (way == 1)
        {
            if (currentShift != 0 && iterator == 0)
            {
                currentShift -= 1;

                iterator += 1;
            }
            else if (currentShift == 0 && iterator == -1)
            {
                currentShift = items.Length - list.childCount;

                iterator = list.childCount - 1;
            }
        }

        for (int j = 0; j < items.Length; j++)
        {
            list.GetChild(j).GetComponent<Text>().text = items[j + currentShift].ToString();
        }

        for (int i = items.Length; i < list.childCount; i++)
        {
            list.GetChild(i).GetComponent<Text>().text = "---";
        }
    }

    protected void Open(bool b)
    {
        opened = b;

        foreach (GameObject go in stuff)
            go.SetActive(b);
    }

    protected IEnumerator OpenDialogue(bool playerIsBusyUsingIt)
    {
        Open(true);
        way = 1;

        Player._instance.isBusy = playerIsBusyUsingIt;

        yield return new WaitForSeconds(timeToWait);

        way = 0;
    }

    protected IEnumerator CloseDialogue(Transform transform)
    {
        way = -1;

        yield return new WaitForSeconds(timeToWait);

        Open(false);

        transform.transform.localPosition = new Vector3(
            transform.transform.localPosition.x,
            yStart,
            transform.transform.localPosition.z
        );

        Player._instance.isBusy = false;
        way = 0;
    }
}