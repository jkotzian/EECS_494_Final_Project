using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	public float speed;

    public KeyCode          rightKey;
    public KeyCode          leftKey;
    public KeyCode          upKey;
    public KeyCode          downKey;

    public Human human;
	// Use this for initialization
	void Start () {
        human = gameObject.GetComponent<Human>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (human.alive)
        {
            if (!ClimbScript.S.canMove)
            {
                gameObject.GetComponent<Rigidbody>().useGravity = true;
            }

            if (Input.GetKey(rightKey))
            {
                transform.Translate(Vector3.right * Time.deltaTime * speed);
            }
            if (Input.GetKey(leftKey))
            {
                transform.Translate(Vector3.left * Time.deltaTime * speed);
            }

            if (ClimbScript.S.canMove)
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
            }
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
