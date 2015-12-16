using UnityEngine;
using System.Collections;
using InControl;

public class Door : MonoBehaviour {

    public Door above;
    public Door below;
	public float reappear;

    public GameObject detectiveGlowObj;
    public GameObject possessedNPCGlowObj;
    private Component detectiveGlow;
    private Component possessedNPCGlow;
    Animator animator;
    bool openingClosing;
    int timer;
    public int openCloseTime;
    public int delay;

    public void Awake()
    {
        detectiveGlow = detectiveGlowObj.GetComponent("Halo");
        possessedNPCGlow = possessedNPCGlowObj.GetComponent("Halo");
        animator = GetComponent<Animator>();
        openingClosing = false;
    }

    void FixedUpdate()
    {
        if (timer > 0)
        {
            if (timer == openCloseTime)
            {
                // Play the open close animation
                animator.SetTrigger("OpenClose");
            }
            --timer;
            if (timer == 0)
            {
                animator.SetTrigger("Done");
            }
        }
    }
    public void MoveUp(GameObject passenger)
    {                                
        MoveHelper(passenger, above);
    }

    public void MoveDown(GameObject passenger)
    {
        MoveHelper(passenger, below);
    }

    void MoveHelper(GameObject passenger, Door destDoor)
    {
        NPC npc = passenger.GetComponent<NPC>();
        Vector3 dest = new Vector3(destDoor.transform.position.x, destDoor.transform.position.y, -0.2f);
        passenger.transform.position = dest;
        if (npc)
        {
            npc.hint.SetActive(false);
        }
        passenger.SetActive(false);
        StartCoroutine(EnableSprite(passenger));
        DeactivateDetectiveGlow(true);
        DeactivateNPCGlow(true);
        // Only play the animation if it's not already playing
        if (timer == 0)
            timer = openCloseTime;
        // Play a delayed animation for destination elevator
        destDoor.PlayOpenCloseAnimationWithDelay();
    }

    public void PlayOpenCloseAnimationWithDelay()
    {
        if (timer == 0)
            timer = openCloseTime + delay;
    }

    public void ActivateDetectiveGlow(bool source = false)
    {
        detectiveGlow.GetType().GetProperty("enabled").SetValue(detectiveGlow, true, null);
        if (source)
        {
            if (above)
            {
                above.ActivateDetectiveGlow();
            }
            if (below)
            {
                below.ActivateDetectiveGlow();
            }
        }
    }

    public void DeactivateDetectiveGlow(bool source = false)
    {
        detectiveGlow.GetType().GetProperty("enabled").SetValue(detectiveGlow, false, null);
        if (source)
        {
            if (above)
            {
                above.DeactivateDetectiveGlow();
            }
            if (below)
            {
                below.DeactivateDetectiveGlow();
            }
        }
    }

    public void ActivateNPCGlow(bool source = false)
    {
        possessedNPCGlow.GetType().GetProperty("enabled").SetValue(possessedNPCGlow, true, null);
        if (source)
        {
            if (above)
            {
                above.ActivateNPCGlow();
            }
            if (below)
            {
                below.ActivateNPCGlow();
            }
        }
    }

    public void DeactivateNPCGlow(bool source = false)
    {
        possessedNPCGlow.GetType().GetProperty("enabled").SetValue(possessedNPCGlow, false, null);
        if (source)
        {
            if (above)
            {
                above.DeactivateNPCGlow();
            }
            if (below)
            {
                below.DeactivateNPCGlow();
            }
        }
    }

    IEnumerator EnableSprite(GameObject passenger){
		yield return new WaitForSeconds (.5f);
//		if (passenger.GetComponent<SpriteRenderer> ().enabled == false) {
//			passenger.GetComponent<SpriteRenderer> ().enabled = true;
//		}
		passenger.SetActive (true);
	}

    void OnTriggerStay(Collider other){
        /*Detective detective = other.GetComponent<Detective>();
        NPC npc = other.GetComponent<NPC>();
        if (detective)
        {
            // Activate glow
            print("hit1");
            ActivateDetectiveGlow(true);
        }
        if (npc)
        {
            if (npc.possessed)
            {
                // Activate glow
                ActivateNPCGlow(true);
            }
            // Tell the NPC that they could take this elevator
            npc.setElevator(this);
        }*/                                           
	}

    void OnTriggerEnter(Collider other)
    {
        Detective detective = other.GetComponent<Detective>();
        NPC npc = other.GetComponent<NPC>();
        if (detective)
        {
            // Activate glow
            print("hit2");
            ActivateDetectiveGlow(true);
        }
        if (npc)
        {
            if (npc.possessed)
            {
                // Activate glow
                ActivateNPCGlow(true);
            }
            // Tell the NPC that they could take this elevator
            npc.setElevator(this);
        }   
    }

    void OnTriggerExit(Collider other)
    {
        Detective detective = other.GetComponent<Detective>();
        NPC npc = other.GetComponent<NPC>();       
        if (detective)
        {
            // Activate glow
            print("hit3");
            DeactivateDetectiveGlow(true);
        }
        if (npc)
        {
            // Tell the NPC they can no longer take this elevator
            npc.unsetElevator();
            if (npc.possessed)
            {
                // Activate glow
                DeactivateNPCGlow(true);
            }
        }
    }
}
