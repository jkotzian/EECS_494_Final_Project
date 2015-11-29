using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class TotalGame : MonoBehaviour {
    public static TotalGame S;

    public int round;
    public List<int> bodyCount;

    private bool inMainMenu;

    void Awake()
    {
        inMainMenu = true;
        S = this;
        round = 0;
        bodyCount[0] = 0;
        bodyCount[1] = 0;
        DontDestroyOnLoad(transform.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        if ((Input.GetKeyDown(KeyCode.Space) /*|| ControllerManager.S.allControllers.Count == ControllerManager.S.setPlayers.Count*/ )
		    && inMainMenu)
        {
            inMainMenu = false;
            Application.LoadLevel("AlphaMansion");
        }
    }
}
