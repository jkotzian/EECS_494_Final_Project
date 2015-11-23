using UnityEngine;
using System.Collections;

public class HideLight : MonoBehaviour {

    public Light light;
    public Light light2;
    public bool alwaysHidden;
    bool hidden = false;

    void OnPreCull()
    {
        if (light != null && (hidden || alwaysHidden))
            light.enabled = false;
        if (light2 != null && (hidden || alwaysHidden))
            light2.enabled = false;
    }

    void OnPreRender()
    {
        if (light != null && (hidden || alwaysHidden))
            light.enabled = false;
        if (light2 != null && (hidden || alwaysHidden))
            light2.enabled = false;
    }
    void OnPostRender()
    {
        if (light != null && (hidden || alwaysHidden))
            light.enabled = true;
        if (light2 != null && (hidden || alwaysHidden))
            light2.enabled = true;
    }

    public void hideLight() {
        hidden = true;
    }

    public void unhideLight() {
        hidden = false;
    }
}
