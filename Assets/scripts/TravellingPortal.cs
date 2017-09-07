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
    public Image image;
    public bool vertical, horizontal;
    public int portalToRespOn;
    float colorTimer = 0.5f;

    void Update()
    {
        text.color = Color.Lerp(Color.blue, Color.white, Mathf.PingPong(Time.time / colorTimer, 1));
        image.color = Color.Lerp(Color.white, Color.blue, Mathf.PingPong(Time.time / colorTimer, 1));
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);

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
            Player._instance.travellingController.Travel(place);
        }
    }
}