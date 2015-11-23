using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ghost : Human
{
    public KeyCode possessKey;
    public RaycastHit hitInfo;
    public int bloodDropTimeMax;
    public int bloodDropInterval;
    int bloodDropTimer;

    public int newInterval;

    List<GameObject> bloodTrail;

    public GameObject possessionObjRef;
    GameObject currentPossessionObj;

    bool tracked;

    public bool possessing;

    public void setPossessKey(KeyCode key)
    {
        possessKey = key;
    }

    void Awake()
    {
        // Default murder key
        setPossessKey(KeyCode.Space);
		newInterval = 7;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(possessKey) && !currentPossessionObj)
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
        }
        if (Input.GetKeyUp(possessKey) && currentPossessionObj)
        {
            Destroy(currentPossessionObj);
        }
    }

    void FixedUpdate() { 
		if(Time.time % newInterval == 0f){
			GameObject blood = Instantiate(trackerPrefab, transform.position, Quaternion.identity) as GameObject;
		}
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
}

