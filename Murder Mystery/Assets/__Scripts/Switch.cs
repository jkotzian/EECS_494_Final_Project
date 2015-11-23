using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Switch : MonoBehaviour {
	
	public Material			emptyMaterial;
	public int				switchNum;
	Light					lightObject;

	GameObject				murderer;
	Rigidbody               rigidbody2Dimensional;
	float 					moveSpeed = 1f;
	float					rotationAmount = 90f;
	float					pushPower = 2.0f;
	bool					isInfected = false;
	bool					isChopped = false;
	bool					droppedLid = false;

	//static public int		howManyTimes = 0;

	// Use this for initialization
	void Start () {
		lightObject = GameObject.Find ("Directional Light").GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
		if (isInfected && lightObject.enabled) {
			murderer.GetComponent<Human> ().Kill ();
			isInfected = false;
		}
	}

	void OnTriggerStay(Collider other){
		Movement movement = other.GetComponent<Movement> ();
		bool pressed = Input.GetKeyDown (KeyCode.E);
		if (movement && pressed) {
			print ("Hey");
			print (switchNum);
			if (switchNum == 1){	
				rigidbody2Dimensional = GamePlay.S.EnvironmentalObjects [0].GetComponent<Rigidbody> ();
                rigidbody2Dimensional.useGravity = true;
			}
			else if (switchNum == 2){
				other.GetComponent<BoxCollider>().enabled = false;
			}
			else if (switchNum == 3){
				Color tmpColor = GamePlay.S.EnvironmentalObjects [1].GetComponent<Renderer> ().material.color;
				tmpColor.a = 0.5f;
				GamePlay.S.EnvironmentalObjects [1].GetComponent<Renderer> ().material.color = tmpColor;
				Vector3 tmpPos = GamePlay.S.EnvironmentalObjects [1].GetComponent<Transform>().position;
				tmpPos.z = 0;
				GamePlay.S.EnvironmentalObjects [1].GetComponent<Transform>().position = tmpPos;
				other.gameObject.GetComponent<Human> ().Kill ();
			}
			else if (switchNum == 4){
				GamePlay.S.EnvironmentalObjects[2].GetComponent<Flamethrower>().flameOn = true;
			}
			else if (switchNum == 5){
				this.GetComponent<Renderer>().material = emptyMaterial;
				murderer = other.gameObject;
				isInfected = true;
			}
			else if (switchNum == 6 && !isChopped){
				GamePlay.S.EnvironmentalObjects[3].GetComponent<Transform>().Rotate(new Vector3(0, 0, 90));
				other.gameObject.GetComponent<Human> ().Kill ();
				isChopped = true;
			}
			else if (switchNum == 7 && !droppedLid){
				print ("Got in");
				GamePlay.S.EnvironmentalObjects[4].GetComponent<Transform>().Rotate(new Vector3(0, 0, 320));
				this.transform.Rotate(new Vector3(0, 0, -60));
				other.gameObject.GetComponent<Human> ().Kill ();
				droppedLid = true;
			}
		}
	}
}
