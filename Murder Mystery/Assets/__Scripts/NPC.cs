using UnityEngine;
using System.Collections;

public class NPC : Human {

    // These things should most definitely be kept public!
    public int         checkMoveTimerMin;
    public int         checkMoveTimerMax;
    public int         standingTime;
    int                checkMoveTimer;

    private bool        moving;
    private bool        movingRight;
    public  float       speed;
    public bool         target;

    public bool possessed;
    public Ghost possessionOwner;
    public Movement NPCMovement;

    Rigidbody rigidbody;
    // Much better to set these values in the inspector for quick
    // iteration rather than hard code it in a function like this
    public void setTimerValues(int min, int max, int standing)
    {
        checkMoveTimerMin = min;
        checkMoveTimerMax = max;
        standingTime = standing;
        // Randomely set the next time the NPC will check its direction between the min and the max
        checkMoveTimer = Random.Range(checkMoveTimerMin, checkMoveTimerMax + 1);
    }

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
        if (checkMoveTimer > 0 && !possessed)
        {
            --checkMoveTimer;
            if (checkMoveTimer == 0)
            {
                // If the NPC was moving, have it stand still
                if (moving)
                {
                    moving = false;
                    checkMoveTimer = standingTime;
                    return;
                }
                // Randomely decide the next movement of the NPC
                int moveNextRandNum = Random.Range(0, 101);
                // Go left
                if (moveNextRandNum < 50)
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

        if (possessionOwner && Input.GetKeyDown(possessionOwner.possessKey))
        {
            possessed = false;
            // Disable the movement
            NPCMovement.enabled = false;
            // Re-enable the possession owner wherever the NPC is with an offset
            Vector3 offset = new Vector3(0, .3f, 0);
            possessionOwner.transform.position = gameObject.transform.position + offset;
            possessionOwner.gameObject.SetActive(true);
            possessionOwner = null;
        }
	}

    public void possess(Ghost possessor)
    {
        possessed = true;
        possessionOwner = possessor;
        Movement possessorMovement = possessor.GetComponent<Movement>();
        
        // Enable the NPC's movement with the Ghost's controls
        NPCMovement.enabled = true;
        NPCMovement.setUDLRKeys(possessorMovement.upKey, possessorMovement.downKey,
                                possessorMovement.leftKey, possessorMovement.rightKey);
        // Disable the ghost
        possessor.gameObject.SetActive(false);
        // Set the velocity to 0
        rigidbody.velocity = Vector3.zero;
    }
}
