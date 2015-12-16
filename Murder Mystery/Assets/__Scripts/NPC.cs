using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class NPC : Human {

    public bool         active;
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
    public GameObject hint;

    Rigidbody rb;

    Animator animator;

    public GameObject glowObject;
    private Component glow;

    public GameObject negativeScorePrefab;
    Vector3 offset;
    public Camera detectiveCamera;

    // Percentage chance (1 - 100) that an NPC will
    // take an elevator if they land on one
    public int chanceToTakeElevator;
    // Chance that they would take an elevator twice in
    // a row (1 - 100)
    public int chanceToTakeElevatorAgain;
    public bool canTakeElevator;
    bool justTookElevator;
    Door availableElevator;

    public void setElevator(Door elevator)
    {
        availableElevator = elevator;
        canTakeElevator = true;
    }
    public void unsetElevator()
    {
        availableElevator = null;
        canTakeElevator = false;
    }

    void Start()
    {
        hint.SetActive(false);
        checkMoveTimer = Random.Range(checkMoveTimerMin, checkMoveTimerMax + 1);
        moving = false;
        movingRight = false;
        target = false;
        rb = GetComponent<Rigidbody>();
        NPCMovement = GetComponent<Movement>();
        animator = GetComponent<Animator>();
        facingRight = true;
        glow = glowObject.GetComponent("Halo");
        offset = new Vector3(0, 0.8f, -3);
        detectiveCamera = GameObject.Find("DetectiveCamera").GetComponent<Camera>();
    }

	// Update is called once per frame
	void Update () {
        if (!alive || !active)
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
                bool takeElevator = false;
                if (canTakeElevator)
                {
                    int takeElevatorNum = Random.Range(0, 101);
                    if (justTookElevator)
                    {
                        if (takeElevatorNum <= chanceToTakeElevatorAgain)
                        {
                            takeElevator = true;
                        }
                    }
                    else
                    {
                        if (takeElevatorNum <= chanceToTakeElevator)
                        {
                            takeElevator = true;
                        }
                    }
                }
                // If the NPC is going to take an elevator, choose
                // whether to go up or down
                if (takeElevator)
                {
                    bool canTakeAbove = true;
                    bool canTakeBelow = true;
                    if (!availableElevator.above)
                    {
                        canTakeAbove = false;
                    }
                    if (!availableElevator.below)
                    {
                        canTakeBelow = false;
                    }
                    // If the NPC can take the above AND below elevator, then
                    // randomley choose
                    if (canTakeAbove && canTakeBelow)
                    {
                        int chooseElevatorNum = Random.Range(0, 2);
                        if (chooseElevatorNum == 1)
                        {
                            availableElevator.MoveUp(gameObject);
                            checkMoveTimer = Random.Range(standingTimeMin, standingTimeMax + 1);
                            justTookElevator = true;
                            return;
                        }
                        else
                        {
                            availableElevator.MoveDown(gameObject);
                            checkMoveTimer = Random.Range(standingTimeMin, standingTimeMax + 1);
                            justTookElevator = true;
                            return;
                        }
                    }
                    else if (canTakeAbove)
                    {
                        availableElevator.MoveUp(gameObject);
                        checkMoveTimer = Random.Range(standingTimeMin, standingTimeMax + 1);
                        justTookElevator = true;
                        return;
                    }
                    else if (canTakeBelow)
                    {
                        availableElevator.MoveDown(gameObject);
                        checkMoveTimer = Random.Range(standingTimeMin, standingTimeMax + 1);
                        justTookElevator = true;
                        return;
                    }
                }
                justTookElevator = false;
                // Go right if blocked left
                if (blockedLeft)
                {
                    turnRight();
                }
                // Go left if blocked right
                else if (blockedRight)
                {
                    turnLeft();
                }
                // If the NPC is not blocked, then randomely determine the direction
                else
                {
                    // Randomely decide the direction of the NPC
                    int moveNextRandNum = Random.Range(0, 2);
                    // Go left
                    if (moveNextRandNum == 0)
                    {
                        turnLeft();
                    }
                    // Go right
                    else
                    {
                        turnRight();
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
           (NPCMovement.conNum < GamePlay.S.numControllers && InputManager.Devices[NPCMovement.conNum].Action1.WasPressed)))
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

    void turnLeft()
    {
        //print("NPC " + name + " moving left");
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

    void turnRight()
    {
        //print("NPC " + name + " moving right");
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

    public void possess(Ghost possessor)
    {
        possessed = true;      
        hint.SetActive(false);
        possessionOwner = possessor;
        possessorMovement = possessor.GetComponent<Movement>();
        
        possessor.possessing = true;
        possessor.possessedNPC = this;

        possessor.ShrinkIntoBody(transform.position);
        // Set the velocity to 0
        rb.velocity = Vector3.zero;
        // Give it the same controller and controls as the ghost
        NPCMovement.setUDLRKeys(possessorMovement.upKey, possessorMovement.downKey,
                        possessorMovement.leftKey, possessorMovement.rightKey);
        NPCMovement.conNum = possessor.movement.conNum;
        glow.GetType().GetProperty("enabled").SetValue(glow, true, null);
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
            //print("NPC " + name + "blocked right");
        }
        else
        {
            blockedLeft = true;
            //print("NPC " + name + "blocked left");
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
            //print("NPC " + name + "UNblocked right");
        }
        else
        {
            blockedLeft = false;
            //print("NPC " + name + "UNblocked left");
        }
    }
	public void dispossess(bool delay) {
		possessed = false;
		// Disable the movement
		NPCMovement.enabled = false;
		// Re-enable the possession owner wherever the NPC is with an offset
		Vector3 offset = new Vector3(0, .3f, 0);    
        possessionOwner.transform.position = gameObject.transform.position + offset;
        // Delay the ghost coming out of the body for trap kills
        if (delay)
            StartCoroutine(possessionOwner.enableGameObjectWithDelay());
        else
        {
            possessionOwner.gameObject.SetActive(true);
        }
        possessionOwner.possessing = false;
		possessionOwner = null;
        glow.GetType().GetProperty("enabled").SetValue(glow, false, null);
    }

    void OnTriggerStay(Collider collider)
    {
        if (!possessed)
        { 
            GhostCollider ghost = collider.GetComponent<GhostCollider>();
            if (ghost)
            {
                if (alive)
                {
                    hint.SetActive(true);
                }
                Ghost ghostScript = ghost.GetComponentInParent<Ghost>();
                if (ghostScript && ghostScript.possessing)
                {
                    hint.SetActive(false);
                }
            }

        }            
        else
        {
            hint.SetActive(false);
            NPCMovement.checkForDoorInput(collider);
        }     
    }

    void OnTriggerExit(Collider collider)
    {
        GhostCollider ghost = collider.GetComponent<GhostCollider>();
        if (ghost)
        {
            hint.SetActive(false);
        }
    }

    public IEnumerator PopUpNegativeScore()
    {
        GameObject popUp = Instantiate(negativeScorePrefab, detectiveCamera.WorldToViewportPoint(transform.position + offset), Quaternion.identity) as GameObject;
        popUp.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -.05f);          
        yield return new WaitForSeconds(1f);    
        DestroyObject(popUp);
    }
}
