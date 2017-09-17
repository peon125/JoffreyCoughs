using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkToAStranger : Quest 
{
    public Person fellaToSpeakWith;
    public GameObject reward;
    
    public override int CheckOnQuest(InteractableObject person)
    {
        if (!onQuest && completedObjectives != allObjectives)
        {
            person.thingToSay = thingsToSay[0];
            questStadium = 1;
            reactionRequired = true;
            Player._instance.talkingController.questBeingTalkedAbout = this;

            return 1;
        }
        else if (onQuest && completedObjectives != allObjectives)
        {
            person.thingToSay = thingsToSay[1];
            questStadium = 2;

            return 2;
        }
        else if (onQuest && completedObjectives == allObjectives)
        {
            person.thingToSay = thingsToSay[2];
            Player._instance.questsController.completedQuests.Add(this);
            Player._instance.questsController.activatedQuests.Remove(this);
            Player._instance.items.Add(reward.GetComponent<Gun>());
            if (Player._instance.questsController.trackedQuest == this)
                Player._instance.questsController.SetTrackerActive(false);
            onQuest = false;
            questStadium = 3;

            return 3;
        }
        else if (!onQuest && completedObjectives == allObjectives)
        {
            person.thingToSay = thingsToSay[3];
            questStadium = 4;

            return 4;
        }

        return 999;
    }

    public override bool Reacted(int reaction)
    {
        reactionRequired = false;

        if (reaction == 0)
        {
            ActivateTheQuest();
            Player._instance.talkEnded += CheckIfObjectiveIsCompleted;
            questTrackContent = whatToPutIntoQuestTrack[0];

            return true;
        }
        else if (reaction == 1)
        {
            questStadium = 0;
        }

        return false;
    }

    void CheckIfObjectiveIsCompleted(InteractableObject speaker)
    {
        if (fellaToSpeakWith == speaker)
            ObjectiveCompleted();
    }

    public override void QuestCompleted()
    {
        Player._instance.talkEnded -= CheckIfObjectiveIsCompleted;
        Player._instance.questsController.questLogUpdatedInformer.QuestLogUpdated(questName);
        questTrackContent = whatToPutIntoQuestTrack[1];

        if (Player._instance.questsController.trackedQuest == this)
            Player._instance.questsController.UpdateTracker();
    }
}