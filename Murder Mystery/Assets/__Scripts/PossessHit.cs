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
            target.possess(ghostOwner);
            Destroy(this.gameObject);
        }
    }
}
