using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TravellingPortal : MonoBehaviour
{ 
    public string place;
    public Vector3 whereToResp;
    public Text text;
    public bool vertical, horizontal;
    public int portalToRespOn;
    float colorTimer = 0.5f;

    void Update()
    {
        text.color = Color.Lerp(Color.green, Color.white, Mathf.PingPong(Time.time / colorTimer, 1));

        if (vertical)
            text.transform.position = new Vector3(
                text.transform.position.x,
                Player._instance.transform.position.y,
                text.transform.position.z
                );

        if (horizontal)
            text.transform.position = new Vector3(
                Player._instance.transform.position.x,
                text.transform.position.y,
                text.transform.position.z
                );
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Player>())
        {
            Player._instance.travellingController.whichPortalToResp = portalToRespOn;
            //SceneManager.LoadScene(place);
            Debug.Log("asdfg");
        }
    }
}