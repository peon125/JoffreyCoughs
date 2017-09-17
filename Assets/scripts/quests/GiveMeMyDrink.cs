using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveMeMyDrink : Quest {

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
        //else if (onQuest && completedObjectives == allObjectives) //quest is taken and finished and ready to return
        //{
        //    person.thingToSay = thingsToSay[3];
        //    Player._instance.questsController.completedQuests.Add(this);
        //    Player._instance.questsController.activatedQuests.Remove(this);
        //    Player._instance.items.Add(new Cash(2));
        //    person.Glowing(false);
        //    if (Player._instance.questsController.trackedQuest == this)
        //        Player._instance.questsController.SetTrackerActive(false);
        //    onQuest = false;
        //    questStadium = 4;

        //    return 3;
        //}
        else if (!onQuest && completedObjectives == allObjectives) //quest is finished and returned
        {
            person.thingToSay = thingsToSay[4];
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
            Player._instance.tradeEnded += CheckIfYerbaIsFound;
            questTrackContent = whatToPutIntoQuestTrack[0];

            return true;
        }
        else if (reaction == 1)
        {
            questStadium = 0;
        }

        return false;
    }

    private void CheckIfYerbaIsFound(InteractableObject trader, List<Item> boughtItems, List<Item> soldItems, int spentMoney)
    {
        foreach (Item item in boughtItems)
        {
            if (item.itemName == "yerba mate")
            {
                questTrackContent = whatToPutIntoQuestTrack[1];
                ObjectiveCompleted();
                Player._instance.tradeEnded += CheckIfYerbaIsReturned;
                Player._instance.tradeEnded -= CheckIfYerbaIsFound;

                break;
            }
        }
    }

    private void CheckIfYerbaIsReturned(InteractableObject trader, List<Item> boughtItems, List<Item> soldItems, int spentMoney)
    {
        Debug.Log(soldItems.Count);
        foreach (Item item in soldItems)
        {
            if (item.itemName == "yerba mate" && trader._name == "Mysterious Traveller")
            {
                trader.Glowing(false);
                ObjectiveCompleted();
                Player._instance.talkingController.StartTalking(thingsToSay[2]);
                Player._instance.tradeEnded -= CheckIfYerbaIsReturned;

                break;
            }
        }
    }

    public override void QuestCompleted()
    {
        Player._instance.questsController.completedQuests.Add(this);
        Player._instance.questsController.activatedQuests.Remove(this);
        Player._instance.items.Add(new Cash(2));
        if (Player._instance.questsController.trackedQuest == this)
            Player._instance.questsController.SetTrackerActive(false);
        onQuest = false;

        if (Player._instance.questsController.trackedQuest == this)
            Player._instance.questsController.UpdateTracker();
    }
}
