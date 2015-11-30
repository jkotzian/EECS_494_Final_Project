﻿using UnityEngine;
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
                message = "Now switch roles \n(Swap controllers and monitors).\nPress Space to continue.";
                break;
            case 2:
                message = "Switch roles \n(Swap controllers and monitors).\n\nWe're going to keep score now!\nPress Space to continue.";
                break;
            case 3:
                message = "Round 1 Body Count: " + TotalGame.S.bodyCount[0] + "\n\nPress Space for Round 2";
                break;

            case 4:
                string winner = "No winner";
                if (TotalGame.S.bodyCount[0] > TotalGame.S.bodyCount[1])
                {
                    winner = "Team 1 Wins!\n";
                }
                else if (TotalGame.S.bodyCount[1] > TotalGame.S.bodyCount[0])
                {
                    winner = "Team 2 Wins!\n";
                }
                else
                {
                    winner = "It's a tie!\n";
                }
                message = winner + "\nRound 1 Body Count: " + TotalGame.S.bodyCount[0] + "\nRound 2 Body Count: " + TotalGame.S.bodyCount[1];
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
            if(TotalGame.S.round < 4)
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
