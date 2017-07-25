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

    public static void MoveObjects(GameObject gameObject, int way, float min, float max, float time)
    {
        if (way != 0)
        {
            gameObject.transform.localPosition += new Vector3(
                0,
                way * (Mathf.Abs(min + max) / time) * Time.deltaTime,
                0
            );

            gameObject.transform.localPosition = new Vector3(
                gameObject.transform.localPosition.x,
                Mathf.Clamp(gameObject.transform.localPosition.y, min, max),
                gameObject.transform.localPosition.z
            );

//            if (gameObject.transform.localPosition.y >= max || gameObject.transform.localPosition.y <= min)
//                way = 0;
        }
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