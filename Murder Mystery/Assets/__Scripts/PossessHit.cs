using UnityEngine;
using System.Collections;

public class PossessHit : MonoBehaviour
{
    public int lifetimeMax;
    public Ghost ghostOwner;
    public Vector3 offset;
    public int lifetime;

    // Use this for initialization
    void Awake()
    {
        //lifetime = lifetimeMax;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = ghostOwner.transform.position + offset;
        /*lifetime--;
        if (lifetime == 0)
        {
            this.gameObject.SetActive(false);
        }*/
    }

    void OnCollisionEnter(Collision collision)
    {
        NPC target = collision.gameObject.GetComponent<NPC>();
        if (target)
        {
            // If the ghost is not currently possessing someone,
            // and the target is alive
            if (!ghostOwner.possessing && target.alive)
            {
                target.possess(ghostOwner);
                Destroy(this.gameObject);
            }
        }
    }
}
