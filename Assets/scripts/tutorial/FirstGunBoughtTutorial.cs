using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstGunBoughtTutorial : Tutorial
{
    protected void CheckIfRunTutorial(InteractableObject trader, List<Item> boughtItems, List<Item> soldItems, int spentMoney)
    {
        foreach(Item item in boughtItems)
        {
            if(item.GetComponent<Gun>())
            {
                GetComponent<TutorialController>().RunTutorial(tutorialText);

                TriggerNextTutorial();

                Player._instance.tradeEnded -= CheckIfRunTutorial;
                Destroy(this);
                break;
            }
        }
    }

    public override void StartFollowing()
    {
        Player._instance.tradeEnded += CheckIfRunTutorial;
    }

    public override void TriggerNextTutorial()
    {
        GetComponent<FirstFightTutorial>().StartFollowing();
    }
}
