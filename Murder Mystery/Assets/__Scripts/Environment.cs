using UnityEngine;
using System.Collections;

public class Environment : MonoBehaviour {

	bool					turnedOff;
    // The smallest and largest possible number of seconds until the lights go out
    public int              turnOffTimeMin;
    public int              turnOffTimeMax;
    // The smallest and largest possible number of seconds until the lights go
    // back on
    public int              turnOnTimeMin;
    public int              turnOnTimeMax;
    int                     lightTimer;
    // The amount of time the lights will go off next
    int                     nextTurnOffTime;
    // The amount of time when the lights will
    // go on next
    int                     nextTurnOnTime;
    public Transform        cameraToHideObj;
    HideLight        cameraToHide;
    public Transform        nightVisionObject;
	
	// Use this for initialization
	void Start () {
        nextTurnOffTime = Random.Range(turnOffTimeMin, turnOffTimeMax + 1);
        lightTimer = 0;
        turnedOff = false;
        nightVisionObject.gameObject.SetActive(false);
        cameraToHide = cameraToHideObj.GetComponent<HideLight>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        lightTimer++;
        //Debug.Log(lightTimer);
        if (!turnedOff && lightTimer == nextTurnOffTime)
        {
            turnOff();
        }
		if (turnedOff && lightTimer == nextTurnOnTime) 
        {
            turnOn();
		}
	}

    void turnOff()
    {
        // Say that the lights are off and bring the black screen
        // to the forefront
        turnedOff = true;
        cameraToHide.hideLight();
        nightVisionObject.gameObject.SetActive(true);
        // Reset the timer and figure out when to turn the lights
        // back on
        lightTimer = 0;
        nextTurnOnTime = Random.Range(turnOnTimeMin, turnOnTimeMax + 1);
        //Debug.Log(nextTurnOnTime);
    }

    void turnOn()
    {
        // Say that the lights are on and put the black screen
        // in the background
        turnedOff = false;
        cameraToHide.unhideLight();
        nightVisionObject.gameObject.SetActive(false);
        // Reset the timer and figure out when to turn the lights
        // back off
        lightTimer = 0;
        nextTurnOffTime = Random.Range(turnOffTimeMin, turnOffTimeMax + 1);
        //Debug.Log(nextTurnOffTime);
    }

    //void OnTriggerStay(Collider other) {
    //    //print (other.gameObject.tag);
    //    if (other.gameObject.tag == "Murderer") {
    //        if (Input.GetKeyDown (KeyCode.P) && !beenClicked) {
    //            beenClicked = true;
    //            //print ("Clicked");
    //            Vector3 temp = blackScene.transform.position;
    //            temp.z = -1;
    //            blackScene.transform.position = temp;
    //            //print (blackScene.transform.position);
    //            lightObject.GetComponent<Light> ().enabled = false;
    //            turnedOff = true;
    //        }
    //    }
    //}
	
    //void OnTriggerExit(Collider other) {
    //    beenClicked = false;
    //}
}
