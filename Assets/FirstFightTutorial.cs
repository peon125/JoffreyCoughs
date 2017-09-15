using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstFightTutorial : Tutorial
{
    void Start()
    {
        Player._instance.travelled += CheckIfRunTutorial;
    }

    protected void CheckIfRunTutorial(string from, string to)
    {
        if (to == "saloonKitchenInside")
        {
            GetComponent<TutorialController>().RunTutorial(tutorialText);

            Player._instance.travelled -= CheckIfRunTutorial;
            Destroy(this);
        }
    }
}
