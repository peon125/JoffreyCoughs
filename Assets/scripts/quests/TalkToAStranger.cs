using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkToAStranger : Quest 
{
    public Person fellaToSpeakWith;
    public GameObject reward;

    public override void ActivateTheQuest()
    {
        onQuest = true;

        Player._instance.talkEnded += CheckIfObjectiveIsCompleted;

        Player._instance.questsController.activatedQuests.Add(this);
    }

    void CheckIfObjectiveIsCompleted(Person speaker)
    {
        if (fellaToSpeakWith == speaker)
            ObjectiveCompleted();
    }

    public override void CheckOnQuest(Person person)
    {
        if (!onQuest && completedObjectives != allObjectives)
        {
            person.thingToSay = thingsToSay[0];
            ActivateTheQuest();
            questStadium = 1;
            questTrackContent = whatToPutIntoQuestTrack[0];
        }
        else if (onQuest && completedObjectives != allObjectives)
        {
            person.thingToSay = thingsToSay[1];
            questStadium = 2;

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
        }
        else if (!onQuest && completedObjectives == allObjectives)
        {
            person.thingToSay = thingsToSay[3];
            questStadium = 4;
        }
    }

    void  ObjectiveCompleted()
    {
        completedObjectives++;

        if (completedObjectives >= allObjectives)
            QuestCompleted();
    }

    void QuestCompleted()
    {
        Player._instance.talkEnded -= CheckIfObjectiveIsCompleted;
        questTrackContent = whatToPutIntoQuestTrack[1];

        if (Player._instance.questsController.trackedQuest == this)
            Player._instance.questsController.UpdateTracker();
        

    }
}