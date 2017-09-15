using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopVisitedTutorial : Tutorial
{
    void Start()
    {
        Player._instance.travelled += CheckIfRunTutorial;
    }

    protected void CheckIfRunTutorial(string from, string to)
    {
        if(to == "tommysGunsInside")
        {
            GetComponent<TutorialController>().RunTutorial(tutorialText);

            Player._instance.travelled -= CheckIfRunTutorial;
            Destroy(this);
        }
    }
}
