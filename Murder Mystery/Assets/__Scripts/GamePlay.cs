using UnityEngine;
using System.Collections;

public class GamePlay : MonoBehaviour {

	public GameObject		switchPrefab;
	
	// Use this for initialization
	void Start () {
		Instantiate(switchPrefab, new Vector3(-4.78f, -3.54f, -0.2f), Quaternion.identity);
		Instantiate(switchPrefab, new Vector3(-7.46f, -1.24f, -0.2f), Quaternion.identity);
		Instantiate(switchPrefab, new Vector3(-1.76f, 1.3f, -0.2f), Quaternion.identity);
		Instantiate(switchPrefab, new Vector3(-4.68f, 3.74f, -0.2f), Quaternion.identity);
		Instantiate(switchPrefab, new Vector3(4.38f, 3.74f, -0.2f), Quaternion.identity);
		Instantiate(switchPrefab, new Vector3(7.09f, 1.3f, -0.2f), Quaternion.identity);
		Instantiate(switchPrefab, new Vector3(1.43f, -1.24f, -0.2f), Quaternion.identity);
		Instantiate(switchPrefab, new Vector3(4.25f, -3.75f, -0.2f), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
