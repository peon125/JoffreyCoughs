using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLogUpdatedInformer : UiElement
{
    public Text questName;
    public float timeOfVisibility;
    float timer;
    float min = 500f, max = 900f;

    void Update()
    {
        MoveObjects(1, 0, gameObject, way, min, max, 2f);

        if(way == -1)
        {
            timer += Time.deltaTime;

            if(timer >= timeOfVisibility)
            {
                way = 1;
            }
        }
    }

    public void QuestLogUpdated(string questName)
    {
        this.questName.text = questName;
        timer = 0;
        way = -1;
    }
}