using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using InControl;

public class Restart : MonoBehaviour {
    public List<Text> texts;
    public List<Text> endTexts;
    public GameObject cameraObj;
    private Rect leftScreen;
    private Rect rightScreen;


    void Start()
    {
        leftScreen = new Rect(0, 0, 0.5f, 1);
        rightScreen = new Rect(0.5f, 0, 0.5f, 1);
        string message = "";
        string winScore = "";
        string loseScore = "";
        switch (TotalGame.S.round)
        {
            //case 3:
            case 1:
                message = (TotalGame.S.bodyCount[0] * 100).ToString();
                break;

            //case 4:
            case 2:    
                if (TotalGame.S.bodyCount[0] > TotalGame.S.bodyCount[1])
                {
                    winScore = (TotalGame.S.bodyCount[0] * 100).ToString();
                    loseScore = (TotalGame.S.bodyCount[1] * 100).ToString();
                }
                else if (TotalGame.S.bodyCount[1] > TotalGame.S.bodyCount[0])
                {
                    winScore = (TotalGame.S.bodyCount[1] * 100).ToString();
                    loseScore = (TotalGame.S.bodyCount[0] * 100).ToString();
                }
                else
                {
                    if (endTexts.Count > 5)
                    {
                        endTexts[4].text = "TIE!";
                        endTexts[5].text = "TIE!";
                    }
                    winScore = (TotalGame.S.bodyCount[0] * 100).ToString();
                    loseScore = (TotalGame.S.bodyCount[1] * 100).ToString();
                }
                break;
            default:
                message = "invalid round number";
                break;
        }
        foreach(Text t in texts)
        {
            t.text = message;
        }
        for (int i = 0; i < 4 && i < endTexts.Count; i++)
        {
            if(i % 2 == 0)
            {
                endTexts[i].text = winScore;
            }
            else
            {
                endTexts[i].text = loseScore;
            }
        }
    }

	void Update () {
        bool actionPressed = false;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            actionPressed = true;
        }

        int numControllers = InputManager.Devices.Count;

        for (int i = 0; i < numControllers; i++)
        {
            InputDevice controller = InputManager.Devices[i];
            if (controller.Action1.WasPressed)
            {
                actionPressed = true;
            }
        }

        if (actionPressed)
        {
            if (TotalGame.S.round < 2)
            {
                TotalGame.S.inReady = true;
                Application.LoadLevel("ReadyScreen");
            }
            else
            {
                Application.LoadLevel("MainMenu");
            }
        }
	}
}
