using UnityEngine;
using System.Collections;

public class HideLight : MonoBehaviour {

    public Light light1;
    public Light light2;
    public bool alwaysHidden;
    bool hidden = false;

    void OnPreCull()
    {
        if (light1 != null && (hidden || alwaysHidden))
            light1.enabled = false;
        if (light2 != null && (hidden || alwaysHidden))
            light2.enabled = false;
    }

    void OnPreRender()
    {
        if (light1 != null && (hidden || alwaysHidden))
            light1.enabled = false;
        if (light2 != null && (hidden || alwaysHidden))
            light2.enabled = false;
    }
    void OnPostRender()
    {
        if (light1 != null && (hidden || alwaysHidden))
            light1.enabled = true;
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
