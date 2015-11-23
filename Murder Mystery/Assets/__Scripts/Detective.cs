﻿using UnityEngine;
using System.Collections;

public class Detective : Human {
    private KeyCode         arrestKey;
    private Ghost[]      ghosts;

    public void setArrestKey(KeyCode key)
    {
        arrestKey = key;
    }
    
	void Awake () {
        // Set default arrest key
        setArrestKey(KeyCode.RightShift);
        // Link Ghost objects from GamePlay singleton GameObjects
        ghosts = new Ghost[2];
        ghosts[0] = GamePlay.S.Ghosts[0].GetComponent<Ghost>();
        ghosts[1] = GamePlay.S.Ghosts[1].GetComponent<Ghost>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(arrestKey))
        {
            Vector3 detectivePos = gameObject.transform.position;
            // Check for ghosts within arrest range
            for(int i=0; i<ghosts.Length; i++)
            {
                if (ghosts[i] && ghosts[i].alive)
                {
                    Vector3 murdererPos = ghosts[i].transform.position;
                    if ((detectivePos - murdererPos).magnitude < 1)
                    {
                        ghosts[i].Kill();
                    }
                }
            }
        }
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        ////Make Arrest
        //if (Input.GetKey(KeyCode.RightShift))
        //{
        //    // See if the object is a murderer
        //    Ghost murderer = collisionInfo.collider.gameObject.GetComponent<Ghost>();


        //}
    }
}
