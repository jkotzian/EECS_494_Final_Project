using UnityEngine;
using System.Collections;
using InControl;

public class Trap : MonoBehaviour {

    public bool activated;
    int timer;
    // The time at which the NPC is killed
    public int deathTime;
    // The time at which the animation is over
    public int animOverTime;
    NPC target;
    Animator animator;
    // Use this for initialization
    void Start()
    {
        activated = false;
        timer = 0;
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (activated)
        {
            ++timer;
            if (timer == deathTime)
            {
                target.Kill();
            }
            if (timer == animOverTime)
            {
                animator.SetTrigger("Done");
            }
        }
	}

    public void activate(NPC npc, int newDeathTime, int newAnimOverTime)
    {
        deathTime = newDeathTime;
        animOverTime = newAnimOverTime;
        activated = true;
        target = npc;
        animator.SetTrigger("Kill");
    }

    public void reset(NPC npc, int newDeathTime, int newAnimOverTime)
    {
        //print("Base case reset");
    }

    void OnTriggerStay(Collider other)
    {
        NPC npc = other.GetComponent<NPC>();
        if (npc == null || !npc.possessed)
            return;

        bool keyboardPressed = Input.GetKeyDown(npc.possessionOwner.actionKey);
        bool controllerPressed = (GamePlay.S.usingControllers && InputManager.Devices[npc.NPCMovement.conNum].Action1);
        if (keyboardPressed || controllerPressed)
        {
            activate(npc, 20, 25);
        }
    }
}
