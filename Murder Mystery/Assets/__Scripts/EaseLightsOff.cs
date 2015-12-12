using UnityEngine;
using System.Collections;

public class EaseLightsOff : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (GamePlay.S.gameOver) {
			gameObject.GetComponent<Light>().intensity -= gameObject.GetComponent<Light>().intensity*.015f;
		}
	}
}
