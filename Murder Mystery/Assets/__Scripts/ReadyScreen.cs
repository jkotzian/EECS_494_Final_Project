using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine.UI;

public class ReadyScreen : MonoBehaviour {

    bool[] ready = new bool[4];

    public List<Text> pressTexts;
    public List<Text> readyTexts;

    void Start()
    {
        for(int i = 0; i < ready.Length; i++)
        {
            ready[i] = false;
            readyTexts[i].enabled = false;
            pressTexts[i].enabled = true;    
        }
    }
    // Update is called once per frame
    void Update() {        
        int numControllers = InputManager.Devices.Count;
        if (Input.GetKeyDown(KeyCode.Space) || (ready[0] && ready[1] && ready[2] && ready[3]))
        {
            TotalGame.S.inReady = false;
            Application.LoadLevel("Mansion");
        }  
        for (int i = 0; i < numControllers; i++)
        {
            InputDevice controller = InputManager.Devices[i];     
            if (controller.Action1.WasPressed)
            {
                ready[i] = !ready[i];
            }
            if (ready[i])
            {
                pressTexts[i].enabled = false;
                //readyTexts[i].enabled = true;
            }
            else
            {
                //readyTexts[i].enabled = false;
                pressTexts[i].enabled = true;
            }
        }  
    }              
}
