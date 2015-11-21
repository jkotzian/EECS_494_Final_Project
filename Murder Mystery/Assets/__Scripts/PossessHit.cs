using UnityEngine;
using System.Collections;

public class PossessHit : MonoBehaviour
{
    public int lifetimeMax;
    public Ghost ghostOwner;
    public Vector3 offset;
    int lifetime;

    // Use this for initialization
    void Start()
    {
        lifetime = lifetimeMax;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = ghostOwner.transform.position + offset;
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
        NPC target = collision.gameObject.GetComponent<NPC>();
        if (target)
        {
            Possess(target);
        }
    }

    public void Possess(NPC target)
    {
        target.isStabbed = true;
    }
}
