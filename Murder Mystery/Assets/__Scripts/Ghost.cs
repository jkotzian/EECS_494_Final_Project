using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ghost : Human
{
    private KeyCode possessKey;
    public RaycastHit hitInfo;
    public int bloodDropTimeMax;
    public int bloodDropInterval;
    int bloodDropTimer;

    public int newInterval;

    List<GameObject> bloodTrail;

    public GameObject possessObjRef;
    GameObject currentPossessObj;

    bool tracked;

    public void setPossessKey(KeyCode key)
    {
        possessKey = key;
    }

    void Awake()
    {
        // Default murder key
        setPossessKey(KeyCode.Space);
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
        if (Input.GetKeyDown(possessKey) && !currentPossessObj)
        {
            Vector3 possessObjPos = transform.position;
            Vector3 possessObjOffset;
            if (facingRight)
            {
                possessObjOffset = Vector3.right / 1.3f;
            }
            else
            {
                possessObjOffset = Vector3.left / 1.3f;
            }
            possessObjPos += possessObjOffset;
            currentPossessObj = Instantiate(possessObjRef, possessObjPos, transform.rotation) as GameObject;

            // Get the possess hit script
            PossessHit possessHit = currentPossessObj.GetComponent<PossessHit>();
            possessHit.ghostOwner = this;
            // Make sure to set its offset!!!
            possessHit.offset = possessObjOffset;
        }

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

    void getPossessTarget()
    {

    }

    public void possess()
    {

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

