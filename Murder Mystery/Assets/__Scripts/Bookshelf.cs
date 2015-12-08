using UnityEngine;
using System.Collections;

public class Bookshelf : MonoBehaviour {

    public int hp;

	// Use this for initialization
	void Start () {
        hp = 100;
	}
	
	// Update is called once per frame
	void Update () {
	    if(hp < 1)
        {
            DestroyObject(gameObject);
        }
	}
}
