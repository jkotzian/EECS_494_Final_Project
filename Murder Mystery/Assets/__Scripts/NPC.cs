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

    Animator animator;

    void Start()
    {
        checkMoveTimer = Random.Range(checkMoveTimerMin, checkMoveTimerMax + 1);
        moving = false;
        movingRight = false;
        target = false;
        rigidbody = GetComponent<Rigidbody>();
        NPCMovement = GetComponent<Movement>();
        animator = GetComponent<Animator>();
        facingRight = true;
    }

	// Update is called once per frame
	void Update () {
        if (!alive)
            return;
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
                    // TODO THIS CODE IS DUPLICATED IN MOVEMENT, CONSIDER CONSOLIDATING INTO HUMAN
                    // Flip the scale of the image if they are switching direction
                    if (facingRight)
                    {
                        Vector3 newScale = this.gameObject.transform.localScale;
                        newScale.x *= -1;
                        this.gameObject.transform.localScale = newScale;
                        facingRight = false;
                    }
                }
                // Go right
                else
                {
                    moving = true;
                    movingRight = true;
                    // DUPLICATE CODE
                    if (!facingRight)
                    {
                        Vector3 newScale = this.gameObject.transform.localScale;
                        newScale.x *= -1;
                        this.gameObject.transform.localScale = newScale;
                        facingRight = true;
                    }
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
                        
        // If the Ghost is possessed, they are not currently shrinkingInto the NPCs body, AND they hit the
        // possession key, then dispossess them
        if (possessed && !possessionOwner.shrinkingIntoBody && (Input.GetKeyDown(possessionOwner.actionKey) || 
           (GamePlay.S.usingControllers && InputManager.Devices[NPCMovement.conNum].RightTrigger.WasPressed)))
        {
			/* WE'RE GOING TO EXPERIMENT WITH THE GHOST ONLY BEING ABLE TO DISPOSSESS WHEN THEY KILL
              SOMEONE. NOTE, THERE IS A RACE CONDITION THAT YOU MUST DEAL WITH IF YOU UNCOMMENT THIS LINE
             dispossess();*/
        }

        // If the NPC is possessed, the movement script will determine the walking animation
        if (!possessed)
        {
            animator.SetBool("Moving", moving);
        }
                     
	}

    public void possess(Ghost possessor)
    {
        possessed = true;
        possessionOwner = possessor;
        possessorMovement = possessor.GetComponent<Movement>();
        
        possessor.possessing = true;
        possessor.possessedNPC = this;

        possessor.ShrinkIntoBody(transform.position);
        // Set the velocity to 0
        rigidbody.velocity = Vector3.zero;
        // Give it the same controller and controls as the ghost
        NPCMovement.setUDLRKeys(possessorMovement.upKey, possessorMovement.downKey,
                        possessorMovement.leftKey, possessorMovement.rightKey);
        NPCMovement.conNum = possessor.movement.conNum;
    }

    public void turnOnMovement()
    {
        // Enable the NPC's movement with the Ghost's controls
        NPCMovement.enabled = true;
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
            return;

        NPCMovement.checkForDoorInput(collider);
    }
}
