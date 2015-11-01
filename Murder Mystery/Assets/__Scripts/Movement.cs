using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	public float speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!ClimbScript.S.canMove) {
			gameObject.GetComponent<Rigidbody>().useGravity = true;
		}

		if (Input.GetKey (KeyCode.RightArrow)) {
			transform.Translate(Vector3.right*Time.deltaTime*speed);
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.Translate(Vector3.left*Time.deltaTime*speed);
		}

		if (ClimbScript.S.canMove) {
			gameObject.GetComponent<Rigidbody>().useGravity = false;

			if (Input.GetKey (KeyCode.UpArrow)) {
				transform.Translate (Vector3.up * Time.deltaTime * speed);
			}
			if (Input.GetKey (KeyCode.DownArrow)) {
				transform.Translate (Vector3.down * Time.deltaTime * speed);
			}
		}


	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.name == "Stairs") {
			print("Stairs");

			if (Input.GetKey (KeyCode.UpArrow)) {
				transform.Translate(Vector3.up*Time.deltaTime*speed);
			}
			if (Input.GetKey (KeyCode.DownArrow)) {
				transform.Translate(Vector3.down*Time.deltaTime*speed);
			}
		}

	}
}
