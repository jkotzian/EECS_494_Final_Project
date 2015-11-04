using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Murderer : Human {
    private 	KeyCode         	murderKey;
	private 	bool				isStabbed = false;
	private 	float				delayKill = 0, delayPerSecond = 0.2f; 
	private 	Human				collidedHuman;
	private 	List<GameObject>	bloodTrail = new List<GameObject>();
	public	 	GameObject			trackerPrefab;
    public 		RaycastHit       	hitInfo;

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
			//print ("Pressed");
            //Debug.DrawLine(transform.position, transform.position + Vector3.right, Color.black, 1.0f);
            // Look left and right for a target
            if (Physics.Raycast(transform.position, Vector3.right, out hitInfo, 1f, GetLayerMask(new string[] { "Human" })) ||
                Physics.Raycast(transform.position, Vector3.left, out hitInfo, 1f, GetLayerMask(new string[] { "Human" })))
            {
                collidedHuman = hitInfo.collider.gameObject.GetComponent<Human>();
                //Human collidedHuman = collisionInfo.collider.gameObject.GetComponent<Human>();
                //print(collidedHuman);
                // If the murderer collided with a human and they're alive
                if (collidedHuman && !isStabbed)
                {
					isStabbed = true;
					//print ("Initial: " + transform.position);
					GameObject blood = Instantiate(trackerPrefab, transform.position, Quaternion.identity) as GameObject;
					bloodTrail.Add (blood);
					InvokeRepeating ("tracking", 1f, 1f);
                }
            }
        }
		if (isStabbed) {
            if (delayKill < 1){
                delayKill += delayPerSecond * Time.deltaTime;
			}
            else{
                collidedHuman.Kill();
                delayKill = 0;
                print("Killed");
                isStabbed = false;
				CancelInvoke();
            }
        }
	}

	void tracking(){
		GameObject blood = Instantiate(trackerPrefab, transform.position, Quaternion.identity) as GameObject;
		bloodTrail.Add (blood);
		//print (transform.position);
	}

    //void OnCollisionStay(Collision collisionInfo)
    //{
    //    Human collidedHuman = collisionInfo.collider.gameObject.GetComponent<Human>();
    //    print(collidedHuman);


    //}
}
