using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
    private float           speed;

    private KeyCode         upKey;
    private KeyCode         downKey;
    private KeyCode         leftKey;
    private KeyCode         rightKey;

    private Human human;

    public void setUDLRKeys(KeyCode up, KeyCode down, KeyCode left, KeyCode right) {
        upKey = up;
        downKey = down;
        leftKey = left;
        rightKey = right;
    }

    void Awake() {
        setUDLRKeys(KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow);
        speed = 1;
    }

	void Start() {
        human = gameObject.GetComponent<Human>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (human.alive)
        {
           
            gameObject.GetComponent<Rigidbody>().useGravity = true;
           

            if (Input.GetKey(rightKey))
            {
                transform.Translate(Vector3.right * Time.deltaTime * speed);
            }
            if (Input.GetKey(leftKey))
            {
                transform.Translate(Vector3.left * Time.deltaTime * speed);
            }

            /*if (ClimbScript.S.canMove)
            {
                gameObject.GetComponent<Rigidbody>().useGravity = false;

                if (Input.GetKey(upKey))
                {
                    transform.Translate(Vector3.up * Time.deltaTime * speed);
                }
                if (Input.GetKey(downKey))
                {
                    transform.Translate(Vector3.down * Time.deltaTime * speed);
                }
            }*/
        }
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.name == "Stairs") {
			print("Stairs");

			if (Input.GetKey (upKey)) {
				transform.Translate(Vector3.up*Time.deltaTime*speed);
			}
			if (Input.GetKey (downKey)) {
				transform.Translate(Vector3.down*Time.deltaTime*speed);
			}
		}
	}
}
