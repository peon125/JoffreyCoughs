using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour 
{
    public Quest FindMyQuest(Person person)
    {
        foreach (Transform quest in transform)
            if (quest.GetComponent<Quest>() && quest.GetComponent<Quest>().questGiver == person)
            {
                return quest.GetComponent<Quest>();
            }

        return null;
    }
}