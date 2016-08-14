using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class Ghost : Human
{
    public bool active;

    public GameObject possessionObjRef;
    GameObject currentPossessionObj;

    NPC possessionTarget;
    public NPC possessedNPC;

    public Movement movement;
    public float flyingSpeed = 3;
    public float closeEnoughDistance = .2f;

    public SpriteRenderer srend;

    bool growing;
    public float growthRate;
    public float maxGrowthY;
    public float normalY;
    public float growthVal;
    public float fadingIntoBodySpeed;

    public enum State {Idle, FlyingToPossess, FadingIntoBody, Possessing}
    public State currentState;

    void Awake()
    {
        setActionKey(KeyCode.Q);   
        normalY = transform.position.y;

        currentPossessionObj = null;

        movement = GetComponent<Movement>();
        facingRight = true;
        srend = GetComponent<SpriteRenderer>();
        growing = false;

        currentState = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {      
        if (TotalGame.S.inMainMenu || TotalGame.S.inReady || !active)
        {
            return;
        } 

        bool possessActivated = (Input.GetKeyDown(actionKey) ||
                                (movement.conNum < GamePlay.S.numControllers && 
                                InputManager.Devices[movement.conNum].Action1.WasPressed));

        if (possessActivated && currentState == State.Idle)
        {
            // Set the possession target
            possessionTarget = getClosestPossessionTarget(transform);
            if (possessionTarget)
                currentState = State.FlyingToPossess;
        }

        bool possessDeactivated = (Input.GetKeyUp(actionKey) || (movement.conNum < GamePlay.S.numControllers && 
                                   InputManager.Devices[movement.conNum].Action1.WasReleased));
		if (possessDeactivated)
        {
        
        }
        // If the ghost is flying to possess
        if (currentState == State.FlyingToPossess)
        {
            // Make sure the possession target is still alive
            if (!possessionTarget.alive)
            {
                // If it's not still alive, go back to idle
                possessionTarget = null;
                currentState = State.Idle;
            }
            // Check to see if the ghost has made it to the target
            if (closeEnoughToTarget(possessionTarget))
            {
                currentState = State.FadingIntoBody;
                StartCoroutine(fadeAndPossess());
            }
            else
            {
                flyTowardsTarget(possessionTarget);
            }
        }
    }
    // Fades the ghost sprite until it is transparent
    public IEnumerator fadeAndPossess()
    {
        float fadeAmnt = .1f;
        float fadeSpeed = .05f;
        float fadeValue = 1.0f;
        while (fadeValue > 0)
        {
            srend.color = new Color(1f, 1f, 1f, fadeValue);
            fadeValue -= fadeAmnt;
            yield return new WaitForSeconds(fadeSpeed);
        }
        // Make sure the ghost is completely faded
        srend.color = new Color(1f, 1f, 1f, 0.0f);
        Debug.Log("Hey!");
    }

    // Return whether or not the ghost is close enough to the NPC target
    bool closeEnoughToTarget(NPC target)
    {
        // [TECH DEBT] constantly getting the Transform component
        if ((target.GetComponent<Transform>().position - transform.position).sqrMagnitude <= 
            closeEnoughDistance)
            return true;
        return false;
    }
    // Tells the ghost to fly towards the target's position
    void flyTowardsTarget(NPC target)
    {
        // [TECH DEBT] getting the transform component
        transform.position = Vector3.MoveTowards(transform.position, 
                                                 target.GetComponent<Transform>().position, 
                                                 flyingSpeed * Time.deltaTime);
    }
    // Gets the closest living NPC to the inputed position
    // Returns null if there are no alive NPCs
    NPC getClosestPossessionTarget(Transform ghostTransform)
    {
        // Calculate the distances between the ghost and all party guests
        // [TECH DEBT] it would be more optimized if we went over a list of alive NPCs that
        // was managed by the Gameplay class
        NPC closestNPC = null;
        float closestSqrDistance = Mathf.Infinity;
        foreach (GameObject partyGuest in GamePlay.S.NPCs)
        {
            NPC npc = partyGuest.GetComponent<NPC>();
            // See if the party guest is still alive
            if (npc.alive)
            {
                if (!closestNPC) {
                    closestNPC = npc;
                    closestSqrDistance = (npc.GetComponent<Transform>().position - ghostTransform.position).sqrMagnitude;
                }
                else
                {
                    // Calculate the distance to see if it's the closest
                    // [TECH DEBT] constantly getting the Tranform component here is bad
                    float distanceSqrToTarget = 
                    (npc.GetComponent<Transform>().position - ghostTransform.position).sqrMagnitude;
                    // Check to see if it's now the closest NPC
                    if (distanceSqrToTarget < closestSqrDistance)
                    {
                        closestNPC = npc;
                        closestSqrDistance = distanceSqrToTarget;
                    }
                }
            }
        }
        return closestNPC;
    }

    void FixedUpdate() {
        // Ghost floating
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

