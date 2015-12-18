using UnityEngine;
using System.Collections;
using SpriteShatter;

public class Bookshelf : MonoBehaviour {

    Animator animator;
    int timer;
    int animationTime;
    public int index;
    public int shatterTime;
    int shatterTimer;
    public BoxCollider collider;
    bool dying;

	// Use this for initialization
	void Awake () {
        animator = GetComponent<Animator>();
        timer = 0;
        animationTime = 12;
        index = 0;
        shatterTime = 10;
        // Set it to -1 so it won't activate the destroy
        shatterTimer = 0;
        dying = false;
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        if (timer < animationTime) {
            ++timer;
        }
        else {
            animator.SetTrigger("Done");
        }
    }

    public void destroyPlant()
    {
        if (dying)
            return;
        GamePlay.S.addBackBookshelfLoc(index);
        GetComponent<Shatter>().shatter();
        dying = true;
        StartCoroutine(dieWithDelay());
    }

    IEnumerator dieWithDelay()
    {
        yield return new WaitForSeconds(.3f);
        Destroy(this.gameObject);
    }
}
