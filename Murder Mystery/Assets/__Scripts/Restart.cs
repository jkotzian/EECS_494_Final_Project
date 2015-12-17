using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Restart : MonoBehaviour {
    public List<Text> texts;
    public List<Text> endTexts;


	void Start()
    {
        string message = "";
        string winScore = "";
        string loseScore = "";
        switch (TotalGame.S.round)
        {
            /*case 1:
                message = "Now switch roles \n(Swap controllers and monitors).\nPress Space to continue.";
                break;
            case 2:
                message = "Switch roles \n(Swap controllers and monitors).\n\nWe're going to keep score now!\nPress Space to continue.";
                break;*/
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
                    if(endTexts.Count > 5)
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(TotalGame.S.round < 2)
            {
                Application.LoadLevel("AlphaMansion");
            }
            else
            {
                Application.LoadLevel("MainMenu");
            }
        }
	}
}
