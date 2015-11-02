using UnityEngine;
using System.Collections;

public class Environment : MonoBehaviour {

	bool					beenClicked = false, turnedOff = false;
	public float 			timeOff = 0;
	public float 			lightOffPerSecond = 0.2f;
	GameObject 				blackScene, lightObject;
	
	// Use this for initialization
	void Start () {
		blackScene = GameObject.Find ("BlackScene");
		lightObject = GameObject.Find ("Directional Light");
	}
	
	// Update is called once per frame
	void Update () {
		if (turnedOff) {
			if (timeOff < 1)
				timeOff += lightOffPerSecond * Time.deltaTime;
			else{
				turnedOff = false;
				Vector3 temp = blackScene.transform.position;
				temp.z = 1;
				blackScene.transform.position = temp;
				//print (blackScene.transform.position);
				lightObject.GetComponent<Light> ().enabled = true;
				timeOff = 0;
			}
		}
	}
	
	void OnTriggerStay(Collider other) {
		//print (other.gameObject.tag);
		if (other.gameObject.tag == "Murderer") {
			if (Input.GetKeyDown (KeyCode.P) && !beenClicked){
				beenClicked = true;
				//print ("Clicked");
				Vector3 temp = blackScene.transform.position;
				temp.z = -1;
				blackScene.transform.position = temp;
				//print (blackScene.transform.position);
				lightObject.GetComponent<Light> ().enabled = false;
				turnedOff = true;
			}
		}
	}
	
	void OnTriggerExit(Collider other) {
		beenClicked = false;
		
	}
}
