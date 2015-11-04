using UnityEngine;
using System.Collections;

public class Detective : Human {
    private KeyCode         arrestKey;
    private Murderer[]      murderers;

    public void setArrestKey(KeyCode key)
    {
        arrestKey = key;
    }
    
	void Awake () {
        // Set default arrest key
        setArrestKey(KeyCode.RightShift);
        // Link Murderer objects from GamePlay singleton GameObjects
        murderers = new Murderer[2];
        murderers[0] = GamePlay.S.Murderers[0].GetComponent<Murderer>();
        murderers[1] = GamePlay.S.Murderers[1].GetComponent<Murderer>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(arrestKey))
        {
            Vector3 detectivePos = gameObject.transform.position;
            // Check for murderers within arrest range
            for(int i=0; i<murderers.Length; i++)
            {
                if (murderers[i] && murderers[i].alive)
                {
                    Vector3 murdererPos = murderers[i].transform.position;
                    if ((detectivePos - murdererPos).magnitude < 1)
                    {
                        murderers[i].Kill();
                    }
                }
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
