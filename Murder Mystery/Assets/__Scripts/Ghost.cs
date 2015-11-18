﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ghost : Human
{
    private KeyCode murderKey;
    public RaycastHit hitInfo;
    public int bloodDropTimeMax;
    public int bloodDropInterval;
    int bloodDropTimer;

    public int newInterval;

    List<GameObject> bloodTrail;
    bool tracked;

    public void setMurderKey(KeyCode key)
    {
        murderKey = key;
    }

    void Awake()
    {
        // Default murder key
        setMurderKey(KeyCode.Space);
    }

    void Start()
    {
        bloodTrail = new List<GameObject>();
        bloodDropTimer = 0;
        tracked = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.deltaTime % bloodDropInterval == 0f)
        {
            GameObject blood = Instantiate(trackerPrefab, transform.position, Quaternion.identity) as GameObject;
        }
        // Use this code if we want tracking 
        if (tracked)
        {
            if (bloodDropTimer % bloodDropInterval == 0)
            {
                // Drop the blood
                GameObject blood = Instantiate(trackerPrefab, transform.position, Quaternion.identity) as GameObject;
                bloodTrail.Add(blood);
            }

            bloodDropTimer++;

            if (bloodDropTimer == bloodDropTimeMax)
            {
                tracked = false;
                bloodDropTimer = 0;
            }
        }
    }

    public void track()
    {
        // If the murderer is already being tracked, just reset the tracked timer
        if (tracked)
        {
            bloodDropTimer = 0;
        }
        tracked = true;
    }
}

