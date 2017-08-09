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

    void CheckIfObjectiveIsCompleted(InteractableObject speaker)
    {
        if (fellaToSpeakWith == speaker)
            ObjectiveCompleted();
    }

    void ObjectiveCompleted()
    {
        completedObjectives++;

        if (completedObjectives == allObjectives)
            QuestCompleted();
    }

    void QuestCompleted()
    {
        Player._instance.talkEnded -= CheckIfObjectiveIsCompleted;
        Player._instance.questsController.questLogUpdatedInformer.QuestLogUpdated(questName);
        questTrackContent = whatToPutIntoQuestTrack[1];

        if (Player._instance.questsController.trackedQuest == this)
            Player._instance.questsController.UpdateTracker();
    }
}