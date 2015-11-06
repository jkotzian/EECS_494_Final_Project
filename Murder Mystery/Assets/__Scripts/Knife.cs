using UnityEngine;
using System.Collections;

public class Knife : MonoBehaviour {

    public int 			lifetimeMax;
    public Murderer		murdererOwner;
    public Vector3		offset;
    int lifetime;
    
	// Use this for initialization
	void Start () {
        lifetime = lifetimeMax;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.position = murdererOwner.transform.position + offset;
        if (lifetime > 0)
        {
            lifetime--;
            if (lifetime == 0)
            {
                Destroy(this.gameObject);
            }
        }
	}
	
	void OnCollisionEnter(Collision collision)
	{
		Human human = collision.gameObject.GetComponent<Human>();
		if (human) {
			Stab(human);
		}
	}
	
	public void Stab(Human target) 
	{
		target.isStabbed = true;
		murdererOwner.track();
	}
}
