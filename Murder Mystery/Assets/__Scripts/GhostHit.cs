using UnityEngine;
using System.Collections;

public class GhostHit : MonoBehaviour {
    public int lifetimeMax;
    public Detective detectiveOwner;
    public Vector3 offset;
    public int lifetime;

    // Use this for initialization
    void Awake()
    {
        lifetime = lifetimeMax;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = detectiveOwner.transform.position + offset;
        lifetime--;
        if (lifetime == 0)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        Ghost target = other.gameObject.GetComponent<Ghost>();
        if (target)
        {
            Human human = target.GetComponent<Human>();
            human.Kill();
        }
        NPC npc = other.gameObject.GetComponent<NPC>();
        if (npc)
        {
            if (npc.possessed)
                npc.dispossess();
            // Kill the party guest :o
            npc.Kill();
        }
        Bookshelf bookshelf = other.gameObject.GetComponent<Bookshelf>();
        if (bookshelf)
        {
            bookshelf.hp -= 50;
        }
    }
}
