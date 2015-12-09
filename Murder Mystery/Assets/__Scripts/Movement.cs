using UnityEngine;
using System.Collections;
using InControl;

public class Movement : MonoBehaviour {
    public float           speed;

    public KeyCode         upKey;
    public KeyCode         downKey;
    public KeyCode         leftKey;
    public KeyCode         rightKey;
    public KeyCode         boostKey;
	public KeyCode		   detectiveMode;	
    private Human human;

    public bool moving;
	public bool isDetective;
	public bool inDetectiveMode;
	public bool isGhost;

	public GameObject dModeBar;
	public float dModeTotal;
	public float dModeLoss;
	public float dModeRegain;
	public Rect label;
	public string playerNum;

	public InputDevice inputDevice;
	public int conNum;

    Vector3 moveVec;
    InputDevice controller;
    Animator animator;

    public Transform flashlightRightObj;
    public Transform flashlightLeftObj;
    public Transform weaponEffectObj;

    public void setUDLRKeys(KeyCode up, KeyCode down, KeyCode left, KeyCode right) {
        upKey = up;
        downKey = down;
        leftKey = left;
        rightKey = right;

		dModeTotal = 100f;
		dModeLoss = 45f;
		dModeRegain = 10f;
    }

	public void setBoostKey(KeyCode boost, KeyCode dMode) {
		boostKey = boost;
		detectiveMode = dMode;
	}

	
	public void setLabel(int x, int y, string playernum){
		label.x = x;
		label.y = y;
		label.width = 300;
		label.height = 300;
		playerNum = playernum;
	}

    void Awake() {
        setUDLRKeys(KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow);
    }

    void Start()
    {
        if (isDetective)
        {
            flashlightLeftObj.gameObject.SetActive(false);
        }

        human = gameObject.GetComponent<Human>();
        animator = GetComponent<Animator>();
        
        if (InputManager.Devices.Count > 0)
        {
            controller = InputManager.Devices[conNum];
        }
        else
        {
            controller = null;
        }
        moving = false;
    }
	// Update is called once per frame
	void FixedUpdate () {
        if (!human.alive)
            return;

        moveVec = Vector3.zero;
        moving = false;
            
        // Controller specific stuff
        if (controller != null)
        {
            // Set the move vector in the direction of the left analog stick
            moveVec += new Vector3(controller.LeftStick.Vector.x, controller.LeftStick.Vector.y, 0.0f);
            moving = true;
            // Check if the character changed direction using the analog stick
            if (controller.LeftStickX > 0 && !human.facingRight)
            {
                flip(true);
            }
            else if (controller.LeftStickX < 0 && human.facingRight)
            {
                flip(false);
            }
        }

        // Only allow upward and dowward movement for the ghost
        if (!isGhost)
        {
            moveVec.y = 0;
        }

        if (Input.GetKey(rightKey) || (controller != null && controller.DPadRight))
        {
            moveVec += Vector3.right;
            moving = true;
            // If the human is facing right, then flip
            if (!human.facingRight)
            {
                flip(true);
            }
        }

		if (Input.GetKey(leftKey) || (controller != null && controller.DPadLeft))
        {
            moveVec += Vector3.left;
            moving = true;
            // If the human is facing right, then flip
            if (human.facingRight)
            {
                flip(false);
            }
        }
        // Move up for ghosts
        if ((Input.GetKey(upKey) || (controller != null && controller.DPadUp)) && isGhost)
        {
            moveVec += Vector3.up;
            moving = true;
        }
        // Move up for ghosts
		if ((Input.GetKey(downKey) || (controller != null && controller.DPadDown)) && isGhost)
        {
            moveVec += Vector3.down;
            moving = true;
        }

        transform.Translate(moveVec * Time.deltaTime * speed);
        animator.SetBool("Moving", moving);
	}

    void flip (bool right)
    {
        Vector3 newScale = this.gameObject.transform.localScale;
        newScale.x *= -1;
        this.gameObject.transform.localScale = newScale;
        human.facingRight = right;
        // Manage flashlights
        if (isDetective)
        {
            if (right)
            {
                flashlightRightObj.gameObject.SetActive(true);
                flashlightLeftObj.gameObject.SetActive(false);
                Vector3 newRot = new Vector3(0, 180f, 0);
                weaponEffectObj.Rotate(newRot);
            }
            else
            {
                flashlightRightObj.gameObject.SetActive(false);
                flashlightLeftObj.gameObject.SetActive(true);
                Vector3 newRot = new Vector3(0, -180f, 0);
                weaponEffectObj.Rotate(newRot);
            }
        }
    }

    // Door movement
    public void checkForDoorInput(Collider collider)
    {  
        Door door = collider.GetComponent<Door>();
        float analogAngleLeeway = .3f;
        float analogThrustThreshold = .9f;
        if (door)
        {
            // TODO TELL ME (JAMES KOTZIAN) TO CLEAN THIS UP
            if ((Input.GetKeyDown(upKey) || 
               (controller != null && ((controller.LeftStick.Vector.x > -analogAngleLeeway && 
               controller.LeftStick.Vector.x < analogAngleLeeway &&
               controller.LeftStick.Vector.y > 0 &&
               controller.LeftStickY > analogThrustThreshold) ||
               controller.DPadUp))) && 
               door.above)
            {
                door.MoveUp(gameObject);
				//gameObject.GetComponent<SpriteRenderer>().enabled = false;

            }
            else if ((Input.GetKeyDown(downKey) ||
                     (controller != null && ((controller.LeftStick.Vector.x > -analogAngleLeeway &&
                      controller.LeftStick.Vector.x < analogAngleLeeway &&
                      controller.LeftStick.Vector.y < 0 &&
                      controller.LeftStickY < -analogThrustThreshold) ||
                      controller.DPadDown))) && 
                      door.below)
            {
                door.MoveDown(gameObject);
				//gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
}
