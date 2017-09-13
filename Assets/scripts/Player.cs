using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void ShootingEnded(InteractableObject winner, InteractableObject looser, Gun playersGun);
public delegate void TradeEnded(InteractableObject trader, List<Item> boughtItems, int spentMoney);
public delegate void InspectionEnded(InteractableObject inspected);
public delegate void TalkEnded(InteractableObject speaker);
public delegate void ItemUsed(Item item);

public class Player : Person 
{
    public static Player _instance;

    public event ShootingEnded shootingEnded;
    public event TradeEnded tradeEnded;
    public event InspectionEnded inspectionEnded;
    public event TalkEnded talkEnded;
    public event ItemUsed itemUsed;

    public TradingController tradingController;
    public InspectingController inspectingController;
    public TalkingController talkingController;
    public ShootingController shootingController;
    public QuestsController questsController;
    public EquipmentController equipmentController;
    public TravellingController travellingController;

    public GameObject heartPrefab;
    public Transform hpHeartsSpawn;
    public GameObject UIkeys;
    public GameObject[] interactables;
    public float radius;
    public InteractableObject target;
    public int maxHp;

    public Transform feedTransform;
    public GameObject feedPrefab;
    public int feed;
    public float feedTime;
    float feedTimer;

    float currentSpeed;

    public AreaHandler areaICurrentlyAm;

    List<GameObject> nearbyObjects = new List<GameObject>();
    Vector2 startPosDrag, currentPosDrag, outcomePosDrag;

    // TODO: napisac funkcje "checkHP"


    void Awake()
    {
        if (Player._instance == null)
        _instance = this;
        else
            Destroy(transform.parent.gameObject);
    }

    void Start()
    {
        DontDestroyOnLoad(transform.parent.gameObject);
        rb = GetComponent<Rigidbody2D>();
        DontDestroyOnLoad(this);

        CheckHP();

        for (int i = 0; i < feed; i++)
        {
            feedTransform.GetChild(i).gameObject.SetActive(true);
        }

        currentSpeed = speed;
    }

	void Update()
    {
        Feed();

        if (!isBusy)
        {
            ChangeSprite();

            rb.velocity = new Vector2(
                Input.GetAxis("Horizontal1"),
                Input.GetAxis("Vertical1") 
            ).normalized * currentSpeed;

            if (Input.GetButton("Run"))
            {
                currentSpeed = speed * 1.5f;
            }
            else
                currentSpeed = speed;

            LookingForTheNearestInteractiveObject();

            if (target != null)
            {
                if (Input.GetButtonDown("Challenge"))
                    target.LetsShoot();

                if (Input.GetButtonDown("Talk"))
                    target.LetsTalk();

                if (Input.GetButtonDown("Trade"))
                    target.LetsTrade();

                if (Input.GetButtonDown("Inspect"))
                    target.LetsSee();

                if (Input.GetButtonDown("Interact") && !target.GetComponent<InteractableObject>().notInteractable)
                    target.Interact();
            }

            if(Input.GetButtonDown("Equipment"))
                equipmentController.StartEquiping();

            if (Input.GetButtonDown("QuestLog"))
                questsController.StatViewingQuests();
        }
        else
            rb.velocity = Vector2.zero;


        transform.localPosition = new Vector3(
            transform.localPosition.x,
            transform.localPosition.y,
            transform.localPosition.y + 0
        );
	} 

    void Feed()
    {
        feedTimer += Time.deltaTime;

        if (feedTimer >= feedTime)
        {
            if (feed > 0)
            {
                feed--;

                feedTimer = 0f;

                feedTransform.GetChild(feedTransform.childCount - 1).gameObject.SetActive(false);
            }
            else
                GoToHospital();
        }
    }

    public void EatFood(int _i)
    {
        if (feed < 10)
        {
            feed += _i;

            for (int i = 0; i < _i; i++)
            {
                feedTransform.GetChild(feedTransform.childCount).gameObject.SetActive(true);
            }
        }
    }

    public void DrinkAlkohol(int _i)
    {
        int howManyEnabled = 0;

        foreach (Transform heart in heartsSpawn)
            if (heart.gameObject.activeInHierarchy)
                howManyEnabled++;
        
        if (hp < 10)
        {
            hp += _i;

            if (hp > 10)
                hp = 10;

            for (int i = howManyEnabled; i < hp; i++)
            {
                heartsSpawn.GetChild(heartsSpawn.childCount).gameObject.SetActive(true);
            }
        }
    }

    void GoToHospital()
    {
        Start();
    }

    void ChangeSprite()
    {
        if (Input.GetAxis("Horizontal1") != 0 || Input.GetAxis("Vertical1") != 0)
        {
            moveSpriteTimer += Time.deltaTime;

            if (moveSpriteTimer > moveSpriteSpeed)
            {
                moveI++;
                moveI %= moveSprites.Length;

                GetComponent<SpriteRenderer>().sprite = moveSprites[moveI];

                moveSpriteTimer = 0f;

                if (Input.GetAxis("Horizontal1") > 0)
                    transform.eulerAngles = new Vector3(0, 0, 0);
                else if (Input.GetAxis("Horizontal1") < 0)
                    transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
    }

    public void PrepareToShootout()
    {
        for (int i = 0; i < hp; i++)
        {
            Vector2 pos = new Vector2(
                i * (heartPrefab.GetComponent<RectTransform>().sizeDelta.y + 0),
                0
            );

            GameObject heart = Instantiate(heartPrefab, heartsSpawn) as GameObject;

            heart.transform.localScale = new Vector3(1, 1, 1);

            heart.transform.localPosition = pos;
        }

        isBusy = true;
    }

    public void ShootoutOver()
    {
        isBusy = false;

        foreach (Transform heart in heartsSpawn)
            Destroy(heart.gameObject);
    }

    void LookingForTheNearestInteractiveObject()
    {
        Vector3 playerPostion = new Vector3(transform.position.x, transform.position.y, 0);

        foreach (GameObject interactableObject in interactables)
        {
            Vector3 interactableObjectPosition = new Vector3(interactableObject.transform.position.x, interactableObject.transform.position.y, 0);

            if (Vector3.Distance(interactableObjectPosition, playerPostion) < interactableObject.GetComponent<InteractableObject>().howCloseINeedToApproach)
                nearbyObjects.Add(interactableObject);               
        }

        if (nearbyObjects.Count != 0)
        {
            GameObject nearest = nearbyObjects[0];

            Vector3 nearestObjectPositon = new Vector3(nearest.transform.position.x, nearest.transform.position.y, 0);

            if (nearbyObjects.Count > 0)
            {
                for (int i = 1; i < nearbyObjects.Count; i++)
                {
                    Vector3 targetPosition;

                    targetPosition = new Vector3(nearbyObjects[i].transform.position.x, nearbyObjects[i].transform.position.y, 0);


                    if (Vector3.Distance(targetPosition, playerPostion) < Vector3.Distance(nearestObjectPositon, playerPostion))
                        nearest = nearbyObjects[i];
                }
            }

            if (target != nearest.GetComponent<InteractableObject>() && target != null)
            {
                target.GetComponent<SpriteRenderer>().color = Color.white;

            }

            target = nearest.GetComponent<InteractableObject>();
            //target.transform.position = new Vector3(
            //    target.transform.position.x,
            //    target.transform.position.y, 
            //    0
            //    );
            if (!target.GetComponent<InteractableObject>().notInteractable)
                target.GetComponent<SpriteRenderer>().color = Color.green;
            else
                target.GetComponent<SpriteRenderer>().color = Color.red;

            UIkeys.SetActive(true);
        }
        else
        {
            if (target != null)
            {
                target.GetComponent<SpriteRenderer>().color = Color.white;
                target = null;

                UIkeys.SetActive(false);
            }
        }

        nearbyObjects.Clear();
    }

    public void DamageTaken()
    {
        heartsSpawn.GetChild(hp - 1).gameObject.SetActive(false);
        hp--;

        if (hp <= 0)
        {
            shootingController.ShootoutOver(gameObject);
        }
    }

    public void EndOfShooting(Person winner, Person looser, Gun playersGun)
    {
        if (shootingEnded != null)
            shootingEnded(winner, looser, playersGun);

        CheckHP();
    }

    void CheckHP()
    {
        for (int i = 0; i < hp; i++)
        {
            hpHeartsSpawn.GetChild(i).gameObject.SetActive(true);
        }

        for (int i = hp; i < hpHeartsSpawn.childCount; i++)
        {
            hpHeartsSpawn.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void EndOfTrade(Person trader, List<Item> boughtItems, int spentMoney)
    {
        if (tradeEnded != null)
            tradeEnded(trader, boughtItems, spentMoney);
    }

    public void EndOfInspection(Person inspected)
    {
        if (inspectionEnded != null)
            inspectionEnded(inspected);
    }

    public void EndOfTalk(InteractableObject speaker)
    {
        if (talkEnded != null)
            talkEnded(speaker);
    }

    public void Death()
    {
        GoToHospital();
    }
}