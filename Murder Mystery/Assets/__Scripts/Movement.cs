using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
    public float           speed;

    public KeyCode         upKey;
    public KeyCode         downKey;
    public KeyCode         leftKey;
    public KeyCode         rightKey;
    public KeyCode         boostKey;

    private Human human;

	public bool isDetective;
	public bool isMurderer;

    public void setUDLRKeys(KeyCode up, KeyCode down, KeyCode left, KeyCode right) {
        upKey = up;
        downKey = down;
        leftKey = left;
        rightKey = right;
        print(upKey);
        print(downKey);
        print(leftKey);
        print(rightKey);
    }

	public void setBoostKey(KeyCode boost) {
		boostKey = boost;
	}

    void Awake() {
        setUDLRKeys(KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow);
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
                human.facingRight = true;
            }
            if (Input.GetKey(leftKey))
            {
                transform.Translate(Vector3.left * Time.deltaTime * speed);
                human.facingRight = false;
            }

			if (Input.GetKey(rightKey) && isDetective && Input.GetKey(boostKey))
			{
				transform.Translate(Vector3.right * Time.deltaTime * speed * 1.5f);
			}
			if (Input.GetKey(leftKey) && isDetective && Input.GetKey(boostKey))
			{
				transform.Translate(Vector3.left * Time.deltaTime * speed * 1.5f);
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
