using UnityEngine;
using System.Collections;

public class Detective : Human {
    public GameObject detective;


	// Use this for initialization
	void Start () {
	
	}

    void FixedUpdate()
    {

    }

    void OnCollisionStay(Collision collisionInfo)
    {
        //Make Arrest
        if (Input.GetKey(KeyCode.RightShift))
        {
            // See if the object is a murderer
            Murderer murderer = collisionInfo.collider.gameObject.GetComponent<Murderer>();

            if (murderer && murderer.alive)
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
    }
}
