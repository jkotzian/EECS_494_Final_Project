﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;

public class Switch : MonoBehaviour {
	public Trap             trap;

	Rigidbody               rigidbody2Dimensional;
	float 					moveSpeed = 1f;
	float					rotationAmount = 90f;
	float					pushPower = 2.0f;
	bool					isInfected = false;
	bool					isChopped = false;
	bool					droppedLid = false;

	float 			reset = 0;
	float 			resetPerSecond = 0.2f;
	bool			flippedSwitch = false;

    void Start()
    {
        trap = GetComponent<Trap>();
    }
	
	// Update is called once per frame
	void Update () {
		if (flippedSwitch) {
			if (reset < 2) {
				reset += resetPerSecond * Time.deltaTime;
			} else {
				resetEnvironment();
			}
		}
	}

    void OnTriggerEnter(Collider other)
    {
        NPC npc = other.GetComponent<NPC>();
        if (npc && npc.possessed)
        {
            GamePlay.S.texts[4].text = "press 'A' to activate trap";
        }
    }

    void OnTriggerExit(Collider other)
    {
        NPC npc = other.GetComponent<NPC>();
        if (npc && npc.possessed)
        {
            GamePlay.S.texts[4].text = "";
        }
    }

	void OnTriggerStay(Collider other) {
		NPC npc = other.GetComponent<NPC>();
        if (npc == null || !npc.possessed)
            return;

        bool keyboardPressed = Input.GetKeyDown(npc.possessionOwner.possessKey);
        bool controllerPressed = (GamePlay.S.usingControllers && InputManager.Devices[npc.NPCMovement.conNum].Action1);
        if (keyboardPressed || controllerPressed)
        {
            // Say that the NPC is no longer possessed, so when the "unpossess" key is pressed, it kills
            // the NPC instead of dispossessing them.
            //npc.possessed = false;

            //print (switchNum);
            flippedSwitch = true;
            trap.activate(npc, 20, 25);
            
            /*if (switchNum == 1)
            {
                rigidbody2Dimensional = GamePlay.S.EnvironmentalObjects[0].GetComponent<Rigidbody>();
                rigidbody2Dimensional.useGravity = true;
            }
            else if (switchNum == 2)
            {
                other.GetComponent<BoxCollider>().enabled = false;
                npc.Kill();
            }
            else if (switchNum == 3)
            {
                Color tmpColor = GamePlay.S.EnvironmentalObjects[1].GetComponent<Renderer>().material.color;
                tmpColor.a = 0.5f;
                GamePlay.S.EnvironmentalObjects[1].GetComponent<Renderer>().material.color = tmpColor;
                Vector3 tmpPos = GamePlay.S.EnvironmentalObjects[1].GetComponent<Transform>().position;
                tmpPos.z = 0;
                GamePlay.S.EnvironmentalObjects[1].GetComponent<Transform>().position = tmpPos;
                npc.Kill();
            }
            else if (switchNum == 4)
            {
                GamePlay.S.EnvironmentalObjects[2].GetComponent<FlameTrap>().flameOn = true;
            }
            else if (switchNum == 5)
            {
                isInfected = true;
            }
            else if (switchNum == 6 && !isChopped)
            {
                GamePlay.S.EnvironmentalObjects[3].GetComponent<Transform>().Rotate(new Vector3(0, 0, 90));
                npc.Kill();
                isChopped = true;
            }
            else if (switchNum == 7 && !droppedLid)
            {
                print("Got in");
                GamePlay.S.EnvironmentalObjects[4].GetComponent<Transform>().Rotate(new Vector3(0, 0, 320));
                this.transform.Rotate(new Vector3(0, 0, -60));
                npc.Kill();
                droppedLid = true;
            }*/
        }
	}

	void resetEnvironment(){
		/*if (switchNum == 1) {
			rigidbody2Dimensional = GamePlay.S.EnvironmentalObjects [0].GetComponent<Rigidbody> ();
			rigidbody2Dimensional.useGravity = false;
			Vector3 tmpPos = rigidbody2Dimensional.position;
			tmpPos.y = 2.059f;
			rigidbody2Dimensional.position = tmpPos;
		} else if (switchNum == 3) {
			Vector3 tmpPos = GamePlay.S.EnvironmentalObjects [1].GetComponent<Transform> ().position;
			tmpPos.z = 1;
			GamePlay.S.EnvironmentalObjects [1].GetComponent<Transform> ().position = tmpPos;
		} else if (switchNum == 5) {

		} else if (switchNum == 6 && isChopped) {
			GamePlay.S.EnvironmentalObjects [3].GetComponent<Transform> ().Rotate (new Vector3 (0, 0, -90));
			isChopped = false;
		} else if (switchNum == 7 && droppedLid) {
			//print ("Got in");
			GamePlay.S.EnvironmentalObjects [4].GetComponent<Transform> ().Rotate (new Vector3 (0, 0, -320));
			this.transform.Rotate (new Vector3 (0, 0, 60));
			droppedLid = false;
		}*/
		flippedSwitch = false;
	}
}
