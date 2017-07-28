﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : MonoBehaviour 
{
    public Person questGiver;
    public Person questReceiver;
    public Person[] peopleRelated;
    public string questName, rewardName;
    public int allObjectives;
    public int completedObjectives;
    public string[] thingsToSay;
    public bool[] whatGoesToQuestLog;
    public string[] whatToPutIntoQuestTrack;
    public bool onQuest;
    public int questStadium = 0;
    public string questTrackContent;

    public void ActivateTheQuest()
    {
        onQuest = true;
        Player._instance.questsController.activatedQuests.Add(this);
        Player._instance.questsController.questLogUpdatedInformer.QuestLogUpdated(questName);
    }

    public abstract void CheckOnQuest(InteractableObject person);
}