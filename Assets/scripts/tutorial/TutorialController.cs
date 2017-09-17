using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public GameObject board;
    public Text tutorialText;
    bool tutorialOpened;

    void Start()
    {
        GetComponent<GetQuestTutorial>().StartFollowing();
    }

    void Update()
    {
        if(tutorialOpened)
            Player._instance.isBusy = true;

        if (tutorialOpened && Input.GetButtonDown("Submit"))
        {
            CloseTutorial();
        }
    }

    public void RunTutorial(string s)
    {
        tutorialText.text = s;

        board.SetActive(true);

        tutorialOpened = true;
    }

    public void CloseTutorial()
    {
        Player._instance.isBusy = false;

        board.SetActive(false);

        tutorialOpened = false;
    }
}
