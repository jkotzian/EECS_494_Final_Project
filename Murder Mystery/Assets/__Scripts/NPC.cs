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
    public AudioSource drum;

    Rigidbody rb;

    Animator animator;

    private GameObject indicator;

    public GameObject negativeScorePrefab;
    Vector3 offset;
    public GameObject mainCameraObj;
    Camera mainCamera;

    // Percentage chance (1 - 100) that an NPC will
    // take an elevator if they land on one
    public int chanceToTakeElevator;
    // Chance that they would take an elevator twice in
    // a row (1 - 100)
    public int chanceToTakeElevatorAgain;
    public bool canTakeElevator;
    bool justTookElevator;
    Door availableElevator;
    bool walkedOverTrap;

    public void setWalkedOverTrap(bool value)
    {
        walkedOverTrap = value;
    }

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
        if(!TotalGame.S.inReady)
        {
            hint.SetActive(false);
            mainCamera = mainCameraObj.GetComponent<Camera>();
        }
        checkMoveTimer = Random.Range(checkMoveTimerMin, checkMoveTimerMax + 1);
        moving = false;
        movingRight = false;
        target = false;
        rb = GetComponent<Rigidbody>();
        NPCMovement = GetComponent<Movement>();
        animator = GetComponent<Animator>();
        facingRight = true;
        offset = new Vector3(0, 0.8f, -3);
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
                // If for some reason the NPC is blocked both right and left,
                // randomely choose a direction
                if (blockedLeft && blockedRight)
                {
                    randomelyChooseDirection();
                }
                // Go right if blocked left
                else if (blockedLeft)
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
                    randomelyChooseDirection();
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
                        
        // If the Ghost is possessed, they are not currently shrinkingInto the NPCs body, and they are 
        // not currently over a trap, AND they hit the possession key, then dispossess them
        bool canDispossess = possessed && !walkedOverTrap &&
                            (Input.GetKeyDown(possessionOwner.actionKey) || (NPCMovement.conNum < GamePlay.S.numControllers &&
                             InputManager.Devices[NPCMovement.conNum].Action1.WasPressed));
        if (canDispossess)
        {
             //dispossess(false);
        }

        // If the NPC is possessed, the movement script will determine the walking animation
        if (!possessed && !TotalGame.S.inReady)
        {
            animator.SetBool("Moving", moving);
        }
                     
	}

    void randomelyChooseDirection()
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

        // Set the velocity to 0
        rb.velocity = Vector3.zero;
        // Give it the same controller and controls as the ghost
        NPCMovement.setUDLRKeys(possessorMovement.upKey, possessorMovement.downKey,
                        possessorMovement.leftKey, possessorMovement.rightKey);
        NPCMovement.conNum = possessor.movement.conNum;

        if (possessor.gameObject == GamePlay.S.Ghosts[0])
        {
            indicator = Instantiate(GamePlay.S.gIndicatorPrefab1, Vector3.zero, Quaternion.identity) as GameObject;
            indicator.transform.parent = transform;
            indicator.transform.localPosition = new Vector3(-0.03f, 0.15f, 0);
        }
        else if (possessor.gameObject == GamePlay.S.Ghosts[1])
        {
            indicator = Instantiate(GamePlay.S.gIndicatorPrefab2, Vector3.zero, Quaternion.identity) as GameObject;
            indicator.transform.parent = transform;
            indicator.transform.localPosition = new Vector3(-0.03f, 0.15f, 0);
        }
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
        if (indicator)
        {
            DestroyObject(indicator);
        }
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
        possessionOwner.currentState = Ghost.State.Idle;
		possessionOwner = null;
    }

    void OnTriggerStay(Collider collider)
    {
        if (!possessed)
        { 
            GhostCollider ghostColl = collider.GetComponent<GhostCollider>();
            if (ghostColl)
            {
                Ghost ghost = ghostColl.GetComponentInParent<Ghost>();
                if (ghost && ghost.currentState == Ghost.State.Possessing && !TotalGame.S.inReady)
                {
                    hint.SetActive(false);
                }
                if (alive && ghost && ghost.alive && !TotalGame.S.inReady)
                {
                    hint.SetActive(true);
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
        GameObject popUp = Instantiate(negativeScorePrefab, mainCamera.WorldToViewportPoint(transform.position + offset), Quaternion.identity) as GameObject;
        popUp.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -.05f);          
        yield return new WaitForSeconds(1f);    
        DestroyObject(popUp);
    }
}
