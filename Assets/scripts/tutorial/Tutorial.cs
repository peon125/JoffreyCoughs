using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tutorial : MonoBehaviour
{
    [TextArea(3, 30)]
    public string tutorialText;

    public abstract void StartFollowing();

    public abstract void TriggerNextTutorial();
}
