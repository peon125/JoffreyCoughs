using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstGunBoughtTutorial : Tutorial
{
    void Start()
    {
        Player._instance.tradeEnded += CheckIfRunTutorial;
    }

    protected void CheckIfRunTutorial(InteractableObject trader, List<Item> boughtItems, int spentMoney)
    {
        foreach(Item item in boughtItems)
        {
            if(item.GetComponent<Gun>())
            {
                GetComponent<TutorialController>().RunTutorial(tutorialText);

                Player._instance.tradeEnded -= CheckIfRunTutorial;
                Destroy(this);
                break;
            }
        }
    }
}
