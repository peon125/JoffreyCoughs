﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticValues : MonoBehaviour
{
    public static StaticValues _instance;
    public GameObject bundlePrefab;
    //public Transform bundlesTransform;
    public GameObject enemyHeartPrefab;

    void Awake()
    {
        _instance = this;
    }
}