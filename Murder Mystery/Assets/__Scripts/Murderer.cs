using UnityEngine;
using System.Collections;

public class Murderer : Human {

    public KeyCode         murderKey;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnCollisionStay(Collision collisionInfo)
    {
        print("a");
        if (Input.GetKeyDown(murderKey)) 
        {
            print("b");
            Human collidedHuman = collisionInfo.collider.gameObject.GetComponent<Human>();
            // If the murderer collided with a human and they're alive
            if (collidedHuman && collidedHuman.alive)
            {
                Kill();
                print("Killed");
            }
        }
    }
}
