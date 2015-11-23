using UnityEngine;
using System.Collections;

public class NPCStopZone : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        // Get the NPC
        NPC npc = other.gameObject.GetComponent<NPC>();
        if (npc)
        {
            // If the NPC was moving right, then block it on the right
            if (npc.facingRight)
            {
                npc.block(true);
                //Debug.Log(other.gameObject.name + " Blocked Right");
            }
            else
            {
                npc.block(false);
                //Debug.Log(other.gameObject.name + " Blocked Left");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Get the NPC
        NPC npc = other.gameObject.GetComponent<NPC>();
        if (npc)
        {
            // If the NPC was moving right, then block it on the right
            if (!npc.facingRight)
            {
                npc.unblock(true);
            }
            else
            {
                npc.unblock(false);
            }
        }
    }
}
