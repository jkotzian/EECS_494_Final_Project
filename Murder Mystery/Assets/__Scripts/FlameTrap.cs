using UnityEngine;
using System.Collections;

public class FlameTrap : Trap {
	
	float 			heat = 0;
	float 			heatPerSecond = 0.2f;
	public bool		flameOn = false;

	// Use this for initialization
	void Start () {
		print (this.GetComponentInChildren<ParticleSystem> ());
		this.GetComponentInChildren<ParticleSystem> ().emissionRate = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void activate()
    {
        if (heat < 1)
        {
            this.GetComponentInChildren<ParticleSystem>().emissionRate = 150f;
            heat += heatPerSecond * Time.deltaTime;
        }
        else
        {
            heat = 0;
            flameOn = false;
            this.GetComponentInChildren<ParticleSystem>().emissionRate = 0;
        }
    }
}
