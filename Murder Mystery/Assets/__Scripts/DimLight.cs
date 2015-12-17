using UnityEngine;
using System.Collections;

public class DimLight : MonoBehaviour {

    public Light light1;
    //public Light light2; 
    public bool alwaysHidden;
    bool hidden = false;
                                      

    void OnPreCull()
    {
        if (light1 != null && (hidden || alwaysHidden))
        {
            light1.intensity = 1;     
        }
        //if (light2 != null && (hidden || alwaysHidden))
            //light2.enabled = false;
    }

    void OnPreRender()
    {
        if (light1 != null && (hidden || alwaysHidden))
        {
            light1.intensity = 1;      
        }
        //if (light2 != null && (hidden || alwaysHidden))
            //light2.enabled = false;
    }
    void OnPostRender()
    {
        if (light1 != null && (hidden || alwaysHidden))
        {
            light1.intensity = 3.5f;      
        }
        //if (light2 != null && (hidden || alwaysHidden))
            //light2.enabled = true;
    }

    public void dimLight() {
        StartCoroutine(flashDown());
    }

    public void undimLight() {
        StartCoroutine(flashUp());
    }

    public IEnumerator flashDown()
    {                        
        yield return new WaitForSeconds(0.2f);
        hidden = true;             
    }

    public IEnumerator flashUp()
    {                             
        yield return new WaitForSeconds(1.7f);       
        hidden = false;
    }
}
