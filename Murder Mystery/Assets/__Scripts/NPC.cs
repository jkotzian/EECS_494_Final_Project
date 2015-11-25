using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class NPC : Human {

    // These things should most definitely be kept public!
    public int         checkMoveTimerMin;
    public int         checkMoveTimerMax;
    public int         standingTimeMin;
    public int         standingTimeMax;
    int                checkMoveTimer;

    private bool        moving;
    private bool        movingRight;
    public bool         blockedRight;
    public bool         blockedLeft;
    public  float       speed;
    public bool         target;

    public bool possessed;
    public Ghost possessionOwner;
    public Movement NPCMovement;
    public Movement possessorMovement;

    Rigidbody rigidbody;

	// Awake might not be the best place for initialization
	void Awake () {
        // Set default timer values
        //setTimerValues(40, 70, 150);
        // Set default values
        //moving = false;
        //movingRight = false;
	}

    void Start()
    {
        checkMoveTimer = Random.Range(checkMoveTimerMin, checkMoveTimerMax + 1);
        moving = false;
        movingRight = false;
        target = false;
        rigidbody = gameObject.GetComponent<Rigidbody>();
        NPCMovement = GetComponent<Movement>();
    }

	// Update is called once per frame
	void Update () {
		if (possessionOwner != null) {
			//if the npc is possessed, just checking controllers
			print ("Posseser Controller #: " + possessionOwner.conNum);
			NPCMovement.conNum = possessionOwner.conNum;
			print ("NPC Movement  #: " + NPCMovement.conNum);
		}
		
        if (checkMoveTimer > 0 && !possessed)
        {
            --checkMoveTimer;

            if (checkMoveTimer == 0)
            {
                // If the NPC was moving, have it stand still
                if (moving)
                {
                    moving = false;
                    checkMoveTimer = Random.Range(standingTimeMin, standingTimeMax + 1);
                    return;
                }
                // Randomely decide the direction of the NPC
                int moveNextRandNum = Random.Range(0, 2);
                // Go left
                if (blockedRight || moveNextRandNum == 0)
                {
                    moving = true;
                    movingRight = false;
                    facingRight = false;
                }
                // Go right
                else
                {
                    moving = true;
                    movingRight = true;
                    facingRight = true;
                }
                // Randomely set the next time the NPC will check its direction between the min and the max
                checkMoveTimer = Random.Range(checkMoveTimerMin, checkMoveTimerMax + 1);
                //Debug.Log(this.gameObject.name + " facing right = " + facingRight);
            }
        }

        if (moving && !possessed)
        {
            if (movingRight)
            {
                transform.Translate(Vector3.right * Time.deltaTime * speed);
            }
            else
            {
                transform.Translate(Vector3.left * Time.deltaTime * speed);
            }
        }

        if (possessed && Input.GetKeyDown(possessionOwner.possessKey))
        {
			dispossess();
        }
	}

    public void possess(Ghost possessor)
    {
        possessed = true;
        possessionOwner = possessor;
        possessorMovement = possessor.GetComponent<Movement>();
        
        // Enable the NPC's movement with the Ghost's controls
        NPCMovement.enabled = true;
        NPCMovement.setUDLRKeys(possessorMovement.upKey, possessorMovement.downKey,
                                possessorMovement.leftKey, possessorMovement.rightKey);
        // Disable the ghost
        possessor.gameObject.SetActive(false);
        // Set the velocity to 0
        rigidbody.velocity = Vector3.zero;
    }

    public void block(bool right)
    {
        if (right)
        {
            blockedRight = true;
        }
        else
        {
            blockedLeft = true;
        }

        // Have the NPC recalculate it's next move (
        // it will also stop and see that it's blocked)
        // Setting it to 1 will make it go just a tid bit further
        // before stopping and re-evaluating
        checkMoveTimer = 1;
    }

    public void unblock(bool right)
    {
        if (right)
        {
            blockedRight = false;
        }
        else
        {
            blockedLeft = false;
        }
    }
	public void dispossess() {
		possessed = false;
		// Disable the movement
		NPCMovement.enabled = false;
		// Re-enable the possession owner wherever the NPC is with an offset
		Vector3 offset = new Vector3(0, .3f, 0);
		possessionOwner.transform.position = gameObject.transform.position + offset;
		possessionOwner.gameObject.SetActive(true);
        possessionOwner.possessing = false;
		possessionOwner = null;
	}

    void OnTriggerStay(Collider collider)
    {
        if (!possessed)
        {
            return;
        }
        Door door = collider.GetComponent<Door>();
        if (door)
        {
            if ((Input.GetKeyDown(possessorMovement.upKey) || InputManager.Devices[possessionOwner.conNum].DPadUp ) && door.above)
            {
                door.MoveUp(gameObject);
            }
			else if ((Input.GetKeyDown(possessorMovement.downKey) || InputManager.Devices[possessionOwner.conNum].DPadDown) && door.below)
            {
                door.MoveDown(gameObject);
            }
        }
    }
}
