using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

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
			}
		}
	}
}
