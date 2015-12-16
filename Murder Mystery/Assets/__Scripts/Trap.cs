using UnityEngine;
using System.Collections;
using SpriteShatter;
using InControl;

public class Trap : MonoBehaviour {

    public bool activated;
    public GameObject hint;
    public GameObject scorePrefab;
    public Camera ghostCamera;
	public AudioSource TrapMusic;
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

    public bool fallingTrap;
    public bool swordTrap;

    public Transform sword1;
    public Transform sword2;

    public Sprite bloodySword;

    public int startingRot1;
    public int endingRot1;

    public int startingRot2;
    public int endingot2;

    public Rigidbody rigidbody;
    bool rotating;
    // Use this for initialization
    void Start()
    {
        hint.SetActive(false);
        offset = new Vector3(0, 0.8f, -3);
        activated = false;
        rotating = false;
        timer = 0;
        animator = GetComponent<Animator>();
        if (fallingTrap)
            rigidbody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (rotating)
        {
            sword1.Rotate(Vector3.forward, -500.0f * Time.deltaTime);
            sword2.Rotate(Vector3.forward, 500.0f * Time.deltaTime);

            if (sword1.rotation.eulerAngles.z <= 118.0f) {
                rotating = false;
            }
        }
        if (activated)
        {
            ++timer;
            if (timer == deathTime)
            {
                target.dispossess(true);
                target.Kill();
                // If it's a sword kill, make them bloody
                if (swordTrap)
                {
                    sword1.GetComponent<SpriteRenderer>().sprite = bloodySword;
                    sword2.GetComponent<SpriteRenderer>().sprite = bloodySword;
                    target.GetComponent<Shatter>().shatter();
                }
                hint.SetActive(false);
                StartCoroutine(PopUpScore());
            }
            if (timer == animOverTime)
            {
                if (!swordTrap)
                    animator.SetTrigger("Done");
                if (tutorialTrap)
                {
                    GamePlay.S.completedObjective(objectiveNum);
                }
                if (fallingTrap)
                {
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.useGravity = false;
                }
            }
        }
	}

    public void activate(NPC npc)
    {
        // Only activate if it is not activated
        if (activated)
            return;
        activated = true;
        target = npc;
        if (!swordTrap)
            animator.SetTrigger("Kill");
        // If it's a falling trap, have it fall by enabling gravity
        if (fallingTrap)
        {
            rigidbody.useGravity = true;
        }
        if (swordTrap)
        {
            rotating = true;
        }
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
			//TrapMusic.Play();
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
