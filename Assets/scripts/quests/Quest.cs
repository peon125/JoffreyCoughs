using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : MonoBehaviour 
{
    public string questGiver;
    public string questReceiver;
    public string[] peopleRelated;
    public string questName, rewardName;
    public int allObjectives;
    public int completedObjectives;
    public string[] thingsToSay;
    public string reactionQuestion;
    public string[] questReactions;
    public string chosenReaction;
    public bool[] whatGoesToQuestLog;
    public string[] whatToPutIntoQuestTrack;
    public bool onQuest;
    public int questStadium = 0;
    public string questTrackContent;
    public bool reactionRequired;

    public void ActivateTheQuest()
    {
        onQuest = true;
        Player._instance.questsController.activatedQuests.Add(this);
        Player._instance.questsController.questLogUpdatedInformer.QuestLogUpdated(questName);
    }

    public abstract int CheckOnQuest(InteractableObject person);

    public abstract bool Reacted(int i);

    public abstract void QuestCompleted();

    protected void ObjectiveCompleted()
    {
        completedObjectives++;
        Player._instance.questsController.questLogUpdatedInformer.QuestLogUpdated(questName);

        Player._instance.questsController.UpdateTracker();

        if (completedObjectives == allObjectives)
            QuestCompleted();
    }
}