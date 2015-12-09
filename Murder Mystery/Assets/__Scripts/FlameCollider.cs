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
        NPC NPCToKill = other.gameObject.GetComponent<NPC>();
		if (NPCToKill!= null && GetComponentInParent<FlameTrap>().flameOn)
			NPCToKill.Kill ();
	}
}
