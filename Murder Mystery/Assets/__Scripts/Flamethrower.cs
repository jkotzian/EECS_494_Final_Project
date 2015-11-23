using UnityEngine;
using System.Collections;

public class Flamethrower : MonoBehaviour {
	
	float 			heat = 0;
	float 			heatPerSecond = 0.2f;
	public bool		flameOn = false;

	// Use this for initialization
	void Start () {
		print (this.GetComponentInChildren<ParticleSystem> ());
		this.GetComponentInChildren<ParticleSystem> ().emissionRate = 0.0f;
		this.GetComponentInChildren<BoxCollider> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (flameOn) {
			if (heat < 1) {
				this.GetComponentInChildren<ParticleSystem> ().emissionRate = 150f;
				this.GetComponentInChildren<BoxCollider> ().enabled = true;
				heat += heatPerSecond * Time.deltaTime;
			}
			else{
				heat = 0;
				flameOn = false;
				this.GetComponentInChildren<ParticleSystem> ().emissionRate = 0;
				this.GetComponentInChildren<BoxCollider> ().enabled = false;
			}
		}
	}
}
