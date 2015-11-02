using UnityEngine;
using System.Collections;

public class Detective : MonoBehaviour {
    public GameObject detective;


	// Use this for initialization
	void Start () {
	
	}

    void FixedUpdate()
    {
        //Make Arrest
        if (Input.GetKey(KeyCode.RightShift))
        {
            GameObject murderer = GameObject.Find("Player1");
            Vector3 detectivePos = gameObject.transform.position;
            Vector3 murdererPos = murderer.transform.position;
            if((detectivePos - murdererPos).magnitude < 1)
            {
                //Knock the murderer over
                murderer.transform.Rotate(new Vector3(0, 0, 90));
            }
        }
    }
}
