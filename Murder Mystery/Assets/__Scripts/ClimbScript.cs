using UnityEngine;
using System.Collections;

public class ClimbScript : MonoBehaviour {
	static public ClimbScript S;
	public bool canMove = false;
	// Use this for initialization
	void Start () {
		S = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		print ("Triggered bitches");
		canMove = true;
	}

	void OnTriggerExit(Collider other){
		canMove = false;
	}
}
