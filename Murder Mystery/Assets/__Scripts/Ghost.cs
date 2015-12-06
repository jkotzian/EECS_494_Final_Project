using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class Ghost : Human
{                               
    public RaycastHit hitInfo;
    public int bloodDropTimeMax;
    public int bloodDropInterval;
    int bloodDropTimer;

    public int newInterval;

    List<GameObject> bloodTrail;

    public GameObject possessionObjRef;
    GameObject currentPossessionObj;

    bool tracked;
    float startTime;

    public bool possessing;
    public Movement movement;
    public SpriteRenderer srend;

    bool growing;
    bool shrinking;
    public bool tryingToPossess;
    public float growthRate;
    public float maxGrowthY;
    public float normalY;

    public float growthVal;

    public float shrinkingIntoBodySpeed;
    public bool shrinkingIntoBody;
    Vector3 targetPossessionPosition;

    public NPC possessedNPC;

    void Awake()
    {
        setActionKey(KeyCode.Q);   
        startTime = Time.time;
        normalY = transform.position.y;

        possessing = false;
        currentPossessionObj = null;

        movement = GetComponent<Movement>();
        facingRight = true;
        srend = GetComponent<SpriteRenderer>();
        tryingToPossess = false;
        growing = false;
        shrinking = false;
        shrinkingIntoBody = false;
    }

    // Update is called once per frame
    void Update()
    {                    
        if (shrinkingIntoBody)
        {
            float min = .1f;
            Vector3 dir = (targetPossessionPosition - transform.position);
            transform.Translate(dir * shrinkingIntoBodySpeed * Time.deltaTime);

            if ((targetPossessionPosition - transform.position).magnitude < min)
            {
                // Re-enable the Ghost's movement
                movement.enabled = true;
                // Turn on the NPC's movement
                possessedNPC.turnOnMovement();
                // Turn off the Ghost object
                gameObject.SetActive(false);
                shrinkingIntoBody = false;
                // Reset the Ghost
                transform.localScale /= growthVal;
                srend.color = Color.white;
            }
        }
        // COULDNT GET THIS CRAP WORKING
        /*if (growing)
        {
            transform.localScale += Vector3.one * growthRate;
            if (transform.localScale.y > maxGrowthY)
            {
                growing = false;
            }
        }
        else if (shrinking)
        {
            transform.localScale -= Vector3.one * growthRate;
            if (transform.localScale.y < normalY)
            {
                shrinking = false;
            }
        }*/
        if ((Input.GetKeyDown(actionKey) ||
            (GamePlay.S.usingControllers && InputManager.Devices[movement.conNum].Action1.WasPressed)) && 
            !currentPossessionObj && !possessing)
        {
            /* NOTE: I USED THIS METHOD INSTEAD OF CREATING A KNIFE AS A CHILD
            OBJECT BECAUSE IT'S ONCOLLISION FUNCTION WILL NOT FIRE IF IT'S PARENT'S
            LAYER IS NOT SUPPOSED TO COLLIDE*/
            Vector3 possessionObjPos = transform.position;
            // Not having an offset for now, might want one laters
            /*Vector3 possessionObjOffset;
            if (facingRight)
            {
                possessionObjOffset = Vector3.right / 1.7f;
            }
            else
            {
                possessionObjOffset = Vector3.left / 1.7f;
            }
            possessionObjPos += possessionObjOffset;
            // Make sure to set its offset!!!
            possess.offset = possessionObjOffset;*/
            currentPossessionObj = Instantiate(possessionObjRef, possessionObjPos, transform.rotation) as GameObject;
            // Get the possession object
            PossessHit possess = currentPossessionObj.GetComponent<PossessHit>();
            possess.ghostOwner = this;

            tryingToPossess = true;
            srend.color = Color.red;
            transform.localScale *= growthVal;
        }
		if ((Input.GetKeyUp(actionKey) || (GamePlay.S.usingControllers && InputManager.Devices[movement.conNum].Action1.WasReleased)) && 
            currentPossessionObj)
        {
            Destroy(currentPossessionObj);
            currentPossessionObj = null;
            srend.color = Color.white;
            tryingToPossess = false;
            transform.localScale /= growthVal;
        }
    }

    public void ShrinkIntoBody(Vector3 targetPos)
    {
        shrinkingIntoBody = true;
        targetPossessionPosition = targetPos;
        // Turn off the movement to play the animation
        movement.enabled = false;
    }

    void FixedUpdate() { 
		if (Time.time - startTime > newInterval){
			GameObject blood = Instantiate(trackerPrefab, transform.position, Quaternion.identity) as GameObject;
            startTime = Time.time;
		}
        float divider = 2.5f;
        float multiplier = 7f;
        transform.Translate((Vector3.up * Time.deltaTime * Mathf.Cos(Time.time * multiplier))/divider);
    }

    void OnTriggerExit(Collider collider)
    {
        Switch s = collider.GetComponent<Switch>();
        if (s)
        {
            GamePlay.S.texts[4].text = "";
            GamePlay.S.texts[5].text = "";
        }
    }
    
    public void possess(NPC target)
    {
        target.possess(this);
        possessing = true;
    }
}

