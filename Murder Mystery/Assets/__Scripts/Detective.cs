using UnityEngine;
using System.Collections;

public class Detective : Human {
    public GameObject   detective;
    public Transform murdererTransform;
    Murderer murderer;

	// Use this for initialization
	void Start () {
        murderer = murdererTransform.GetComponent<Murderer>();
	}

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.RightShift) && murderer && murderer.alive)
        {
            Vector3 detectivePos = gameObject.transform.position;
            Vector3 murdererPos = murderer.transform.position;

            if ((detectivePos - murdererPos).magnitude < 1)
            {
                murderer.Kill();
                //Knock the murderer over
                //murderer.transform.Rotate(new Vector3(0, 0, 90));
            }
        }
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        ////Make Arrest
        //if (Input.GetKey(KeyCode.RightShift))
        //{
        //    // See if the object is a murderer
        //    Murderer murderer = collisionInfo.collider.gameObject.GetComponent<Murderer>();


        //}
    }
}
