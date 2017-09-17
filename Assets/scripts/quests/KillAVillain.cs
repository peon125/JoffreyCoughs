using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAVillain : Quest
{
    public string fellaToBeKilled;
    public Item reward;

    public override int CheckOnQuest(InteractableObject person)
    {
        if (!onQuest && completedObjectives != allObjectives) //quest is not taken
        {
            person.thingToSay = thingsToSay[0];
            questStadium = 1;
            reactionRequired = true;
            Player._instance.talkingController.questBeingTalkedAbout = this;

            return 1;
        }
        else if (onQuest && completedObjectives == 0) //quest is taken and in progress
        {
            person.thingToSay = thingsToSay[1];
            questStadium = 2;

            return 2;
        }
        else if (onQuest && completedObjectives == 1) //quest is taken and finished and ready to return
        {
            person.thingToSay = thingsToSay[2];
            questStadium = 3;

            return 3;
        }
        else if (onQuest && completedObjectives == allObjectives) //quest is taken and finished and ready to return
        {
            person.thingToSay = thingsToSay[3];
            Player._instance.questsController.completedQuests.Add(this);
            Player._instance.questsController.activatedQuests.Remove(this);
            Player._instance.items.Add(new Cash(1000));
            person.Glowing(false);
            if (Player._instance.questsController.trackedQuest == this)
                Player._instance.questsController.SetTrackerActive(false);
            onQuest = false;
            questStadium = 4;

            return 4;
        }
        else if (!onQuest && completedObjectives == allObjectives) //quest is finished and returned
        {
            person.thingToSay = thingsToSay[4];
            questStadium = 4;

            return 5;
        }
        
        

        return 999;
    }

    public override bool Reacted(int reaction)
    {
        reactionRequired = false;

        if (reaction == 0)
        {
            ActivateTheQuest();
            Player._instance.tradeEnded += CheckIfGunIsBought;
            questTrackContent = whatToPutIntoQuestTrack[0];

            return true;
        }
        else if (reaction == 1)
        {
            questStadium = 0;
        }

        return false;
    }

    private void CheckIfGunIsBought(InteractableObject trader, List<Item> boughtItems, List<Item> soldItems, int spentMoney)
    {
        if(trader._name == "Tommy The Gunseller")
        {
            foreach(Item item in boughtItems)
            {
                if (item.GetComponent<Gun>())
                {
                    questTrackContent = whatToPutIntoQuestTrack[1];
                    ObjectiveCompleted();
                    Player._instance.tradeEnded -= CheckIfGunIsBought;
                    Player._instance.shootingEnded += CheckIfVillainIsKilled;

                    break;
                }
            }
        }
    }

    private void CheckIfVillainIsKilled(InteractableObject winner, InteractableObject looser, Gun playersGun)
    {
        if(looser._name == "Demo Villain")
        {
            questTrackContent = whatToPutIntoQuestTrack[2];
            ObjectiveCompleted();

            Player._instance.shootingEnded -= CheckIfVillainIsKilled;
        }
    }

    public override void QuestCompleted()
    {

        if (Player._instance.questsController.trackedQuest == this)
            Player._instance.questsController.UpdateTracker();
    }
}
