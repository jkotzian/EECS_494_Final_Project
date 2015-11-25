﻿using UnityEngine;
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
		//var setDevice = ControllerManager.S.allControllers[0];
		var setDevice = InputManager.Devices[conNum];
		
		//print ("My controller in Movement.Script is : " + setDevice.Name);
//
//		if(setDevice.Action1){
//			print ("I was pressed!");
//		}

        if (human.alive)
        {
			if ((Input.GetKey(detectiveMode) || setDevice.RightBumper ) && dModeTotal > -25f){
				if(dModeTotal > 0f){
					inDetectiveMode = true;
					//print("Currently in D-Mode");
				}
				dModeTotal -= Time.deltaTime * dModeLoss;
			}
			
			if (Input.GetKeyUp(detectiveMode) || !setDevice.DPadRight ){
				inDetectiveMode = false;
				//print("Left D-Mode");
			}

			if(dModeTotal < 100f){
				dModeTotal += Time.deltaTime * dModeRegain;
				//print("D - Total: " + dModeTotal);
			}

			if (Input.GetKey(rightKey) || setDevice.DPadRight)
            {
                transform.Translate(Vector3.right * Time.deltaTime * speed);
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
                transform.Translate(Vector3.left * Time.deltaTime * speed);
                // If the human is facing right, then flip
                if (human.facingRight)
                {
                    Vector3 newScale = this.gameObject.transform.localScale;
                    newScale.x *= -1;
                    this.gameObject.transform.localScale = newScale;
                    human.facingRight = false;
                }
            }

			if ((Input.GetKey(rightKey) || setDevice.DPadRight ) && isDetective && (Input.GetKey(boostKey) || setDevice.RightTrigger))
			{
				transform.Translate(Vector3.right * Time.deltaTime * speed * 1.5f);
			}
			if ((Input.GetKey(leftKey)|| setDevice.DPadLeft ) && isDetective && (Input.GetKey(boostKey)|| setDevice.RightTrigger))
			{
				transform.Translate(Vector3.left * Time.deltaTime * speed * 1.5f);
			}
            // Move up for ghosts
            if ((Input.GetKey(upKey) || setDevice.DPadUp) && isGhost)
            {
                transform.Translate(Vector3.up * Time.deltaTime * speed);
            }
            // Move up for ghosts
			if ((Input.GetKey(downKey) || setDevice.DPadDown) && isGhost)
            {
                transform.Translate(Vector3.down * Time.deltaTime * speed);
            }
        }
	}

	void OnCollisionEnter(Collision other){
		var setDevice = InputManager.Devices[conNum];
		if (other.gameObject.name == "Stairs") {
			//print("Stairs");

			if (Input.GetKey (upKey) || InputManager.Devices[conNum].DPadUp) {
				transform.Translate(Vector3.up*Time.deltaTime*speed);
			}
			if (Input.GetKey (downKey) || InputManager.Devices[conNum].DPadDown) {
				transform.Translate(Vector3.down*Time.deltaTime*speed);
			}
		}
	}
}
