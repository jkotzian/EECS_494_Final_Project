using UnityEngine;
using System.Collections;

public class ButtonFlash : MonoBehaviour {
                               
    private Component glow;
    private float t;
    private bool on;

    // Use this for initialization
    void Start () {
        glow = gameObject.GetComponent("Halo");
        t = Time.time;
        on = false;
    }
	
	// Update is called once per frame
	void Update () {
	    if(Time.time > t + .3f)
        {
            flip();
            t = Time.time;    
        }
	}

    void flip()
    {
        if (on)
        {
            glow.GetType().GetProperty("enabled").SetValue(glow, false, null);
            on = false;
        }
        else
        {
            glow.GetType().GetProperty("enabled").SetValue(glow, true, null);
            on = true;
        }
    }
}
