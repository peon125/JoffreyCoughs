using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopVisitedTutorial : Tutorial
{
    public override void StartFollowing()
    {
        Player._instance.travelled += CheckIfRunTutorial;
    }

    protected void CheckIfRunTutorial(string from, string to)
    {
        if(to == "tommysGunsInside")
        {
            GetComponent<TutorialController>().RunTutorial(tutorialText);

            TriggerNextTutorial();

            Player._instance.travelled -= CheckIfRunTutorial;
            Destroy(this);
        }
    }

    public override void TriggerNextTutorial()
    {
        GetComponent<FirstGunBoughtTutorial>().StartFollowing();
    }
}
