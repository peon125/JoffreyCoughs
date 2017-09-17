using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstFightTutorial : Tutorial
{
    protected void CheckIfRunTutorial(string from, string to)
    {
        if (to == "saloonKitchenInside")
        {
            GetComponent<TutorialController>().RunTutorial(tutorialText);

            TriggerNextTutorial();

            Player._instance.travelled -= CheckIfRunTutorial;
            Destroy(this);
        }
    }

    public override void StartFollowing()
    {
        Player._instance.travelled += CheckIfRunTutorial;
    }

    public override void TriggerNextTutorial()
    {
        GetComponent<ReturnTheQuestTutorial>().StartFollowing();
    }
}
