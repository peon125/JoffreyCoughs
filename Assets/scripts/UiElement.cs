using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiElement : MonoBehaviour 
{
    public Image sprite;
    public GameObject[] stuff;
    protected float yStart = -900, yEnd = 0, timeToWait = 1f;
    protected int way = 0;
    protected bool opened = false;
    protected Person target;

    protected void MoveObjects()
    {
        if (way != 0)
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