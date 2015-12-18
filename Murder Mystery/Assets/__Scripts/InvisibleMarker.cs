using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InvisibleMarker : MonoBehaviour {

	public int index;
	public bool shown = false, firstTextShown = false;
	public GameObject doneTextGhost1;
	public GameObject doneTextGhost2;
	public GameObject doneTextDetective1;
	public GameObject doneTextDetective2;
	Text finalText, finalText1, finalText_D, finalText_D1;

	// Use this for initialization
	void Start () {
		finalText = GameObject.Find ("Kill").GetComponent<Text> ();
		finalText1 = GameObject.Find ("Kill_2").GetComponent<Text> ();
		finalText_D = GameObject.Find ("Blaster_D1").GetComponent<Text> ();
		finalText_D1 = GameObject.Find ("Blaster (4)").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (doneTextGhost1.GetComponent<Text> ().isActiveAndEnabled) {
			finalText.enabled = false;
		}
		if (doneTextGhost2.GetComponent<Text> ().isActiveAndEnabled) {
			finalText1.enabled = false;
		}
		if (doneTextDetective1.GetComponent<Text> ().isActiveAndEnabled) {
			finalText_D.enabled = false;
		}
		if (doneTextDetective2.GetComponent<Text> ().isActiveAndEnabled) {
			finalText_D1.enabled = false;
		}
	}

	void OnTriggerEnter(Collider other){
		Text newText = null, oldText = null;
		if (other.GetComponent<NPC> () != null) {
			if (!other.GetComponent<NPC> ().possessed)
				return;
		}
		if (!shown) {
			//print (index);
			if (index == 1 || index == 2 || index == 3 || index == 4) {
				//print (index);
				newText = GameObject.Find ("Movement2").GetComponent<Text> ();
				oldText = GameObject.Find ("Movement").GetComponent<Text> ();
				GameObject.Find ("InvisibleMarker1").GetComponent<InvisibleMarker>().shown = true;
				GameObject.Find ("InvisibleMarker2").GetComponent<InvisibleMarker>().shown = true;
				GameObject.Find ("InvisibleMarker3").GetComponent<InvisibleMarker>().shown = true;
				GameObject.Find ("InvisibleMarker4").GetComponent<InvisibleMarker>().shown = true;
			} else if (index == 5) {
				newText = GameObject.Find ("Possess").GetComponent<Text> ();
				oldText = GameObject.Find ("Movement2").GetComponent<Text> ();
			} else if (index == 6) {
				newText = GameObject.Find ("Possess2").GetComponent<Text> ();
				newText.enabled = true;
				newText = GameObject.Find ("Possess3").GetComponent<Text> ();
				oldText = GameObject.Find ("Possess").GetComponent<Text> ();
			} else if (index == 7) {
				newText = GameObject.Find ("Elevator").GetComponent<Text> ();
				oldText = GameObject.Find ("Possess2").GetComponent<Text> ();
				oldText.enabled = false;
				oldText = GameObject.Find ("Possess3").GetComponent<Text> ();
			}
			else if (index == 8) {
				newText = GameObject.Find ("Kill").GetComponent<Text> ();
				oldText = GameObject.Find ("Elevator").GetComponent<Text> ();
			}
			else if (index == 9 || index == 10 || index == 11 || index == 12) {
				//print (index);
				//print (other.transform);
				newText = GameObject.Find ("Movement2_2").GetComponent<Text> ();
				oldText = GameObject.Find ("Movement_2").GetComponent<Text> ();
				GameObject.Find ("InvisibleMarker9").GetComponent<InvisibleMarker>().shown = true;
				GameObject.Find ("InvisibleMarker10").GetComponent<InvisibleMarker>().shown = true;
				GameObject.Find ("InvisibleMarker11").GetComponent<InvisibleMarker>().shown = true;
				GameObject.Find ("InvisibleMarker12").GetComponent<InvisibleMarker>().shown = true;
			} else if (index == 13) {
				newText = GameObject.Find ("Possess_2").GetComponent<Text> ();
				oldText = GameObject.Find ("Movement2_2").GetComponent<Text> ();
			} else if (index == 14) {
				newText = GameObject.Find ("Possess2_2").GetComponent<Text> ();
				newText.enabled = true;
				newText = GameObject.Find ("Possess3_2").GetComponent<Text> ();
				oldText = GameObject.Find ("Possess_2").GetComponent<Text> ();
			} else if (index == 15) {
				newText = GameObject.Find ("Elevator_2").GetComponent<Text> ();
				oldText = GameObject.Find ("Possess2_2").GetComponent<Text> ();
				oldText.enabled = false;
				oldText = GameObject.Find ("Possess3_2").GetComponent<Text> ();
			}
			else if (index == 16) {
				newText = GameObject.Find ("Kill_2").GetComponent<Text> ();
				oldText = GameObject.Find ("Elevator_2").GetComponent<Text> ();
			}
			else if (index == 18 || index == 21) {
				newText = GameObject.Find ("Blaster_D_2").GetComponent<Text> ();
				oldText = GameObject.Find ("Movement_D").GetComponent<Text> ();
				GameObject.Find ("InvisibleMarker18").GetComponent<InvisibleMarker>().shown = true;
				GameObject.Find ("InvisibleMarker21").GetComponent<InvisibleMarker>().shown = true;
			}
			else if (index == 22){
				if (other.name == "Collider")
					return;
				newText = GameObject.Find ("Blaster_D1").GetComponent<Text> ();
				oldText = GameObject.Find ("Blaster_D").GetComponent<Text> ();
			}
			else if (index == 29){
				if (other.name == "Collider")
					return;
				newText = GameObject.Find ("Blaster_D").GetComponent<Text> ();
				oldText = GameObject.Find ("Blaster_D_2").GetComponent<Text> ();
			}
			else if (index == 24 || index == 27) {
				newText = GameObject.Find ("Blaster (5)").GetComponent<Text> ();
				oldText = GameObject.Find ("Movement_D_2").GetComponent<Text> ();
			}
			else if (index == 28){
				if (other.name == "Collider")
					return;
				newText = GameObject.Find ("Blaster (4)").GetComponent<Text> ();
				oldText = GameObject.Find ("Blaster_D1_2").GetComponent<Text> ();
			}
			else if (index == 30){
				if (other.name == "Collider")
					return;
				newText = GameObject.Find ("Blaster_D1_2").GetComponent<Text> ();
				oldText = GameObject.Find ("Blaster (5)").GetComponent<Text> ();
			}
			newText.enabled = true;
			oldText.enabled = false;
			shown = true;
		}
	}
}
