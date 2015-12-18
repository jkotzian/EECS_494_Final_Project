using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class Detective : Human {
    private Movement        movement;

    public GameObject ghostHitObjRef;
    public GameObject currentGhostHitObj;
    public Light aura;
    public Transform weaponFireSoundObj;
    public AudioSource weaponFireSound;
    public Transform weaponEffect;
    int weaponFireTimer;
    public int weaponFireTimeMax;
    public bool canFire;

    void Awake()
    {
        movement = transform.GetComponent<Movement>();
    }

	void Start () {
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
        if (TotalGame.S.inMainMenu || TotalGame.S.inReady)
        {
            return;
        }
		if (canFire && !currentGhostHitObj && 
            (Input.GetKeyDown(actionKey) || (GamePlay.S != null && movement.conNum < GamePlay.S.numControllers && 
            InputManager.Devices[movement.conNum].Action1.WasPressed)))
        {
            weaponFireSound.Play();
            weaponEffect.gameObject.SetActive(true);
            weaponFireTimer = weaponFireTimeMax;
            canFire = false;

            Vector3 ghostHitObjPos = transform.position;
            // Not having an offset for now, might want one laters
            Vector3 ghostHitObjOffset = Vector3.zero;
            Vector3 horizontalOffset = Vector3.zero;
            if (facingRight)
            {
                horizontalOffset = new Vector3(1.2f, 0, 0);
            }
            else
            {
                horizontalOffset = new Vector3(-1.2f, 0, 0);
            }
            Vector3 verticalOffset = new Vector3(0, .2f, 0);
            ghostHitObjOffset += (horizontalOffset + verticalOffset);

            ghostHitObjPos += ghostHitObjOffset;

            currentGhostHitObj = Instantiate(ghostHitObjRef, ghostHitObjPos, transform.rotation) as GameObject;

            // Get the possession object
            GhostHit ghostHit = currentGhostHitObj.GetComponent<GhostHit>();
            ghostHit.detectiveOwner = this;
            // Make sure to set its offset!!!
            ghostHit.offset = ghostHitObjOffset;
        }
        if ((Input.GetKeyUp(actionKey) && currentGhostHitObj || 
            (GamePlay.S != null && movement.conNum < GamePlay.S.numControllers && 
            InputManager.Devices[movement.conNum].Action1.WasReleased)))
        {
            Destroy(currentGhostHitObj);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        movement.checkForDoorInput(collider);
    }
}
