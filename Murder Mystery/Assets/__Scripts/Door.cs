using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    public Door above;
    public Door below;

    public void MoveUp(GameObject passenger)
    {
        print("up elevator");
        Vector3 dest = new Vector3(above.transform.position.x, above.transform.position.y, -0.2f);
        passenger.transform.position = dest;
    }

    public void MoveDown(GameObject passenger)
    {
        print("down elevator");
        Vector3 dest = new Vector3(below.transform.position.x, below.transform.position.y, -0.2f);
        passenger.transform.position = dest;
    }

    void OnTriggerStay(Collider other){
        Movement movement = other.GetComponent<Movement>();
		//sending the characters up and down a level 
		if (movement && !movement.isGhost) {
            // move up
			if (Input.GetKeyDown(movement.upKey) && above) {
				
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
