using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnTheQuestTutorial : Tutorial
{
    public override void StartFollowing()
    {
        Player._instance.shootingEnded += CheckIfRunTutorial;
    }

    public override void TriggerNextTutorial()
    {
        throw new NotImplementedException();
    }

    protected void CheckIfRunTutorial(InteractableObject winner, InteractableObject looser, Gun playersGun)
    {
        if(winner == Player._instance)
        {
            GetComponent<TutorialController>().RunTutorial(tutorialText);

            Player._instance.shootingEnded -= CheckIfRunTutorial;
            Destroy(this);
        }
    }
}
