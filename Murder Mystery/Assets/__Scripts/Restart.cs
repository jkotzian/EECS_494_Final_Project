using UnityEngine;

public class Restart : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Application.LoadLevel("AlphaMansion");
        }
	}
}
