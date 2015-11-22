using UnityEngine;
using System.Collections;

public class RevealTracker : MonoBehaviour {
	public float timeTrack;
	public float interval;
	public bool isTrackable;
	
	// Use this for initialization
	void Start () {
		timeTrack = Time.time;
		isTrackable = true;
	}
	
	// Update is called once per frame
	void Update () {
		//for only a given interval, the tracks can be found
		if (Time.time - timeTrack > interval) {
			//print ("TimeTrack: " + timeTrack);
			//print ("Time: " + Time.time);
			isTrackable = false;
			gameObject.SetActive (false);
		}
	
	}

	void OnTriggerEnter(Collider other){
		//tracker will show when a detective gets close enough
		if(other.GetComponent<Movement>() != null){
			if(other.GetComponent<Movement>().inDetectiveMode == true){
				gameObject.GetComponent<MeshRenderer>().enabled = true;
			}
		}

	}
}
