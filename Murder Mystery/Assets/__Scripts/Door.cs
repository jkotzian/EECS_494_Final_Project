using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other){
		//sending the characters up and down a level 
		if(other.gameObject.name == "Detective(Clone)"){
			if (other.GetComponent<Movement> ().isDetective == true && Input.GetKeyDown(other.GetComponent<Movement> ().upKey) && gameObject.name != "TopDoor") {
				print ("Triggered");
				Vector3 temp = other.transform.position;
				temp.y += 2.5f;
				other.transform.position = temp;
			}

			if (other.GetComponent<Movement> ().isDetective == true && Input.GetKeyDown(other.GetComponent<Movement> ().downKey) && gameObject.name != "BottomDoor") {
				print ("Triggered");
				Vector3 temp = other.transform.position;
				temp.y -= 2.5f;
				other.transform.position = temp;
			}
		}
	}
}
