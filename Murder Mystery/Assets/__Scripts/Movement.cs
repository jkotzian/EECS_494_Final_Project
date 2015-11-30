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

	void OnGUI(){
		if (dModeTotal > 0) {
			GUI.Label (label, playerNum + Mathf.RoundToInt (dModeTotal) + "%");
		}

		if(dModeTotal < 0){
			GUI.Label (label, playerNum + "0%");
		}
	}

    void Awake() {
        setUDLRKeys(KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow);
    }

    void Start()
    {
        human = gameObject.GetComponent<Human>();
        if (InputManager.Devices.Count > 0)
        {
            controller = InputManager.Devices[conNum];
        }
        else
        {
            controller = null;
        }
    }
	// Update is called once per frame
	void FixedUpdate () {
        //print(InputManager.Devices[conNum].LeftStick.Vector);
        //var controller = ControllerManager.S.allControllers[0];

        //print("My controller in Movement.Script is : " + controller.Name);

        /*if (controller.Action1 || controller.DPadUp)
        {
            print("I was pressed!");
        }*/
        
        if (human.alive)
        {
            // DETECTIVE MODE AND BOOST NOT OFFICIALLY ACCEPTED BY THE DESIGN
			/*if ((Input.GetKey(detectiveMode) || controller.RightBumper) && dModeTotal > -25f){
				if(dModeTotal > 0f){
					inDetectiveMode = true;
					//print("Currently in D-Mode");
				}
				dModeTotal -= Time.deltaTime * dModeLoss;
			}
			
			if (Input.GetKeyUp(detectiveMode) || !controller.DPadRight)
            {
				inDetectiveMode = false;
				//print("Left D-Mode");
			}

			if(dModeTotal < 100f){
				dModeTotal += Time.deltaTime * dModeRegain;
				//print("D - Total: " + dModeTotal);
			}*/
            moveVec = Vector3.zero;
            
            // Controller specific stuff
            if (controller != null)
            {
                // Set the move vector in the direction of the left analog stick
                moveVec += new Vector3(controller.LeftStick.Vector.x, controller.LeftStick.Vector.y, 0.0f);
                // Check if the character changed direction using the analog stick
                if (controller.LeftStickX > 0 && !human.facingRight)
                {
                    Vector3 newScale = this.gameObject.transform.localScale;
                    newScale.x *= -1;
                    this.gameObject.transform.localScale = newScale;
                    human.facingRight = true;
                }
                else if (controller.LeftStickX < 0 && human.facingRight)
                {
                    Vector3 newScale = this.gameObject.transform.localScale;
                    newScale.x *= -1;
                    this.gameObject.transform.localScale = newScale;
                    human.facingRight = false;
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
                // If the human is facing right, then flip
                if (!human.facingRight)
                {
                    Vector3 newScale = this.gameObject.transform.localScale;
                    newScale.x *= -1;
                    this.gameObject.transform.localScale = newScale;
                    human.facingRight = true;
                }
            }

			if (Input.GetKey(leftKey) || (controller != null && controller.DPadLeft))
            {
                moveVec += Vector3.left;
                // If the human is facing right, then flip
                if (human.facingRight)
                {
                    Vector3 newScale = this.gameObject.transform.localScale;
                    newScale.x *= -1;
                    this.gameObject.transform.localScale = newScale;
                    human.facingRight = false;
                }
            }
			/*if ((Input.GetKey(rightKey) || controller.DPadRight ) && isDetective && (Input.GetKey(boostKey) || controller.RightTrigger))
			{
				transform.Translate(Vector3.right * Time.deltaTime * speed * 1.5f);
			}
			if ((Input.GetKey(leftKey) || controller.DPadLeft) && isDetective && (Input.GetKey(boostKey) || controller.RightTrigger))
			{
				transform.Translate(Vector3.left * Time.deltaTime * speed * 1.5f);
			}*/
            // Move up for ghosts
            if ((Input.GetKey(upKey) || (controller != null && controller.DPadUp)) && isGhost)
            {
                moveVec += Vector3.up;
            }
            // Move up for ghosts
			if ((Input.GetKey(downKey) || (controller != null && controller.DPadDown)) && isGhost)
            {
                moveVec += Vector3.down;
            }

            transform.Translate(moveVec * Time.deltaTime * speed);
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
            }
        }
    }
}
