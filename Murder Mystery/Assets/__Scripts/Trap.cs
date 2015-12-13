using UnityEngine;
using System.Collections;
using InControl;

public class Trap : MonoBehaviour {

    public bool activated;
    public GameObject hint;
    public GameObject scorePrefab;
    public Camera ghostCamera;
    Vector3 offset;
    int timer;
    // The time at which the NPC is killed
    public int deathTime;
    // The time at which the animation is over
    public int animOverTime;
    NPC target;
    Animator animator;
    public bool tutorialTrap;
    public int objectiveNum;

    // Use this for initialization
    void Start()
    {
        hint.SetActive(false);
        offset = new Vector3(0, 0.8f, -3);
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
                hint.SetActive(false);
                StartCoroutine(PopUpScore());
            }
            if (timer == animOverTime)
            {
                animator.SetTrigger("Done");
                if (tutorialTrap)
                {
                    GamePlay.S.completedObjective(objectiveNum);
                }
            }
        }
	}

    public void activate(NPC npc)
    {
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

        if (!activated)
        {
            hint.SetActive(true);
        }        
        bool keyboardPressed = Input.GetKeyDown(npc.possessionOwner.actionKey);
        // Say that the controller was pressed if there are available controllers
        bool controllerPressed = (npc.NPCMovement.conNum < GamePlay.S.numControllers && 
                                  InputManager.Devices[npc.NPCMovement.conNum].Action1.WasPressed);
        // If the ghost is in the middle of possessing and shrinking into the body,
        // then don't say the controller button was pressed
        if (npc.possessed && npc.possessionOwner.shrinkingIntoBody)
        {
            controllerPressed = false;
        }
        if (keyboardPressed || controllerPressed)
        {
            activate(npc);
        }
    }

    void OnTriggerExit(Collider other)
    {
        NPC npc = other.GetComponent<NPC>();
        if(npc && npc.possessed)
        {                         
            hint.SetActive(false);
        }
    }

    IEnumerator PopUpScore()
    {
        GameObject popUp = Instantiate(scorePrefab, ghostCamera.WorldToViewportPoint(transform.position + offset), Quaternion.identity) as GameObject;
        popUp.GetComponent<Rigidbody2D>().velocity = new Vector2(0, .05f);
        yield return new WaitForSeconds(1f);
        DestroyObject(popUp);
    }
}
