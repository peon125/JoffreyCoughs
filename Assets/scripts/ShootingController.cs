using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    public List<KeyCode> takenKeys = new List<KeyCode>();
    public GameObject squarePrefab;
    public Transform squareSpawn;
    public int[] cordsX;
    public int[] cordsY;
    public GameObject[] stuffToEnable;
    int howManySquaresToCreate;
    int squaresCount = 0;
    List<int> takenCordsX;
    List<int> takenCordsY;
    Player shootingPlayer;
    public Person shootingEnemy;

    public void ShallWeBegin(Person enemy)
    {
        Player._instance.PrepareToShootout();
        shootingEnemy = enemy;
        shootingEnemy.PrepareToShootout();
        takenCordsX = new List<int>();
        takenCordsY = new List<int>();
        howManySquaresToCreate = Player._instance.gun.hands;

        foreach (GameObject child in stuffToEnable)
            child.gameObject.SetActive(true);
    }

    void CreateASquare()
    {
        int x = 0, y = 0;

        do
        {
            x = Random.Range(0, cordsX.Length);
            y = Random.Range(0, cordsY.Length);

            x = cordsX[x];
            y = cordsY[y];

        } while(takenCordsX.Contains(x) && takenCordsY.Contains(y));

        GameObject square = Instantiate(
            squarePrefab,
            Vector2.zero,
            new Quaternion(0, 0, 0, 0), 
            squareSpawn
        ) as GameObject;

        square.GetComponent<Square>().howLongLasts *= Player._instance.gun.speed;
        square.transform.localPosition = new Vector2(x, y);

        takenCordsX.Add(x);
        takenCordsY.Add(y);

        squaresCount++;
    }

    public void PermissionWasGiven()
    {
        for (int i = 0; i < howManySquaresToCreate; i++) 
            CreateASquare();
    }

    public void SquareDestroyed(Transform squareTransform)
    {
        CreateASquare();

        takenCordsX.Remove((int)squareTransform.localPosition.x);
        takenCordsY.Remove((int)squareTransform.localPosition.y);

        squaresCount--;
    }

    public void ShotMissed()
    {
        Player._instance.DamageTaken();
    }

    public void ShootoutOver(GameObject haHaLooser)
    {
        foreach (Transform square in squareSpawn)
            Destroy(square.gameObject);
        
        foreach (GameObject child in stuffToEnable)
            child.gameObject.SetActive(false);

        shootingEnemy.ShootoutOver();

        Player._instance.ShootoutOver();

        if (haHaLooser == shootingEnemy)
            Destroy(haHaLooser);
        else
            Debug.Log("przypal xD");
    }
}