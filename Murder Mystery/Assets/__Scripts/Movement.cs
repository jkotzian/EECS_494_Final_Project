using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
    public float           speed;

    public KeyCode         upKey;
    public KeyCode         downKey;
    public KeyCode         leftKey;
    public KeyCode         rightKey;
    public KeyCode         boostKey;
	public KeyCode		   detectiveMode;	
    private Human human;

	public bool isDetective;
	public bool inDetectiveMode;
	public bool isGhost;

	public GameObject dModeBar;
	public float dModeTotal;
	public float dModeLoss;
	public float dModeRegain;

    public void setUDLRKeys(KeyCode up, KeyCode down, KeyCode left, KeyCode right) {
        upKey = up;
        downKey = down;
        leftKey = left;
        rightKey = right;

		dModeTotal = 100f;
		dModeLoss = 45f;
		dModeRegain = 10f;
    }

	public void setBoostKey(KeyCode boost, KeyCode dMode) {
		boostKey = boost;
		detectiveMode = dMode;
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
			if (Input.GetKey(detectiveMode) && dModeTotal > -50f){
				if(dModeTotal > 0f){
					inDetectiveMode = true;
					//print("Currently in D-Mode");
				}
				dModeTotal -= Time.deltaTime * dModeLoss;
			}
			
			if (Input.GetKeyUp(detectiveMode)){
				inDetectiveMode = false;
				//print("Left D-Mode");
			}

			if(dModeTotal < 100f){
				dModeTotal += Time.deltaTime * dModeRegain;
				//print("D - Total: " + dModeTotal);
			}

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
            // Move up for ghosts
            if (Input.GetKey(upKey) && isGhost)
            {
                transform.Translate(Vector3.up * Time.deltaTime * speed);
            }
            // Move up for ghosts
            if (Input.GetKey(downKey) && isGhost)
            {
                transform.Translate(Vector3.down * Time.deltaTime * speed);
            }
        }
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.name == "Stairs") {
			//print("Stairs");

			if (Input.GetKey (upKey)) {
				transform.Translate(Vector3.up*Time.deltaTime*speed);
			}
			if (Input.GetKey (downKey)) {
				transform.Translate(Vector3.down*Time.deltaTime*speed);
			}
		}
	}
}
