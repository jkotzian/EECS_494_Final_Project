using UnityEngine;
using System.Collections;

public class Human : MonoBehaviour {
    public bool alive;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Kill()
    {
        // Daniel, put your rotating sprite on death code here
        alive = false;
    }
}
