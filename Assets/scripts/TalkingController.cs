using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkingController : UiElement 
{
    public GameObject arrow;
    public Image spriteRenderer;
    public Text text;
    public float speed, waitTime;
    public float arrowFrequency;
    bool doSpeak = false;
    public int limit;
    float timer = 0f, arrowTimer = 0f;
    string question, restOfQuestion;

    void Update()
    {
        MoveObjects(stuff[1], way, yStart, yEnd, timeToWait);

        Speaking();

        FlashingArrow();

        KeyHandling();
    }

    void Speaking()
    {
        if (doSpeak)
        {
            timer += Time.deltaTime;

            if (timer > speed)
            {
                text.text += question.Substring(text.text.Length, 1);
                timer = 0;

                if (text.text.Length == limit || (limit > question.Length && text.text.Length == question.Length))
                {
                    doSpeak = false;
                }
            }
        }
    }

    void FlashingArrow()
    {
        if (restOfQuestion != "")
        {
            arrow.SetActive(true);

            arrowTimer += Time.deltaTime;

            if (arrowTimer >= arrowFrequency/2)
            {
                arrow.SetActive(true);
            }
            else if( arrowTimer <= arrowFrequency/2)
            {
                arrow.SetActive(false);
            }

            if(arrowTimer >= arrowFrequency)
                arrowTimer = 0f;
        }
        else
        {
            arrow.SetActive(false);
            arrowTimer = 0f;
        }
    }

    void KeyHandling()
    {
        if (opened && way == 0)
        {
            if (Input.anyKeyDown)
            {
                if (!doSpeak && restOfQuestion != "")
                {
                    Say(restOfQuestion);
                    return;
                }

                if (doSpeak)
                {
                    if (question.Length >= limit)
                        text.text = question.Substring(0, limit);
                    else
                        text.text = question;
                    doSpeak = false;
                    return;
                }

                if (!doSpeak && restOfQuestion == "")
                {
                    Player._instance.EndOfTalk(target);

                    StartCoroutine(CloseDialogue(""));
                    return;
                }
            }
        }
    }

    public void Say(string question)
    {
        this.question = question;

        doSpeak = true;

        text.text = "";


        if (question.Length > limit)
        {
            restOfQuestion = question.Substring(limit);

            question = question.Substring(0, limit);
        }
        else
            restOfQuestion = "";
    }

    public void StartTalking(string s)
    {
        StartCoroutine(OpenDialogue(s));
        target = Player._instance.target;
        spriteRenderer.sprite = target.speakerSprite;
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

    protected IEnumerator CloseDialogue(string s)
    {
        way = -1;

        yield return new WaitForSeconds(timeToWait);

        Open(false);

        stuff[1].transform.localPosition = new Vector3(
            stuff[1].transform.localPosition.x,
            yStart,
            stuff[1].transform.localPosition.z
        );

        text.text = s;

        Player._instance.isBusy = false;
        way = 0;
    }
}