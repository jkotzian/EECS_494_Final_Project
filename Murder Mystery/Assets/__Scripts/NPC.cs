using UnityEngine;
using System.Collections;

public class NPC : Human {

    private int         checkMoveTimerMin;
    private int         checkMoveTimerMax;
    private int         checkMoveTimer;
    private int         standingTime;

    private bool        moving;
    private bool        movingRight;
    private int         speed;

    public void setTimerValues(int min, int max, int standing)
    {
        checkMoveTimerMin = min;
        checkMoveTimerMax = max;
        standingTime = standing;
        // Randomely set the next time the NPC will check its direction between the min and the max
        checkMoveTimer = Random.Range(checkMoveTimerMin, checkMoveTimerMax + 1);
    }

	// Use this for initialization
	void Awake () {
        // Set default timer values
        setTimerValues(40, 70, 150);
        // Set default values
        moving = false;
        movingRight = false;
        speed = 1;
	}
	
	// Update is called once per frame
	void Update () {
     
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        

        if (checkMoveTimer > 0)
        {
            --checkMoveTimer;
            if (checkMoveTimer == 0)
            {
                // If the NPC was moving, have it stand still
                if (moving)
                {
                    moving = false;
                    checkMoveTimer = standingTime;
                    return;
                }
                // Randomely decide the next movement of the NPC
                int moveNextRandNum = Random.Range(0, 101);
                // Go left
                if (moveNextRandNum < 50)
                {
                    moving = true;
                    movingRight = false;
                }
                // Go right
                else
                {
                    moving = true;
                    movingRight = true;
                }
                // Randomely set the next time the NPC will check its direction between the min and the max
                checkMoveTimer = Random.Range(checkMoveTimerMin, checkMoveTimerMax + 1);
            }
        }

        if (moving)
        {
            if (movingRight)
            {
                transform.Translate(Vector3.right * Time.deltaTime * speed);
            }
            else
            {
                transform.Translate(Vector3.left * Time.deltaTime * speed);
            }
        }
	}
}
