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
        float diameter = (interval - (Time.time - timeTrack)) * 0.01f;
        gameObject.transform.localScale = new Vector3(diameter, diameter, 1);
		//for only a given interval, the tracks can be found
		if (Time.time - timeTrack > interval) {
			//print ("TimeTrack: " + timeTrack);
			//print ("Time: " + Time.time);
			isTrackable = false;
			gameObject.SetActive (false);
		}
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        foreach (GameObject detective in GamePlay.S.Detectives)
        {
            if(Mathf.Abs(detective.transform.position.x - transform.position.x) < 3 && Mathf.Abs(detective.transform.position.y - transform.position.y) < 1)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = true;
            }    
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
