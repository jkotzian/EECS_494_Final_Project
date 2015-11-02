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
        //Knock the murderer over
        transform.Rotate(new Vector3(0, 0, 90));
        alive = false;
    }
}
