using UnityEngine;
using System.Collections;

public class Murderer : Human {
    private KeyCode         murderKey;
    public RaycastHit       hitInfo;

    public void setMurderKey(KeyCode key)
    {
        murderKey = key;
    }

    void Awake () {
        // Default murder key
        setMurderKey(KeyCode.Space);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKeyDown(murderKey))
        {
            //Vector3 directionVec = Vector3.zero;

            //Debug.DrawLine(transform.position, transform.position + Vector3.right, Color.black, 1.0f);
            // Look left and right for a target
            if (Physics.Raycast(transform.position, Vector3.right, out hitInfo, 1f, GetLayerMask(new string[] { "Human" })) ||
                Physics.Raycast(transform.position, Vector3.left, out hitInfo, 1f, GetLayerMask(new string[] { "Human" })))
            {
                Human collidedHuman = hitInfo.collider.gameObject.GetComponent<Human>();
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

    //void OnCollisionStay(Collision collisionInfo)
    //{
    //    Human collidedHuman = collisionInfo.collider.gameObject.GetComponent<Human>();
    //    print(collidedHuman);


    //}
}
