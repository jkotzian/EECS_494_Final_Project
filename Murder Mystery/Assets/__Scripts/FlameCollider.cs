using UnityEngine;
using System.Collections;

public class FlameCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other)
	{

		if (other.gameObject.GetComponent<Human>() != null && GetComponentInParent<Flamethrower>().flameOn)
			other.gameObject.GetComponent<Human> ().Kill ();
	}
}
