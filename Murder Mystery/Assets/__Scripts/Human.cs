using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Human : MonoBehaviour {
    public bool 		alive;
    public bool 		facingRight;
    public bool 		isStabbed;
	public float        delayPerSecond; 
	public GameObject	trackerPrefab;
    public KeyCode actionKey;
    public bool tutorialNPC;
    // Number 1 - 0 of the the objective number this completes for the
    // tutorial
    public int objectiveNum;

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
        if (NPCToKill != null && NPCToKill.alive)
        {
            transform.Rotate(new Vector3(0, 0, 90));
            foreach(GameObject npc in GamePlay.S.NPCs)
            {
                //npc.GetComponent<NPC>().speed += 0.5f;
            }
            TotalGame.S.bodyCount[TotalGame.S.round - 1]++;
            alive = false;
            // If it was a tutorial NPC, complete the objective
            if (tutorialNPC)
            {
                GamePlay.S.completedObjective(objectiveNum);
            }
        }
        // If it's a Ghost 
        Ghost GhostToKill = GetComponent<Ghost>();
        if (GhostToKill && alive)
        {
            if (TotalGame.S.bodyCount[TotalGame.S.round - 1] > 0) {
                TotalGame.S.bodyCount[TotalGame.S.round - 1]--;
            }
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
