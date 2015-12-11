using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class TotalGame : MonoBehaviour {
    public static TotalGame S;

    public GameObject NGDetective;
    public GameObject NGGhost;
    public GameObject TDetective;
    public GameObject TGhost;

    public int round;
    public List<int> bodyCount;
    int selected;

    public bool inMainMenu;

    void Awake()
    {
        inMainMenu = true;
        S = this;
        round = 0;
        bodyCount[0] = 0;
        bodyCount[1] = 0;
        selected = 0;
        NGDetective.SetActive(true);
        NGGhost.SetActive(true);
        TDetective.SetActive(false);
        TGhost.SetActive(false);
        DontDestroyOnLoad(transform.gameObject);
    }

    // Update is called once per frame
    void Update() {
        if (inMainMenu) { 
            int numControllers = InputManager.Devices.Count;
            bool actionPressed = false;
            bool upPressed = false;
            bool downPressed = false;
            float analogAngleLeeway = .3f;
            float analogThrustThreshold = .9f;
            for (int i = 0; i < numControllers; i++)
            {
                InputDevice controller = InputManager.Devices[i];
                bool up = ((controller.LeftStick.Vector.x > -analogAngleLeeway &&
                   controller.LeftStick.Vector.x < analogAngleLeeway &&
                   controller.LeftStick.Vector.y > 0 &&
                   controller.LeftStickY > analogThrustThreshold) ||
                   controller.DPadUp);
                bool down = ((controller.LeftStick.Vector.x > -analogAngleLeeway &&
                          controller.LeftStick.Vector.x < analogAngleLeeway &&
                          controller.LeftStick.Vector.y < 0 &&
                          controller.LeftStickY < -analogThrustThreshold) ||
                          controller.DPadDown);
                if (controller.Action1.WasPressed)
                {
                    actionPressed = true;
                }
                if (up)
                {
                    upPressed = true;
                }
                if (down)
                {
                    downPressed = true;
                }
            }
            if (upPressed && selected == 1)
            {
                NGDetective.SetActive(true);
                NGGhost.SetActive(true);
                TDetective.SetActive(false);
                TGhost.SetActive(false);
                selected = 0;
            }
            if (downPressed && selected == 0)
            {
                TDetective.SetActive(true);
                TGhost.SetActive(true);
                NGDetective.SetActive(false);
                NGGhost.SetActive(false);
                selected = 1;
            }
            if (Input.GetKeyDown(KeyCode.Space) || actionPressed)
            {
                // Regular Game
                if (selected == 0)
                {
                    StartCoroutine(startGame());
                }
                // Tutorial
                else if (selected == 1)
                {
                    StartCoroutine(startTutorial());
                }
            }
        }
    }

    IEnumerator startGame()
    {
        AudioSource weaponFireSound = NGDetective.GetComponent<Detective>().weaponFireSoundObj.GetComponent<AudioSource>();
        weaponFireSound.Play();
        NGDetective.GetComponent<Detective>().weaponEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        inMainMenu = false;
        Application.LoadLevel("AlphaMansion");
    }

    IEnumerator startTutorial()
    {
        AudioSource weaponFireSound = TDetective.GetComponent<Detective>().weaponFireSoundObj.GetComponent<AudioSource>();
        weaponFireSound.Play();
        TDetective.GetComponent<Detective>().weaponEffect.gameObject.SetActive(true);
        //inMainMenu = false;
        yield return new WaitForSeconds(1f);
        //Application.LoadLevel("AlphaMansion");
    }
}
