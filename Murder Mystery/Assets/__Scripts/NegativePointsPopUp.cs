using UnityEngine;
using System.Collections;

public class NegativePointsPopUp : MonoBehaviour {

    float t;

	// Use this for initialization
	void Start () {
        t = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
	    if(Time.time > t + 1)
        {
            print("hit");
            Destroy(this.gameObject);
        }
	}
}
