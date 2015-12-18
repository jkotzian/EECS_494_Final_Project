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

	public GameObject healthBarPrefab;
	GameObject healthBar;
	public int currentLife;
	int ghostLives;
	Vector3 reserve;
	float timer;

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

		/*healthBar = Instantiate (healthBarPrefab) as GameObject;
		healthBar.transform.parent = gameObject.transform;
		healthBar.transform.position = gameObject.transform.position;
		healthBar.transform.position = new Vector3 (transform.position.x, transform.position.y + .8f, transform.position.z - 1f);
		healthBar.GetComponent<MeshRenderer>().enabled = false;
		reserve = healthBar.transform.localScale;*/
		ghostLives = 3;
		currentLife = 3;
    }

    // Update is called once per frame
    void Update()
    {      
        if (TotalGame.S.inMainMenu || TotalGame.S.inReady)
        {
            return;
        } 
        /*  
		if (currentLife != ghostLives) {
			healthBar.GetComponent<MeshRenderer>().enabled = true;
			timer = Time.time;
			losingLife = true;
			Vector3 shrink = healthBar.transform.localScale;
			shrink.x = reserve.x*.33f*currentLife;
			healthBar.transform.localScale = shrink;
			ghostLives = currentLife;
			if(currentLife == 2)
				healthBar.GetComponent<Renderer>().material.color = Color.yellow;
			if(currentLife == 1)
				healthBar.GetComponent<Renderer>().material.color = Color.red;
		}

		if((Time.time - timer) > 1f){
			healthBar.GetComponent<MeshRenderer>().enabled = false;
		}*/

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
        bool possessActivated = (Input.GetKeyDown(actionKey) ||
                                (movement.conNum < GamePlay.S.numControllers && 
                                InputManager.Devices[movement.conNum].Action1.WasPressed)) && 
                                !currentPossessionObj && !possessing;
        if (possessActivated)
        {   
            Vector3 possessionObjPos = transform.position;
            currentPossessionObj = Instantiate(possessionObjRef, possessionObjPos, transform.rotation) as GameObject;
            // Get the possession object
            PossessHit possess = currentPossessionObj.GetComponent<PossessHit>();
            possess.ghostOwner = this;

            tryingToPossess = true;
            srend.color = Color.red;
            transform.localScale *= growthVal;
        }
        bool possessDeactivated = (Input.GetKeyUp(actionKey) || (movement.conNum < GamePlay.S.numControllers && 
                                   InputManager.Devices[movement.conNum].Action1.WasReleased)) &&
                                   currentPossessionObj;
		if (possessDeactivated)
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
			//GameObject blood = Instantiate(trackerPrefab, transform.position, Quaternion.identity) as GameObject;
            startTime = Time.time;
		}
        float divider = 2.5f;
        float multiplier = 7f;
        transform.Translate((Vector3.up * Time.deltaTime * Mathf.Cos(Time.time * multiplier))/divider);
    }        

    public IEnumerator enableGameObjectWithDelay()
    {
        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(true);
    }
}

