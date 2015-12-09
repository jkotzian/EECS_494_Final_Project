﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Human : MonoBehaviour {
    public bool 		alive;
    public bool 		facingRight;
    public bool 		isStabbed;
	public float        delayPerSecond; 
	public GameObject	trackerPrefab;
    public KeyCode actionKey;

    public void setActionKey(KeyCode key)
    {
        actionKey = key;
    }

    // NOTE: NOTHING HAPPENS IN THE START FUNCTION FOR A BASE CLASS LIKE THIS

    void FixedUpdate() {
		if (isStabbed)
        {
            Kill();
            isStabbed = false;
        }
	}
	
    public void Kill()
    {
        // If it's an NPC that is killed
        NPC NPCToKill = GetComponent<NPC>();
        if (NPCToKill)
        {
            //Knock the murderer over
            if (NPCToKill.possessed)
				NPCToKill.dispossess();
            transform.Rotate(new Vector3(0, 0, 90));
            foreach(GameObject npc in GamePlay.S.NPCs)
            {
                npc.GetComponent<NPC>().speed += 0.5f;
            }
            if(TotalGame.S.round > 2 && alive)
            {
                TotalGame.S.bodyCount[TotalGame.S.round - 3]++;
            }
            alive = false;
        }
        // If it's a Ghost 
        Ghost GhostToKill = GetComponent<Ghost>();
        if (GhostToKill && alive)
        {
            transform.Rotate(new Vector3(0, 0, 90));
            alive = false;
        }
    }

    public int GetLayerMask(string[] layerNames)
    {
        int layerMask = 0;

        foreach (string layer in layerNames)
        {
            layerMask = layerMask | (1 << LayerMask.NameToLayer(layer));
        }

        return layerMask;
    }
}
