using UnityEngine;
using System.Collections;

public class HideLight : MonoBehaviour {

    public Light mansionLight;
    bool hidden = false;

    void OnPreCull()
    {
        if (mansionLight != null & hidden)
            mansionLight.enabled = false;
    }

    void OnPreRender()
    {
        if (mansionLight != null && hidden)
            mansionLight.enabled = false;
    }
    void OnPostRender()
    {
        if (mansionLight != null && hidden)
            mansionLight.enabled = true;
    }

    public void hideLight() {
        hidden = true;
    }

    public void unhideLight() {
        hidden = false;
    }
}
