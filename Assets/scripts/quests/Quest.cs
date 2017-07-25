using System.Collections;
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
    public bool onQuest;
    public int questStadium = 0;

    public abstract void ActivateTheQuest();

    public abstract void CheckOnQuest(Person person);
}