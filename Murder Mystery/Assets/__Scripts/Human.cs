using UnityEngine;
using System.Collections;

public class Human : MonoBehaviour {
    public bool alive;

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
