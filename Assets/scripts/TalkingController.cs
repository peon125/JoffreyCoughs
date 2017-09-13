using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TalkingController : UiElement 
{
    public GameObject arrow;
    public Image spriteRenderer;
    public Text text;
    public Transform reactionsTransform;
    public Quest questBeingTalkedAbout;
    public float speed, waitTime;
    public float arrowFrequency;
    bool doSpeak = false, reactNow =false;
    public int limit;
    int reactionsIterator, reactionsIteratorShift;
    float timer = 0f, arrowTimer = 0f;
    string question, restOfQuestion;
    bool leftVerticalAxisInUse = false;

    void Update()
    {
        MoveObjects(0, 1, stuff[1], way, yStart, yEnd, timeToWait);

        Speaking();

        FlashingArrow();

        KeyHandling();


        for (int i = 0; i < reactionsTransform.childCount && reactNow; i++)
        {
            if (i == reactionsIterator)
                reactionsTransform.GetChild(i).GetComponent<Text>().color = selectedColor;
            else
                reactionsTransform.GetChild(i).GetComponent<Text>().color = unselectedColor;
        }
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
        if (restOfQuestion != "" && !doSpeak)
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
            if (Input.anyKeyDown && !reactNow)
            {
                if (!doSpeak && restOfQuestion == "") /////
                {
                    
                    Say(restOfQuestion);
                        Player._instance.EndOfTalk(target);

                        StartCoroutine(CloseDialogue(""));
                    return;
                }
                else if (!doSpeak && restOfQuestion != "")
                {
                    if (questBeingTalkedAbout != null && questBeingTalkedAbout.reactionRequired)
                    {
                        StartReacting();
                        return;
                    }
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
            } else

            if (reactNow)
            {
                React();
            }
        }
    }

    public void Say(string question)
    {
        if (question != "")
            this.question = question;
        else
            this.question = ">>nothing to say<<";

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

    void StartReacting()
    {
        Say(questBeingTalkedAbout.reactionQuestion);

        for (int j = 0; j < questBeingTalkedAbout.questReactions.Length; j++)
        {
            reactionsTransform.GetChild(j).GetComponent<Text>().text = questBeingTalkedAbout.questReactions[j + reactionsIteratorShift];
        }

        for (int i = questBeingTalkedAbout.questReactions.Length; i < reactionsTransform.childCount; i++)
        {
            reactionsTransform.GetChild(i).GetComponent<Text>().text = "---";
        }

        reactionsTransform.gameObject.SetActive(true);
        reactionsIterator = 0;
        reactNow = true;
    }

    void React()
    {
        if (Input.GetAxisRaw("Vertical1") != 0)
        {
            if (!leftVerticalAxisInUse)
            {
                if (Input.GetAxisRaw("Vertical1") > 0)
                {
                    reactionsIterator--;

                    ScrollAList(reactionsTransform, questBeingTalkedAbout.questReactions, ref reactionsIterator, 1, ref reactionsIteratorShift);
                }
                else if (Input.GetAxisRaw("Vertical1") < 0)
                {
                    reactionsIterator++;

                    ScrollAList(reactionsTransform, questBeingTalkedAbout.questReactions, ref reactionsIterator, -1, ref reactionsIteratorShift);
                }

                leftVerticalAxisInUse = true;
            }
        }
        else
            leftVerticalAxisInUse = false;

        




        if(Input.GetButtonDown("Submit"))
        {
            questBeingTalkedAbout.Reacted(reactionsIterator + reactionsIteratorShift);

            reactNow = false;

            Player._instance.EndOfTalk(target);

            StartCoroutine(CloseDialogue(""));

            Debug.Log("dab on them haters!!!");
        }





    }

    public void StartTalking(string s)
    {
        StartCoroutine(OpenDialogue(s));
        target = Player._instance.target;
        spriteRenderer.sprite = target.speakerSprite;
    }

    protected IEnumerator OpenDialogue(string s)
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
        questBeingTalkedAbout = null;
        reactionsTransform.gameObject.SetActive(false);
        reactionsIterator = 0;
        way = -1;
        doSpeak = false;

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