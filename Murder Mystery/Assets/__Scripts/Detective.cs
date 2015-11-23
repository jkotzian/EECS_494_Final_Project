using UnityEngine;
using System.Collections;

public class Detective : Human {
    private KeyCode     arrestKey;
    private Ghost[]     ghosts;
    private Movement movement;

    public void setArrestKey(KeyCode key)
    {
        arrestKey = key;
    }
    
	void Awake () {
        // Set default arrest key
        setArrestKey(KeyCode.RightShift);
        // Link Ghost objects from GamePlay singleton GameObjects
        ghosts = new Ghost[2];
        ghosts[0] = GamePlay.S.Ghosts[0].GetComponent<Ghost>();
        ghosts[1] = GamePlay.S.Ghosts[1].GetComponent<Ghost>();
        movement = transform.GetComponent<Movement>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(arrestKey))
        {
            Vector3 detectivePos = gameObject.transform.position;
            // Check for ghosts within arrest range
            for(int i=0; i<ghosts.Length; i++)
            {
                if (ghosts[i] && ghosts[i].alive)
                {
                    Vector3 ghostPos = ghosts[i].transform.position;
                    if ((detectivePos - ghostPos).magnitude < 1)
                    {
                        ghosts[i].Kill();
                    }
                }
            }
            foreach(GameObject g in GamePlay.S.NPCs)
            {
                NPC npc = g.GetComponent<NPC>();
                if (npc.possessed)
                {
                    Vector3 ghostPos = g.transform.position;
                    if((detectivePos - ghostPos).magnitude < 1)
                    {
                        npc.possessed = false;
                        // Disable the movement
                        npc.NPCMovement.enabled = false;
                        // Re-enable the possession owner wherever the NPC is with an offset
                        Vector3 offset = new Vector3(0, .3f, 0);
                        npc.possessionOwner.transform.position = g.transform.position + offset;
                        npc.possessionOwner.gameObject.SetActive(true);
                        npc.possessionOwner.Kill();
                        npc.possessionOwner = null;
                    }
                }
            }
        }
    }

    void OnTriggerStay(Collider collider)
    {
        print(collider);
        Door door = collider.GetComponent<Door>();
        if (door)
        {
            if (Input.GetKeyDown(movement.upKey) && door.above)
            {
                door.MoveUp(gameObject);    
            }
            else if (Input.GetKeyDown(movement.downKey) && door.below)
            {
                door.MoveDown(gameObject);
            }
        }
        ////Make Arrest
        //if (Input.GetKey(KeyCode.RightShift))
        //{
        //    // See if the object is a murderer
        //    Ghost murderer = collisionInfo.collider.gameObject.GetComponent<Ghost>();


        //}
    }
}
