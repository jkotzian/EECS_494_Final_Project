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
    public GameObject CDetective;
    public GameObject CGhost;

    public int round;
    public List<int> bodyCount;
    int selected;

    public bool inMainMenu;
    public bool inReady;
    public bool inCredits;

    bool routineRunning;

    void Awake()
    {
        inMainMenu = true;
        inReady = false;
        S = this;
        round = 0;
        bodyCount[0] = 0;
        bodyCount[1] = 0;
        selected = 0;
        NGDetective.SetActive(true);
        NGGhost.SetActive(true);
        TDetective.SetActive(false);
        TGhost.SetActive(false);
        CDetective.SetActive(false);
        CGhost.SetActive(false);
        DontDestroyOnLoad(transform.gameObject);
        routineRunning = false;
    }

    // Update is called once per frame
    void Update() {
        if (inMainMenu) { 
            int numControllers = InputManager.Devices.Count;
            bool actionPressed = false;
            bool upPressed = false;
            bool downPressed = false;
            bool analogUpPressed = false;
            bool analogDownPressed = false;
            float analogAngleLeeway = .3f;
            float analogThrustThreshold = .9f;
            for (int i = 0; i < numControllers; i++)
            {
                InputDevice controller = InputManager.Devices[i];
                bool analogUp = ((controller.LeftStick.Vector.x > -analogAngleLeeway &&
                   controller.LeftStick.Vector.x < analogAngleLeeway &&
                   controller.LeftStick.Vector.y > 0 &&
                   controller.LeftStickY > analogThrustThreshold));

                bool analogDown = ((controller.LeftStick.Vector.x > -analogAngleLeeway &&
                          controller.LeftStick.Vector.x < analogAngleLeeway &&
                          controller.LeftStick.Vector.y < 0 &&
                          controller.LeftStickY < -analogThrustThreshold));

                upPressed = controller.DPadUp.WasPressed;
                downPressed = controller.DPadDown.WasPressed;

                if (controller.Action1.WasPressed)
                {
                    actionPressed = true;
                }
                if (analogUp)
                {
                    analogUpPressed = true;
                }
                if (analogDown)
                {
                    analogDownPressed = true;
                }
            }
            // Keyboard selection
            if (Input.GetKeyDown(KeyCode.UpArrow)) 
            {
                upPressed = true;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                downPressed = true;
            }
            // If there's not a directly pressed key or dpad, but the analog is used,
            // do the delay select
            if (!routineRunning && !upPressed && !downPressed && (analogUpPressed || analogDownPressed))
                StartCoroutine(checkForSelectionChange(analogUpPressed, analogDownPressed));
            if (upPressed || downPressed)
            {
                checkForSelectionChangeHelper(upPressed, downPressed);
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
                // Credits
                else if (selected == 2)
                {
                    StartCoroutine(startCredits());
                }
            }
        }
        if (inCredits)
        {
            bool actionPressed = false; 

            int numControllers = InputManager.Devices.Count;

            for (int i = 0; i < numControllers; i++)
            {
                InputDevice controller = InputManager.Devices[i];
                if (controller.Action1.WasPressed)
                {
                    actionPressed = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                actionPressed = true;
            }

            if (actionPressed)
            {
                Application.LoadLevel("MainMenu");
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator checkForSelectionChange(bool upPressed, bool downPressed)
    {
        routineRunning = true;
        yield return new WaitForSeconds(.15f);
        checkForSelectionChangeHelper(upPressed, downPressed);
        routineRunning = false;
    }

    void checkForSelectionChangeHelper(bool upPressed, bool downPressed)
    {
        //print("here");
        // Tutorial to New Game
        if (upPressed && selected == 1)
        {
            NGDetective.SetActive(true);
            NGGhost.SetActive(true);
            TDetective.SetActive(false);
            TGhost.SetActive(false);
            selected = 0;
        }
        // Credits to Tutorial
        else if (upPressed && selected == 2)
        {
            CDetective.SetActive(false);
            CGhost.SetActive(false);
            TDetective.SetActive(true);
            TGhost.SetActive(true);
            selected = 1;
        }
        // New Game to Tutorial
        if (downPressed && selected == 0)
        {
            TDetective.SetActive(true);
            TGhost.SetActive(true);
            NGDetective.SetActive(false);
            NGGhost.SetActive(false);
            selected = 1;
        }
        // Tutorial to New Game
        else if (downPressed && selected == 1)
        {
            CDetective.SetActive(true);
            CGhost.SetActive(true);
            TDetective.SetActive(false);
            TGhost.SetActive(false);
            selected = 2;
        }
    }

    IEnumerator startGame()
    {
        AudioSource weaponFireSound = NGDetective.GetComponent<Detective>().weaponFireSoundObj.GetComponent<AudioSource>();
        weaponFireSound.Play();
        NGDetective.GetComponent<Detective>().weaponEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        inReady = true;
        inMainMenu = false;
        Application.LoadLevel("ReadyScreen1");
    }

    IEnumerator startTutorial()
    {
        AudioSource weaponFireSound = TDetective.GetComponent<Detective>().weaponFireSoundObj.GetComponent<AudioSource>();
        weaponFireSound.Play();
        TDetective.GetComponent<Detective>().weaponEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        inReady = false;
        inMainMenu = false;
        Application.LoadLevel("Tutorial");
    }

    IEnumerator startCredits()
    {
        AudioSource weaponFireSound = CDetective.GetComponent<Detective>().weaponFireSoundObj.GetComponent<AudioSource>();
        weaponFireSound.Play();
        CDetective.GetComponent<Detective>().weaponEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        inReady = false;
        inMainMenu = false;
        inCredits = true;
        Application.LoadLevel("Credits");
    }
}
