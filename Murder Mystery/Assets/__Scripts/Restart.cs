using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Restart : MonoBehaviour {
    public List<Text> texts;


	void Start()
    {
        string message;
        switch (TotalGame.S.round)
        {
            case 1:
                message = "Round 1 Body Count: " + TotalGame.S.bodyCount[0];
                break;

            case 2:
                message = "Round 2 Body Count: " + TotalGame.S.bodyCount[1];
                break;
            default:
                message = "invalid round number";
                break;
        }
        foreach(Text t in texts)
        {
            t.text = message;
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
