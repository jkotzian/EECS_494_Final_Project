using UnityEngine;
using System.Collections;

public class HideLight : MonoBehaviour {

    public Light light1;
    //public Light light2;
    public Light lightning;
    public AudioSource lightningSound;
    public AudioSource lightSwitchSound;
    public AudioSource voltageSound;
    public bool alwaysHidden;
    bool hidden = false;

    void Awake()
    {
        lightning.enabled = false;
    }

    void OnPreCull()
    {
        if (light1 != null && (hidden || alwaysHidden))
        {                        
            light1.enabled = false;
        }
    }

    void OnPreRender()
    {
        if (light1 != null && (hidden || alwaysHidden))
        {                          
            light1.enabled = false;
        }
    }
    void OnPostRender()
    {
        if (light1 != null && (hidden || alwaysHidden))
        {                          
            light1.enabled = true;
        }
    }

    public void hideLight() {
        StartCoroutine(flashDown());
    }

    public void unhideLight() {
        StartCoroutine(flashUp());
    }

    public IEnumerator flashDown()
    {
        lightningSound.Play();
        yield return new WaitForSeconds(0.2f);
        hidden = true;
        lightning.enabled = true;
        yield return new WaitForSeconds(.1f);
        lightning.enabled = false;
        yield return new WaitForSeconds(.05f);
        lightning.enabled = true;
        yield return new WaitForSeconds(.2f);
        lightning.enabled = false;
        yield return new WaitForSeconds(.05f);
        lightning.enabled = true;
        yield return new WaitForSeconds(.05f);
        lightning.enabled = false;
        yield return new WaitForSeconds(.05f);
        lightning.enabled = true;
        yield return new WaitForSeconds(.05f);
        lightning.enabled = false;
        yield return new WaitForSeconds(.05f);
        lightning.enabled = true;
        yield return new WaitForSeconds(.05f);
        lightning.enabled = false;
    }

    public IEnumerator flashUp()
    {
        lightSwitchSound.Play();
        yield return new WaitForSeconds(.3f);
        voltageSound.Play();
        yield return new WaitForSeconds(.8f);
        lightning.enabled = true;
        yield return new WaitForSeconds(.1f);
        lightning.enabled = false;
        yield return new WaitForSeconds(.1f);
        lightning.enabled = true;
        yield return new WaitForSeconds(.4f);
        lightning.enabled = false;
        hidden = false;
    }
}
