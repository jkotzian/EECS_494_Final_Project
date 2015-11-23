﻿using UnityEngine;
using System.Collections;

public class GhostHit : MonoBehaviour {
    public int lifetimeMax;
    public Detective detectiveOwner;
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
        transform.position = detectiveOwner.transform.position + offset;
        /*lifetime--;
        if (lifetime == 0)
        {
            this.gameObject.SetActive(false);
        }*/
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
        if (npc && npc.possessed)
        {
            npc.dispossess();
        }
    }
}
