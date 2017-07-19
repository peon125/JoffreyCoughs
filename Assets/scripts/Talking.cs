using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Talking : MonoBehaviour 
{
    public Text text;
    public float speed, waitTime;
    public GameObject[] stuff;
    public int yStart, yEnd;
    public float timeToWait;
    bool doSpeak = false, opened = false;
    float timer = 0f;
    string question;
    int way = 0;

    void Update()
    {

        if (way != 0)
        {
            MoveObjects();
        }


        if (opened && way ==0)
        {
            if (Input.anyKeyDown)
            if (doSpeak)
            {
                text.text = question;
            }
            else
            {
                StartCoroutine(CloseDialogue());
            }
        }

        if (doSpeak)
        {
            timer += Time.deltaTime;

            if (timer > speed)
            {
                if (text.text.Length == question.Length)
                {
                    doSpeak = false;
                    return;
                }

                text.text = question.Substring(0, text.text.Length + 1);
                timer = 0;
            }
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

    public void Say(string question)
    {
        this.question = question;
        doSpeak = true;
    }

    void Open(bool b)
    {
        opened = b;

        foreach (GameObject go in stuff)
            go.SetActive(b);
    }

    public void Says(string s)
    {
        StartCoroutine(OpenDialogue(s));
    }

    IEnumerator OpenDialogue(string s)
    {
        Open(true);
        way = 1;

        Player._instance.isBusy = true;

        yield return new WaitForSeconds(timeToWait);

        way = 0;

        Say(s);
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

        text.text = "";

        Player._instance.isBusy = false;
        way = 0;
    }
}