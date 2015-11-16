using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Human : MonoBehaviour {
    public bool 		alive;
    public bool 		facingRight;
    public bool 		isStabbed;
	float				delayKill;
	public float        delayPerSecond; 
	Human				collidedHuman;
	public	 GameObject	trackerPrefab;

	void Start() {
		delayKill = 0;
		delayPerSecond = 0.2f;
	}
	
	void FixedUpdate() {
		if (isStabbed)
        {
            Kill();
            isStabbed = false;
            /*if (delayKill < 1)
			{
				delayKill += delayPerSecond * Time.deltaTime;
			}
			else
			{
				Kill();
				delayKill = 0;
				print("Killed");
				isStabbed = false;
				CancelInvoke();
			}*/
        }
	}
	
    public void Kill()
    {
        //Knock the murderer over
        transform.Rotate(new Vector3(0, 0, 90));
        alive = false;
    }

    public int GetLayerMask(string[] layerNames)
    {
        int layerMask = 0;

        foreach (string layer in layerNames)
        {
            layerMask = layerMask | (1 << LayerMask.NameToLayer(layer));
        }

        return layerMask;
    }
}
