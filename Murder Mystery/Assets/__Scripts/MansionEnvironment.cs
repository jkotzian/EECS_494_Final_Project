using UnityEngine;
using System.Collections;

public class MansionEnvironment : MonoBehaviour {
	
	public static MansionEnvironment E;
	
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
    int                     nextFlashTime;
    public Transform        cameraToHideObj;
    HideLight               cameraHideObj;
	public bool lightsOn = true;
	bool first;
	
	void Awake(){
		E = this;
	}
	
	// Use this for initialization
	void Start () {
		nextTurnOffTime = turnOffTimeMax + 1;
		// Set it so the lights start turned off
		lightTimer = nextTurnOffTime - 1;
		turnedOff = false;         
        cameraHideObj = cameraToHideObj.GetComponent<HideLight>();
        first = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		lightTimer++;
		//Debug.Log(lightTimer);
		if (!turnedOff && lightTimer == nextTurnOffTime)
		{
			//turnOff();
		}
		if (turnedOff && lightTimer == nextTurnOnTime) 
		{
			//turnOn();
		}
        if (turnedOff && lightTimer == nextFlashTime)
        {
            //StartCoroutine(cameraHideObj.flashDown());
        }
	}
	
	void turnOff()
	{
		// Say that the lights are off and bring the black screen
		// to the forefront
		turnedOff = true;
		cameraHideObj.hideLight();
        StartCoroutine(delayActivateNightVision());
        // Reset the timer and figure out when to turn the lights
        // back on
        lightTimer = 0;
		if (first)
		{
			first = false;
            nextTurnOnTime = turnOnTimeMax * 2;
		}
		else
        {
            nextTurnOnTime = Random.Range(turnOnTimeMin, turnOnTimeMax + 1);
        }
        nextFlashTime = nextTurnOnTime / 2;
		lightsOn = false;
		//Debug.Log(nextTurnOnTime);
	}
	
	void turnOn()
	{
		// Say that the lights are on and put the black screen
		// in the background
		turnedOff = false;
		cameraHideObj.unhideLight();
        StartCoroutine(delayDeactivateNightVision());
        // Reset the timer and figure out when to turn the lights
        // back off
        lightTimer = 0;
		nextTurnOffTime = Random.Range(turnOffTimeMin, turnOffTimeMax + 1);
		lightsOn = true;
		//Debug.Log(nextTurnOffTime);
	}
    IEnumerator delayActivateNightVision()
    {
        yield return new WaitForSeconds(1f);
    }

    IEnumerator delayDeactivateNightVision()
    {
        yield return new WaitForSeconds(1.1f);
    }
}
