using UnityEngine;
using System.Collections;

public class Chandelier : MonoBehaviour {

	bool		hasFallen;

	// Use this for initialization
	void Start () {
		hasFallen = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.GetComponent<Human> () != null && !hasFallen) {
			//GetComponent<BoxCollider> ().enabled = false;
			collision.gameObject.GetComponent<Human> ().Kill ();
			hasFallen = true;
		}
	}
}
