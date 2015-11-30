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

	void Start() {
        human = gameObject.GetComponent<Human>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //print(InputManager.Devices[conNum].LeftStick.Vector);
        //var setDevice = ControllerManager.S.allControllers[0];
        var setDevice = InputManager.Devices[conNum];
        
        //print("My controller in Movement.Script is : " + setDevice.Name);

        /*if (setDevice.Action1 || setDevice.DPadUp)
        {
            print("I was pressed!");
        }*/
        
        if (human.alive)
        {
            // DETECTIVE MODE AND BOOST NOT OFFICIALLY ACCEPTED BY THE DESIGN
			/*if ((Input.GetKey(detectiveMode) || setDevice.RightBumper) && dModeTotal > -25f){
				if(dModeTotal > 0f){
					inDetectiveMode = true;
					//print("Currently in D-Mode");
				}
				dModeTotal -= Time.deltaTime * dModeLoss;
			}
			
			if (Input.GetKeyUp(detectiveMode) || !setDevice.DPadRight)
            {
				inDetectiveMode = false;
				//print("Left D-Mode");
			}

			if(dModeTotal < 100f){
				dModeTotal += Time.deltaTime * dModeRegain;
				//print("D - Total: " + dModeTotal);
			}*/
            moveVec = Vector3.zero;
            //print("Left stick x = " + setDevice.LeftStickX);
            moveVec += new Vector3(setDevice.LeftStick.Vector.x, setDevice.LeftStick.Vector.y, 0.0f);

            if (Input.GetKey(rightKey) || setDevice.DPadRight)
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

			if (Input.GetKey(leftKey) || setDevice.DPadLeft)
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
			/*if ((Input.GetKey(rightKey) || setDevice.DPadRight ) && isDetective && (Input.GetKey(boostKey) || setDevice.RightTrigger))
			{
				transform.Translate(Vector3.right * Time.deltaTime * speed * 1.5f);
			}
			if ((Input.GetKey(leftKey) || setDevice.DPadLeft) && isDetective && (Input.GetKey(boostKey) || setDevice.RightTrigger))
			{
				transform.Translate(Vector3.left * Time.deltaTime * speed * 1.5f);
			}*/
            // Move up for ghosts
            if ((Input.GetKey(upKey) || setDevice.DPadUp) && isGhost)
            {
                moveVec += Vector3.up;
            }
            // Move up for ghosts
			if ((Input.GetKey(downKey) || setDevice.DPadDown) && isGhost)
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
        Vector2 stickUp = new Vector2(1.0f, 1.0f);
        Vector2 stickDown = new Vector2(1.0f, -1.0f);
        if (door)
        {
            // TODO TELL ME (JAMES KOTZIAN) TO CLEAN THIS UP
            if ((Input.GetKeyDown(upKey) ||
               (InputManager.Devices[conNum].LeftStick.Vector.x > -.3 && 
               InputManager.Devices[conNum].LeftStick.Vector.x < .3 &&
               InputManager.Devices[conNum].LeftStick.Vector.y > 0) ||
               InputManager.Devices[conNum].DPadUp) && door.above)
            {
                door.MoveUp(gameObject);
            }
            else if ((Input.GetKeyDown(downKey) ||
                     (InputManager.Devices[conNum].LeftStick.Vector.x > -.3 &&
                      InputManager.Devices[conNum].LeftStick.Vector.x < .3 &&
                      InputManager.Devices[conNum].LeftStick.Vector.y < 0) ||
                      InputManager.Devices[conNum].DPadDown) && door.below)
            {
                door.MoveDown(gameObject);
            }
        }
    }
}
