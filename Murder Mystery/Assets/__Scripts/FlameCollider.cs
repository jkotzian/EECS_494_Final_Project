using UnityEngine;
using System.Collections;

public class FlameCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision)
	{
		print (collision.gameObject.name);
		collision.gameObject.GetComponent<Human> ().Kill ();
	}
}
