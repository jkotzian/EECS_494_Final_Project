using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class Detective : Human {
    private Movement        movement;

    public GameObject ghostHitObjRef;
    GameObject currentGhostHitObj;
    public Light aura;
    public Transform weaponFireSoundObj;
    AudioSource weaponFireSound;
    public Transform weaponEffect;
    int weaponFireTimer;
    public int weaponFireTimeMax;
    bool canFire;
    
	void Start () {
        // Set default arrest key
        movement = transform.GetComponent<Movement>();
        facingRight = true;
        weaponFireSound = weaponFireSoundObj.GetComponent<AudioSource>();
        weaponFireTimer = 0;
        canFire = true;
    }

    void FixedUpdate()
    {
        if (weaponFireTimer > 0)
        {
            --weaponFireTimer;
            if (weaponFireTimer == 0)
            {
                weaponEffect.gameObject.SetActive(false);
                canFire = true;
            }
        }
    }
    void Update()
    {
		if (canFire && (Input.GetKeyDown(actionKey) || (GamePlay.S.usingControllers && InputManager.Devices[movement.conNum].Action1.WasPressed)) && 
            !currentGhostHitObj)
        {
            weaponFireSound.Play();
            weaponEffect.gameObject.SetActive(true);
            weaponFireTimer = weaponFireTimeMax;
            canFire = false;

            Vector3 ghostHitObjPos = transform.position;
            // Not having an offset for now, might want one laters
            Vector3 ghostHitObjOffset;
            if (facingRight)
            {
                ghostHitObjOffset = Vector3.right;
            }
            else
            {
                ghostHitObjOffset = Vector3.left;
            }
            //ghostHitObjOffset += Vector3.up;

            ghostHitObjPos += ghostHitObjOffset;

            currentGhostHitObj = Instantiate(ghostHitObjRef, ghostHitObjPos, transform.rotation) as GameObject;

            // Get the possession object
            GhostHit ghostHit = currentGhostHitObj.GetComponent<GhostHit>();
            ghostHit.detectiveOwner = this;
            // Make sure to set its offset!!!
            ghostHit.offset = ghostHitObjOffset;
        }
		if ((Input.GetKeyUp(actionKey) || (GamePlay.S.usingControllers && InputManager.Devices[movement.conNum].Action1.WasReleased)) && 
            currentGhostHitObj)
        {
            Destroy(currentGhostHitObj);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        movement.checkForDoorInput(collider);
    }
}
