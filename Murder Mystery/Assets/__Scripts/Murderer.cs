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
        Human collidedHuman = collisionInfo.collider.gameObject.GetComponent<Human>();
        print(collidedHuman);

        if (Input.GetKeyDown(murderKey)) 
        {
            //Human collidedHuman = collisionInfo.collider.gameObject.GetComponent<Human>();
            //print(collidedHuman);
            // If the murderer collided with a human and they're alive
            if (collidedHuman && collidedHuman.alive)
            {
                collidedHuman.Kill();
                print("Killed");
            }
        }
    }
}
