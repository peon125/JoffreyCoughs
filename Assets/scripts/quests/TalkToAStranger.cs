using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkToAStranger : Quest 
{
    public Person fellaToSpeakWith;

    public override void ActivateTheQuest()
    {
        onQuest = true;

        Player._instance.talkEnded += CheckIfObjectiveIsCompleted;

        //dodać do listy
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
        }
        else if (onQuest && completedObjectives != allObjectives)
        {
            person.thingToSay = thingsToSay[1];
            questStadium = 2;
        }
        else if (onQuest && completedObjectives == allObjectives)
        {
            person.thingToSay = thingsToSay[2];
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
    }
}