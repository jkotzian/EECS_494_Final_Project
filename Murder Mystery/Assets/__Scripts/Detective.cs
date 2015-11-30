using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class Detective : Human {
    private KeyCode         arrestKey;
    private Movement        movement;

    public GameObject ghostHitObjRef;
    GameObject currentGhostHitObj;
    public Light aura;

	public int conNum;

    public void setArrestKey(KeyCode key)
    {
        arrestKey = key;
    }
    
	void Start () {
        // Set default arrest key
        setArrestKey(KeyCode.RightShift);
        movement = transform.GetComponent<Movement>();
    }

    void Update()
    {
		if ((Input.GetKeyDown(arrestKey) || InputManager.Devices[conNum].Action1) && !currentGhostHitObj)
        {
            Vector3 ghostHitObjPos = transform.position;
            // Not having an offset for now, might want one laters
            Vector3 ghostHitObjOffset;
            if (facingRight)
            {
                ghostHitObjOffset = Vector3.right / 1.3f;
            }
            else
            {
                ghostHitObjOffset = Vector3.left / 1.3f;
            }
            ghostHitObjOffset += Vector3.up / 2.0f;

            ghostHitObjPos += ghostHitObjOffset;

            currentGhostHitObj = Instantiate(ghostHitObjRef, ghostHitObjPos, transform.rotation) as GameObject;

            // Get the possession object
            GhostHit ghostHit = currentGhostHitObj.GetComponent<GhostHit>();
            ghostHit.detectiveOwner = this;
            // Make sure to set its offset!!!
            ghostHit.offset = ghostHitObjOffset;
        }
		if ((!Input.GetKeyDown(arrestKey) || !InputManager.Devices[conNum].Action1) && currentGhostHitObj)
        {
            Destroy(currentGhostHitObj);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        movement.checkForDoorInput(collider);
    }
}
