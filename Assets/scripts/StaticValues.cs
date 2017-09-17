using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticValues : MonoBehaviour
{
    public static StaticValues _instance;
    public GameObject bundlePrefab;
    public GameObject enemyHeartPrefab;
    public AudioClip doorOpeningSound;
    public AudioClip doorClosingSound;
    public AudioClip doorKnockingSound;
    public AudioClip barrelKnockingSound;

    void Awake()
    {
        _instance = this;
    }
}