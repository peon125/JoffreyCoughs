using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestsController : UiElement 
{
    public List<Quest> activatedQuests, completedQuests;
    public GameObject questsListElementPrefab;
    public Transform questsStatus, questsList, questsTransform;
    public Text questTitle, questReward, questDescription;
    public QuestLogUpdatedInformer questLogUpdatedInformer;
    public float scrollingSpeed;

    public Text trackQuestName, trackQuestContent;
    public RectTransform trackBackground;

    int left = 0, up = 0, right = 0, leftShift = 0;
    bool leftVerticalAxisInUse = false, leftHorizontalAxisInUse = false, rightVerticalAxisInUse = false;
    List<Quest> currentlyChoosedQuests = new List<Quest>();

    public Quest trackedQuest = null;

    Vector3 questsListStartPos = new Vector3(), descripitonStartPos = new Vector3();

    float questListLength = 317f, descriptionLength = 220f, trackerMin = 350, trackerMax = 600;

    int trackerWay = 0;

    void Start()
    {
        questsListStartPos = questsList.localPosition;
        descripitonStartPos = questDescription.transform.localPosition;

        currentlyChoosedQuests = activatedQuests;
    }

    void Update()
    {
        MoveObjects(0, 1, stuff[1], way, yStart, yEnd, timeToWait);

        MoveObjects(0, 1, trackBackground.parent.gameObject, trackerWay, trackerMin, trackerMax, timeToWait);

        if (opened && way == 0)
        {
            if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("QuestLog"))
            {
                StartCoroutine(CloseDialogue(stuff[1].transform));
                //UnshowQuests();
            }

            if (Input.GetKeyDown(KeyCode.F) && (up == 0))
            {
                if (trackedQuest != currentlyChoosedQuests[left])
                {
                    trackedQuest = currentlyChoosedQuests[left];
                    UpdateTracker();
                    SetTrackerActive(true);
                }
                else
                {
                    trackedQuest = null;
                    SetTrackerActive(false);
                }
            }
        }

        if (opened)
        {
            SelectingQuest();
        }

        //questsList.localPosition = new Vector3(
        //    questsList.localPosition.x,
        //    Mathf.Clamp(questsListStartPos.y - 35 * left, questsListStartPos.y, questsListStartPos.y - 35 * (currentlyChoosedQuests.Count - 7)),
        //    questsList.localPosition.z
        //);

        if (questDescription.GetComponent<RectTransform>().sizeDelta.y > descriptionLength * 2)
        {
            questDescription.transform.localPosition = new Vector3(
                questDescription.transform.localPosition.x,
                Mathf.Clamp(questDescription.transform.localPosition.y, descripitonStartPos.y, descripitonStartPos.y + questDescription.GetComponent<RectTransform>().sizeDelta.y - descriptionLength),
                questDescription.transform.localPosition.z
            );
        }
    }

    public Quest FindMyQuest(InteractableObject person)
    {
        foreach (Transform quest in questsTransform)
            if (quest.GetComponent<Quest>() && quest.GetComponent<Quest>().questGiver == person)
                return quest.GetComponent<Quest>();

        return null;
    }

    public void StatViewingQuests()
    {
        StartCoroutine(OpenDialogue(true));

        ShowQuests();
        ShowDetailsOfQuest();
    }

    void ShowQuests()
    {
        //UnshowQuests();

        for (int i = 0; i < questsList.childCount && i < currentlyChoosedQuests.Count; i++)
        {
            questsList.GetChild(i).GetComponent<Text>().text = currentlyChoosedQuests[i].questName;

            if (questsList.GetChild(i).GetComponent<Text>().text.Length > 13)
                questsList.GetChild(i).GetComponent<Text>().text = questsList.GetChild(i).GetComponent<Text>().text.Substring(0, 10) + "...";
        }

        for (int i = currentlyChoosedQuests.Count; i < questsList.childCount; i++)
        {
            questsList.GetChild(i).GetComponent<Text>().text = "---";
        }

        leftShift = 0;
        left = 0;


        //for (int i = 0; i < currentlyChoosedQuests.Count; i++)
        //{
        //    GameObject questName = (GameObject)Instantiate(
        //        questsListElementPrefab,
        //        questsListElementPrefab.transform.localPosition,
        //        questsListElementPrefab.transform.rotation,
        //        questsList
        //    );

        //    questName.transform.localPosition = new Vector3(
        //        0,
        //        (questsListElementPrefab.transform.localScale.y - 10) * i,
        //        0
        //    );
        //}

        ShowDetailsOfQuest();

        HighlightChosenObject(questsStatus, up);
    }

    void UnshowQuests()
    {
        foreach (Transform questName in questsList)
            Destroy(questName.gameObject);
    }

    void SelectingQuest()
    {
        if (Input.GetAxisRaw("Horizontal1") != 0)
        {
            if (!leftHorizontalAxisInUse)
            {
                if (Input.GetAxisRaw("Horizontal1") > 0)
                {
                    up++;
                }
                else if (Input.GetAxisRaw("Horizontal1") < 0)
                {
                    if (up - 1 >= 0)
                        up--;
                    else
                        up = questsStatus.childCount - 1;
                }

                leftHorizontalAxisInUse = true;
            }
        }
        else
            leftHorizontalAxisInUse = false;

        if (Input.GetAxisRaw("Vertical1") != 0)
        {
            if (!leftVerticalAxisInUse)
            {
                if (Input.GetAxisRaw("Vertical1") > 0)
                {
                    left--;

                    ScrollAList(questsList, currentlyChoosedQuests.ToArray(), ref left, 1, ref leftShift);
                }
                else if (Input.GetAxisRaw("Vertical1") < 0)
                {
                    left++;

                    ScrollAList(questsList, currentlyChoosedQuests.ToArray(), ref left, -1, ref leftShift);
                }

                leftVerticalAxisInUse = true;
            }
        }
        else
            leftVerticalAxisInUse = false;

        if (Input.GetAxisRaw("Vertical2") != 0)
        {
            if (!rightVerticalAxisInUse)
            {
                questDescription.transform.localPosition += new Vector3(0, -1 * scrollingSpeed * Input.GetAxisRaw("Vertical2"), 0);

                rightVerticalAxisInUse = true;
            }
        }
        else
            rightVerticalAxisInUse = false;


        if (up == 0)
            currentlyChoosedQuests = activatedQuests;
        else if (up == 1)
            currentlyChoosedQuests = completedQuests;


        if (leftVerticalAxisInUse)
        {
            if (currentlyChoosedQuests.Count != 0)
                left %= currentlyChoosedQuests.Count;
            else
                left = 8;

            ShowDetailsOfQuest();
        }

        if (leftHorizontalAxisInUse)
        {
            left = 0;

            if (questsStatus.childCount != 0)
                up %= questsStatus.childCount;
            else
                up = 8;

//            HighlightChosenObject(questsStatus, up);
//            HighlightChosenObject(questsList, left);

            ShowQuests();
            ShowDetailsOfQuest();
        }
    }

    void ShowDetailsOfQuest()
    {
        if (currentlyChoosedQuests.Count == 0)
            return;

        questTitle.text = currentlyChoosedQuests[left].questName;
        questReward.text = currentlyChoosedQuests[left].rewardName;
        questDescription.text = "";

        for (int i = 0; i < currentlyChoosedQuests[left].questStadium; i++)
        {
            if (currentlyChoosedQuests[left].whatGoesToQuestLog[i])
                questDescription.text += currentlyChoosedQuests[left].questGiver._name + " said: " + currentlyChoosedQuests[left].thingsToSay[i] + System.Environment.NewLine;
        }

        HighlightChosenObject(questsList, left);
    }

    void HighlightChosenObject(Transform transform, int i)
    {
        for (int _i = 0; _i < transform.childCount; _i++)
        {
            if (_i == i)
                transform.GetChild(_i).GetComponent<Text>().color = selectedColor;
            else
                transform.GetChild(_i).GetComponent<Text>().color = unselectedColor;
        }
    }

    public void SetTrackerActive(bool b)
    {
        trackBackground.parent.gameObject.SetActive(b);
        if (b)
            trackerWay = -1;
        else
            trackerWay = 1;
    }

    public void UpdateTracker()
    {
        trackQuestName.text = trackedQuest.questName;
        trackQuestContent.text = trackedQuest.questTrackContent;
        trackBackground.sizeDelta = new Vector2(
            trackBackground.sizeDelta.x,
            trackQuestName.GetComponent<RectTransform>().sizeDelta.y + trackQuestContent.GetComponent<RectTransform>().sizeDelta.y + 40
        );
    }

    public void UpdateQuestLog()
    {
        
    }
}