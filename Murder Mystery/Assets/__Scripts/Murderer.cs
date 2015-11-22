using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Murderer : Human {
    private 	KeyCode         	murderKey;
    public 		RaycastHit       	hitInfo;
    public      GameObject          knifeObjRef;
    public		int					bloodDropTimeMax;
	public		int 				bloodDropInterval;
    int								bloodDropTimer;

	public int newInterval;

    GameObject currentKnifeObj;
    List<GameObject> bloodTrail;
    bool		tracked;

    public void setMurderKey(KeyCode key)
    {
        murderKey = key;
    }

    void Awake () {
        // Default murder key
        setMurderKey(KeyCode.Space);
		newInterval = 7;
	}
	
	void Start() {
		bloodTrail = new List<GameObject>();
		bloodDropTimer = 0;
		tracked = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Time.time % newInterval == 0f){
			GameObject blood = Instantiate(trackerPrefab, transform.position, Quaternion.identity) as GameObject;
		}

        if (Input.GetKeyDown(murderKey) && !currentKnifeObj)
        {
            /* NOTE: I USED THIS METHOD INSTEAD OF CREATING A KNIFE AS A CHILD
            OBJECT BECAUSE IT'S ONCOLLISION FUNCTION WILL NOT FIRE IF IT'S PARENT'S
            LAYER IS NOT SUPPOSED TO COLLIDE*/
            Vector3 possessionObjPos = transform.position;
            Vector3 knifeOffset;
            if (facingRight)
            {
				knifeOffset = Vector3.right/1.7f;
            }
            else
            {
                knifeOffset = Vector3.left/1.7f;
            }
            possessionObjPos += knifeOffset;
            currentKnifeObj = Instantiate(knifeObjRef, possessionObjPos, transform.rotation) as GameObject;
            
            // Get the knife object
            Knife currentKnife = currentKnifeObj.GetComponent<Knife>();
            currentKnife.murdererOwner = this;
            // Make sure to set its offset!!!
            currentKnife.offset = knifeOffset;
        }
		if (tracked) {
			if (bloodDropTimer % bloodDropInterval == 0) {
				// Drop the blood
				GameObject blood = Instantiate(trackerPrefab, transform.position, Quaternion.identity) as GameObject;
				bloodTrail.Add (blood);
			}
			
			bloodDropTimer++;
			
			if (bloodDropTimer == bloodDropTimeMax) {
				tracked = false;
				bloodDropTimer = 0;
			}
		}
		// If you want to Raycast in the future, this is how you do it:
		//Physics.Raycast(transform.position, Vector3.left, out hitInfo, 1f, GetLayerMask(new string[] { "Human" })))
	}

	public void track(){
		// If the murderer is already being tracked, just reset the tracked timer
		if (tracked) {
			bloodDropTimer = 0;
		}
		tracked = true;
	}
}
