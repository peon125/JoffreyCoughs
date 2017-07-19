using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour 
{
    Enemy previousTarget = null;

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.GetComponent<Enemy>())
        {
            if (previousTarget != null)
                previousTarget.iAmBeingMarkedTimer = 0;

            collider.GetComponent<Enemy>().iAmBeingMarkedTimer = 3f;

            previousTarget = collider.GetComponent<Enemy>();
        }
    }
}