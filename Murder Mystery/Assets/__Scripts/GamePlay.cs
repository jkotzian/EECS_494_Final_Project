using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using InControl;

public class GamePlay : MonoBehaviour {
    public static GamePlay S;

    public bool             tutorial;

    public GameObject       switchPrefab;
    public GameObject       bookshelfPrefab;
    public GameObject       npcPrefab;
    public GameObject       ghostPrefab;
    public GameObject       detectivePrefab;
    public GameObject       gIndicatorPrefab1;
    public GameObject       gIndicatorPrefab2;
    public GameObject       dIndicatorPrefab1;
    public GameObject       dIndicatorPrefab2;
    public List<Material>   disguises;
    public GUIText          timerText;
    public GUIText          scoreText;
    public GameObject		chandelierPrefab;
	public GameObject		knightAxePrefab;
	public GameObject		toxicAreaPrefab;
	public GameObject		flamethrowerPrefab;
	public GameObject		poisonWaterPrefab;
	public GameObject		pianoPrefab;
    public GameObject       detectiveCameraObj;
    public GameObject       ghostCameraObj;
    HideLight               ghostCamera;

    public int numNPCs;
    public int numFloors;
    // As of now, this value can only be 2 or 4
    public int numPlayers;
    public List<GameObject> NPCs;
    public List<GameObject> Ghosts;
    public List<GameObject> Detectives;
	public List<GameObject> EnvironmentalObjects;
	public List<GameObject> Switches;

    private List<Vector3> startLoc;
    private Dictionary<Vector3, bool> bookshelfDic;
    private List<bool> bookshelfPresent;

    private int[] targetIndices;
    private float starttime;
    int roundTime;
    public int regularRoundTime;
    public int practiceRoundTime;
    private float bookshelfSpawnTime;
    private Rect leftScreen;
    private Rect rightScreen;
    public bool gameOver;

    public int numControllers;

    public RuntimeAnimatorController detective1AnimationController;
    public RuntimeAnimatorController detective2AnimationController;

    public RuntimeAnimatorController guest1AnimationController;
    public RuntimeAnimatorController guest2AnimationController;
    public RuntimeAnimatorController guest3AnimationController;

    public Transform tutorialStartLocGhost1;
    public Transform tutorialStartLocGhost2;
    public Transform tutorialStartLocDetective1;
    public Transform tutorialStartLocDetective2;

    public GameObject doneTextGhost1;
    public GameObject doneText2Ghost1;
    public GameObject doneTextGhost2;
    public GameObject doneText2Ghost2;
    public GameObject doneTextDetective1;
    public GameObject doneText2Detective1;
    public GameObject doneTextDetective2;
    public GameObject doneText2Detective2;

    public GameObject completedText1;
    public GameObject completedText2;

    int tutorialObjectivesCompleted;

    public AudioSource WindDown;
    public AudioSource wilhelm;
    bool invoked;

    void Awake()
    {
        S = this;
        NPCs = new List<GameObject>();
        Ghosts = new List<GameObject>();
        Detectives = new List<GameObject>();
        targetIndices = Enumerable.Repeat(-1, 4).ToArray();
		EnvironmentalObjects = new List<GameObject> ();
        leftScreen = new Rect(0, 0, 0.5f, 1);
        rightScreen = new Rect(0.5f, 0, 0.5f, 1);
        startLoc = generateStartLoc();
        bookshelfDic = generateBookshelfDic();
        ghostCamera = ghostCameraObj.GetComponent<HideLight>();
        numControllers = 0;
        tutorialObjectivesCompleted = 0;
    }
    
    void Start () {
        if (tutorial)
        {
            tutorialGamePlayStart();
        }
        else
        {
            regularGamePlayStart();
        }
    }

    void Update()
    {
        if (tutorial)
        {
            tutorialGameUpdate();
        }
        else
        {
            regularGameUpdate();
        }
    }

    void tutorialGamePlayStart()
    {
        // See if the players are using the controllers
        numControllers = InputManager.Devices.Count;
        createGhostsAndDetectives(tutorialStartLocGhost1.position, tutorialStartLocGhost2.position,
                                  tutorialStartLocDetective1.position, tutorialStartLocDetective2.position);
        TotalGame.S.round++;

        // NOTE MAKE THIS INTO A FUNCITON AND CALL IT IN REGULAR GAMEPLAY
        // set detective and ghost cameras to correct side.
        if (TotalGame.S.round == 2)
        {
            detectiveCameraObj.GetComponent<Camera>().rect = leftScreen;
            ghostCameraObj.GetComponent<Camera>().rect = rightScreen;
            switchControllers();
        }
        else
        {
            detectiveCameraObj.GetComponent<Camera>().rect = rightScreen;
            ghostCameraObj.GetComponent<Camera>().rect = leftScreen;
        }
    }

    void regularGamePlayStart()
    {
        roundTime = practiceRoundTime;
        print("Num controllers " + InputManager.Devices.Count);
        // See if the players are using the controllers
        numControllers = InputManager.Devices.Count;
        //print("Num of devices " + InputManager.Devices.Count);
        // Place NPCs
        int locationIndex = 0;
        while (locationIndex < numNPCs)
        {
            GameObject newNPC = Instantiate(npcPrefab, startLoc[locationIndex], Quaternion.identity) as GameObject;
            newNPC.gameObject.name = "NPC " + locationIndex.ToString();
            // Add an order in layer so it's not weird when NPCs overlap
            newNPC.GetComponent<SpriteRenderer>().sortingOrder = locationIndex;
            // Randomely assign a party guest to them
            int animationNum = Random.Range(0, 3);
            if (animationNum == 1)
                newNPC.GetComponent<Animator>().runtimeAnimatorController = guest1AnimationController;
            else if (animationNum == 2)
                newNPC.GetComponent<Animator>().runtimeAnimatorController = guest2AnimationController;
            else
                newNPC.GetComponent<Animator>().runtimeAnimatorController = guest3AnimationController;
            NPCs.Add(newNPC);
            ++locationIndex;
        }

        // If it's 4 players, then there will be enough start locs to pass in with 4 increments
        if (numPlayers == 4)
        {
            createGhostsAndDetectives(startLoc[locationIndex++], startLoc[locationIndex++],
                                        startLoc[locationIndex++], startLoc[locationIndex++]);
        }
        // Otherwise, you can't increment and access that far in the startLoc array or else you'll
        // get access errors
        else
        {
            createGhostsAndDetectives(startLoc[locationIndex++], startLoc[locationIndex++],
                                      Vector3.zero, Vector3.zero);
        }

        locationIndex += 4;

        gameOver = false;
        starttime = Time.time;
        bookshelfSpawnTime = Time.time;
        TotalGame.S.round++;

        // set detective and ghost cameras to correct side.
        if (TotalGame.S.round % 2 == 0)
        {
            detectiveCameraObj.GetComponent<Camera>().rect = leftScreen;
            ghostCameraObj.GetComponent<Camera>().rect = rightScreen;
            switchControllers();
        }
        else
        {
            detectiveCameraObj.GetComponent<Camera>().rect = rightScreen;
            ghostCameraObj.GetComponent<Camera>().rect = leftScreen;
        }

        //if (TotalGame.S.round > 2)
        //{
        roundTime = regularRoundTime;
        //}
        timerText.text = "";
        scoreText.text = "";
    }

    void tutorialGameUpdate()
    {
        
    }

    void regularGameUpdate()
    {
        if (!gameOver)
        {
            timerText.text = "Time: " + (roundTime - (int)(Time.time - starttime)).ToString();
            if ((roundTime - (int)(Time.time - starttime)) == 5f && !invoked)
            {
                //playing wind down sound
                InvokeRepeating("PlayWindDown", 0f, 1f);
                invoked = true;
            }
        }
        if (!gameOver)
        {
            scoreText.text = "Score: " + (TotalGame.S.bodyCount[TotalGame.S.round - 1] * 100).ToString();
        }
        // check for round end
        if (Time.time > roundTime + starttime || checkForDetectiveWin())
        {
            gameOver = true;
            StartCoroutine(roundEndSequence());
        }
        // spawn bookshelves
        if (!gameOver && Time.time > bookshelfSpawnTime + 5)
        {
            bookshelfSpawnTime = Time.time;
            int i = UnityEngine.Random.Range(0, 6);
            List<Vector3> locs = Enumerable.ToList(bookshelfDic.Keys);
            int tries = 0;
            bool foundGoodLocation = false;
            // If you found a good location, use it
            if (bookshelfDic[locs[i]] == false)
            {
                foundGoodLocation = true;
            }
            // If the place is taken, try two more times to get an unvisited place
            while (bookshelfDic[locs[i]] == true && tries < 2)
            {
                ++tries;
                i = UnityEngine.Random.Range(0, 7);
                // See if the new place is taken
                if (bookshelfDic[locs[i]] == false)
                {
                    foundGoodLocation = true;
                    break;
                }
            }
            if (foundGoodLocation)
            {
                // Set that location as visited
                bookshelfDic[locs[i]] = true;
                GameObject bookshelf = Instantiate(bookshelfPrefab, locs[i], Quaternion.identity) as GameObject;
                bookshelf.GetComponent<Bookshelf>().index = i;
            }
            print("foundgoodlocation = " + foundGoodLocation);
        }
    }

    void createGhostsAndDetectives(Vector3 ghostPos1, Vector3 ghostPos2,
                                   Vector3 detectivePos1, Vector3 detectivePos2)
    {
        // Place Ghosts
        Ghosts.Add(Instantiate(ghostPrefab, ghostPos1, Quaternion.identity) as GameObject);
        Ghosts[0].GetComponent<Movement>().setUDLRKeys(KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D);
        Ghosts[0].GetComponent<Ghost>().alive = true;
        Ghosts[0].GetComponent<Ghost>().setActionKey(KeyCode.Q);
        Ghosts[0].GetComponent<Movement>().conNum = 0;
        GameObject gInd1 = Instantiate(gIndicatorPrefab1, Vector3.zero, Quaternion.identity) as GameObject;
        gInd1.transform.parent = Ghosts[0].transform;
        gInd1.transform.localPosition = new Vector3(-0.03f, 0.15f, 0);

        if (numPlayers == 4)
        {
            Ghosts.Add(Instantiate(ghostPrefab, ghostPos2, Quaternion.identity) as GameObject);
            Ghosts[1].GetComponent<Movement>().setUDLRKeys(KeyCode.T, KeyCode.G, KeyCode.F, KeyCode.H);
            Ghosts[1].GetComponent<Ghost>().alive = true;
            Ghosts[1].GetComponent<Ghost>().setActionKey(KeyCode.R);
            Ghosts[1].GetComponent<Movement>().conNum = 2;
            GameObject gInd2 = Instantiate(gIndicatorPrefab2, Vector3.zero, Quaternion.identity) as GameObject;
            gInd2.transform.parent = Ghosts[1].transform;
            gInd2.transform.localPosition = new Vector3(-0.03f, 0.15f, 0);
        }
        // Place Detectives
        Detectives.Add(Instantiate(detectivePrefab, detectivePos1, Quaternion.identity) as GameObject);

        Detectives[0].GetComponent<Movement>().setUDLRKeys(KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow);
        Detectives[0].GetComponent<Movement>().setBoostKey(KeyCode.M, KeyCode.N);
        Detectives[0].GetComponent<Movement>().isDetective = true;
        // Set the art/animation
        Detectives[0].GetComponent<Animator>().runtimeAnimatorController = detective1AnimationController;
        Detectives[0].GetComponent<Detective>().setActionKey(KeyCode.RightShift);
        Detectives[0].GetComponent<Movement>().conNum = 1;
        GameObject dInd1 = Instantiate(dIndicatorPrefab1, Vector3.zero, Quaternion.identity) as GameObject;
        dInd1.transform.parent = Detectives[0].transform;
        dInd1.transform.localPosition = new Vector3(-0.03f, 0.2f, 0);

        if (numPlayers == 4)
        {
            Detectives.Add(Instantiate(detectivePrefab, detectivePos2, Quaternion.identity) as GameObject);

            Detectives[1].GetComponent<Movement>().setUDLRKeys(KeyCode.I, KeyCode.K, KeyCode.J, KeyCode.L);
            Detectives[1].GetComponent<Movement>().setBoostKey(KeyCode.H, KeyCode.O);
            Detectives[1].GetComponent<Movement>().isDetective = true;
            Detectives[1].GetComponent<Detective>().setActionKey(KeyCode.U);
            Detectives[1].GetComponent<Movement>().conNum = 3;
            // Set the art/animation
            Detectives[1].GetComponent<Animator>().runtimeAnimatorController = detective2AnimationController;
            GameObject dInd2 = Instantiate(dIndicatorPrefab2, Vector3.zero, Quaternion.identity) as GameObject;
            dInd2.transform.parent = Detectives[1].transform;
            dInd2.transform.localPosition = new Vector3(-0.03f, 0.2f, 0);
        }
    }

    public void completedObjective(int objectiveNum)
    {
        if (!tutorial)
            return;

        if (objectiveNum == 0)
        {
            print("Objective 1 completed");
            doneTextGhost1.SetActive(true);
            doneText2Ghost1.SetActive(true);
        }
        else if (objectiveNum == 1)
        {
            print("Objective 2 completed");
            doneTextGhost2.SetActive(true);
            doneText2Ghost2.SetActive(true);
        }
        else if (objectiveNum == 2)
        {
            print("Objective 3 completed");
            doneTextDetective1.SetActive(true);
            doneText2Detective1.SetActive(true);
        }
        else if (objectiveNum == 3)
        {
            print("Objective 4 completed");
            doneTextDetective2.SetActive(true);
            doneText2Detective2.SetActive(true);
        }

        tutorialObjectivesCompleted++;

        if (tutorialObjectivesCompleted == 2 && numPlayers == 2)
        {
            doneTextGhost1.SetActive(false);
            doneText2Ghost1.SetActive(false);

            doneTextDetective1.SetActive(false);
            doneText2Detective1.SetActive(false);

            completedText1.SetActive(true);
            completedText2.SetActive(true);
            StartCoroutine(tutorialEndSequence());
        }
        else if (tutorialObjectivesCompleted == 4 && numPlayers == 4)
        {
            doneTextGhost1.SetActive(false);
            doneText2Ghost1.SetActive(false);
            doneTextGhost2.SetActive(false);
            doneText2Ghost2.SetActive(false);
            doneTextDetective1.SetActive(false);
            doneText2Detective1.SetActive(false);
            doneTextDetective2.SetActive(false);
            doneText2Detective2.SetActive(false);

            completedText1.SetActive(true);
            completedText2.SetActive(true);
            StartCoroutine(tutorialEndSequence());
        }
    }

	void PlayWindDown(){
		WindDown.Play ();
		if((roundTime - (int)(Time.time - starttime)) <= 0.5f){
			CancelInvoke();
			WindDown.Stop();
		}
	}
    void switchControllers()
    {
        // Switch the detective and ghost controls
        int oldGhostConNum = Ghosts[0].GetComponent<Movement>().conNum;
        print("Old Ghost Controller Number: " + oldGhostConNum);
        Ghosts[0].GetComponent<Movement>().conNum = Detectives[0].GetComponent<Movement>().conNum;
        Detectives[0].GetComponent<Movement>().conNum = oldGhostConNum;
        if (numPlayers > 2)
        {
            // Switch the detective and ghost controls
            int oldGhostConNum2 = Ghosts[1].GetComponent<Movement>().conNum;
            Ghosts[1].GetComponent<Movement>().conNum = Detectives[1].GetComponent<Movement>().conNum;
            Detectives[1].GetComponent<Movement>().conNum = oldGhostConNum2;
        }
        print("Ghost 0 Controller: " + Ghosts[0].GetComponent<Movement>().conNum);
        if(numPlayers > 2)
        {
            print("Ghost 1 Controller: " + Ghosts[1].GetComponent<Movement>().conNum);
        }
        print("Detective 0 Controller: " + Detectives[0].GetComponent<Movement>().conNum);
        if (numPlayers > 2)
        {
            print("Detective 1 Controller: " + Detectives[1].GetComponent<Movement>().conNum);
        }
    }

    IEnumerator roundEndSequence()
    {
        yield return new WaitForSeconds(3f);
        if(TotalGame.S.round < 2)
        {
            Application.LoadLevel("RoundEnd");
        }
        else
        {
            Application.LoadLevel("GameEnd");
        }
    }

    IEnumerator tutorialEndSequence()
    {
        yield return new WaitForSeconds(3f);
        if (TotalGame.S.round == 2)
            Application.LoadLevel("MainMenu");
        else
            Application.LoadLevel("Tutorial");
    }

    List<Vector3> generateStartLoc()
    {
        int numPeople = numNPCs + numPlayers;
        List<Vector3> sL = new List<Vector3>();
        int numPerFloor = (int)Mathf.Ceil(numPeople / (float)numFloors);
        // Fill the top floor with any remaining people
        // EXAMPLE: 10 people, 4 floors
        // numPerFloor will be 2
        // Floor 1: 2, Floor 2: 2, Floor 3: 2, Floor 4: 6
        int remainderPerFloor = (numPeople - (numPerFloor * numFloors)) + numPerFloor;
        float z = -0.2f;
        int currentFloor = 0;
        int currentPersonPerFloor = 0;
        // Create a location for every person
        for (int k = 0; k < numPeople; ++k)
        {
            // Place the y access according to the floor
            float y = (2.5f * currentFloor) - 4;
            float x = (float)((14f / numPerFloor) * (currentPersonPerFloor + 0.5f)) - 7;
            sL.Add(new Vector3(x, y, z));
            currentPersonPerFloor++;
            // Go up a floor if necessary
            if (currentPersonPerFloor == numPerFloor)
            {
                currentPersonPerFloor = 0;
                ++currentFloor;
                if (currentFloor == (numFloors - 1))
                    numPerFloor = remainderPerFloor;
            }
        }
        sL = sL.OrderBy(item => UnityEngine.Random.value).ToList<Vector3>();
        while (tooClose(sL.GetRange(numNPCs, numPlayers)))
        {
            sL = sL.OrderBy(item => UnityEngine.Random.value).ToList<Vector3>();
        }
        return sL;
    }

    bool tooClose(List<Vector3> loc)
    {
        // if there's two players, just check the ghost pos vs. the detective pos.
        if(numPlayers < 4)
        {
            return (Mathf.Abs(loc[0].x - loc[1].x) < 6);
        }
        // if there's four players, check ghost 1 pos vs detective 1 & 2 pos
        // then check ghost 2 pos vs detective 1 & 2 pos 
        else
        {
            if(((Mathf.Abs(loc[0].x - loc[2].x) < 6) /*&& (Mathf.Abs(loc[0].y - loc[2].y) < 1)*/)
                || ((Mathf.Abs(loc[0].x - loc[3].x) < 6) /*&& (Mathf.Abs(loc[0].y - loc[3].y) < 1)*/))
            {
                return true;
            }
            if(((Mathf.Abs(loc[1].x - loc[2].x) < 6) /*&& (Mathf.Abs(loc[1].y - loc[2].y) < 1)*/)
                || ((Mathf.Abs(loc[1].x - loc[3].x) < 6) /*&& (Mathf.Abs(loc[1].y - loc[3].y) < 1)*/))
            {
                return true;
            }
        }
        return false;
    }

    Dictionary<Vector3, bool> generateBookshelfDic()
    {
        Dictionary<Vector3, bool> plantLocs = new Dictionary<Vector3, bool>();
        plantLocs.Add(new Vector3(-1f, 3.5f, 0), false);
        plantLocs.Add(new Vector3(3.5f, 1f, 0), false);
        plantLocs.Add(new Vector3(-6.5f, -1.5f, 0), false);
        plantLocs.Add(new Vector3(1.5f, -1.5f, 0), false);
        plantLocs.Add(new Vector3(-4f, -4f, 0), false);
        plantLocs.Add(new Vector3(4f, -4f, 0), false);
        return plantLocs;
    }

    // Say that a bookshelf can now be spawned in the location
    // of that index
    public void addBackBookshelfLoc(int index)
    {
        // Get the locations from the dictionary
        List<Vector3> locs = Enumerable.ToList(bookshelfDic.Keys);
        bookshelfDic[locs[index]] = false;
    }

    bool checkForDetectiveWin()
    {
        foreach (GameObject m in Ghosts)
        {
            if (m.GetComponent<Ghost>().alive)
            {
                return false;
            }
        }
        return true;
    }
                 
    public void PlayWilhelm()
    {                                         
        wilhelm.Play();
    }

}
