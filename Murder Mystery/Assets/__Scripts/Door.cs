using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

<<<<<<< HEAD
    public Door above;
    public Door below;

	void OnTriggerStay(Collider other){
        Movement movement = other.GetComponent<Movement>();
		//sending the characters up and down a level 
		if(movement && movement.isDetective){
            // move up
			if (Input.GetKeyDown(movement.upKey) && above) {
				print ("up elevator");
                Vector3 dest = new Vector3(above.transform.position.x, above.transform.position.y, -0.2f);
                other.transform.position = dest;
            }
            // move down
			if (Input.GetKeyDown(movement.downKey) && below) {
				print ("down elevator");
                Vector3 dest = new Vector3(below.transform.position.x, below.transform.position.y, -0.2f);
                other.transform.position = dest;
=======
	int pickKeyCode; 
	static public int howManyTimes = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other){
		Movement movement = other.GetComponent<Movement> ();
		                                              
		//sending the characters up and down a level 
		if(other.GetComponent<Movement>() != null){
			if (movement.isDetective == true && Input.GetKeyDown(movement.upKey) && gameObject.name != "TopDoor") {
				//print ("Triggered");
				Vector3 temp = other.transform.position;
				temp.y += 2.5f;
				other.transform.position = temp;
			}

			if (movement.isDetective == true && Input.GetKeyDown(movement.downKey) && gameObject.name != "BottomDoor") {
				//print ("Triggered");
				Vector3 temp = other.transform.position;
				temp.y -= 2.5f;
				other.transform.position = temp;
			}

			if (movement.isMurderer == true && Input.GetKeyDown(movement.upKey) && gameObject.name != "TopDoor") {
				//print ("Triggered");
				howManyTimes++;
				print (howManyTimes);
				Vector3 temp = other.transform.position;
				temp.y += 1.25f;
				other.transform.position = temp;
			}
			
			if (movement.isMurderer == true && Input.GetKeyDown(movement.downKey) && gameObject.name != "BottomDoor") {
				//print ("Triggered");
				Vector3 temp = other.transform.position;
				temp.y -= 1.25f;
				other.transform.position = temp;
>>>>>>> Jeff's-Branch
			}

			//Elevator Kill is glitchy (fix this)
//			if (other.GetComponent<Movement> ().isMurderer == true && Input.GetKeyDown (KeyCode.E)){
//				pickKeyCode = Random.Range (1, 10);
//				print (pickKeyCode);
//				if (pickKeyCode >= 1 && pickKeyCode <= 5 && gameObject.name != "TopDoor"){
//					print ("Going up 1");
//					Vector3 temp = other.transform.position;
//					temp.y += 1.25f;
//					other.transform.position = temp;
//				}
//				if (pickKeyCode >= 6 && pickKeyCode <= 10 && gameObject.name != "BottomDoor"){
//					print ("Going down 1");
//					Vector3 temp = other.transform.position;
//					temp.y -= 1.25f;
//					other.transform.position = temp;
//				}
//				other.GetComponent<Murderer>().Kill();
//			}
		}
	}
}
