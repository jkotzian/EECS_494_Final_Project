using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using InControl;

public class GamePlay : MonoBehaviour {
    public static GamePlay S;

    public GameObject       switchPrefab;
    public GameObject       bookshelfPrefab;
    public GameObject       npcPrefab;
    public GameObject       ghostPrefab;
    public GameObject       detectivePrefab;
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
    private List<Vector3> bookshelfLoc;
    private List<bool> bookshelfPresent;

    private int[] targetIndices;
    private float starttime;
    int roundTime;
    public int regularRoundTime;
    public int practiceRoundTime;
    private float bookshelfSpawnTime;
    private Rect leftScreen;
    private Rect rightScreen;
    private bool gameOver;

    public int numControllers;

    public RuntimeAnimatorController detective1AnimationController;
    public RuntimeAnimatorController detective2AnimationController;

    public RuntimeAnimatorController guest1AnimationController;
    public RuntimeAnimatorController guest2AnimationController;
    public RuntimeAnimatorController guest3AnimationController;

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
        bookshelfLoc = generateBookshelfLoc();
        ghostCamera = ghostCameraObj.GetComponent<HideLight>();
        numControllers = 0;
    }
    
    void Start () {
        roundTime = practiceRoundTime;
        print("Num controllers " + InputManager.Devices.Count);
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
        // See if the players are using the controllers
        numControllers = InputManager.Devices.Count;    
        // Place Ghosts
        Ghosts.Add(Instantiate(ghostPrefab, startLoc[locationIndex], Quaternion.identity) as GameObject);
        Ghosts[0].GetComponent<Movement>().setUDLRKeys(KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D);
        Ghosts[0].GetComponent<Ghost>().alive = true;
        Ghosts[0].GetComponent<Ghost>().setActionKey(KeyCode.Q);
        Ghosts[0].GetComponent<Movement>().conNum = 0;//ControllerManager.S.ghostOne;
		//Ghosts[0].GetComponent<Movement>().inputDevice = ControllerManager.S.allControllers[ControllerManager.S.ghostOne];
		
        ++locationIndex;

        if (numPlayers == 4)
        {
            Ghosts.Add(Instantiate(ghostPrefab, startLoc[locationIndex], Quaternion.identity) as GameObject);
            Ghosts[1].GetComponent<Movement>().setUDLRKeys(KeyCode.T, KeyCode.G, KeyCode.F, KeyCode.H);
            Ghosts[1].GetComponent<Ghost>().alive = true;
            Ghosts[1].GetComponent<Ghost>().setActionKey(KeyCode.R);
       		Ghosts[1].GetComponent<Movement>().conNum = 2;
			//Ghosts[1].GetComponent<Movement>().inputDevice = ControllerManager.S.allControllers[ControllerManager.S.ghostTwo];
            ++locationIndex;
        }
        // Randomely possess one of the NPCs
        //int randPossessNum = Random.Range(0, numNPCs);
        //Ghost ghost1 = Ghosts[0].GetComponent<Ghost>();
        //NPC npc1 = NPCs[1].GetComponent<NPC>();
        //ghost1.possess(npc1);
        //int randPossessNum2 = Random.Range(0, numNPCs);
        //while (randPossessNum2 == randPossessNum)
        //{
        //    randPossessNum2 = Random.Range(0, numNPCs);
        //}
        //Ghosts[1].GetComponent<Ghost>().possess(NPCs[2].GetComponent<NPC>());
        // Place Detectives
        Detectives.Add(Instantiate(detectivePrefab, startLoc[locationIndex], Quaternion.identity) as GameObject);
        // THIS MESSESS STUFF UP RIGHT NOW
        //Detectives[0].transform.GetChild(0).GetComponent<Renderer>().material = disguises[0];
        Detectives[0].GetComponent<Movement>().setUDLRKeys(KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow);
        Detectives[0].GetComponent<Movement>().setBoostKey(KeyCode.M, KeyCode.N);
		Detectives[0].GetComponent<Movement>().isDetective = true;
        // Set the art/animation
        Detectives[0].GetComponent<Animator>().runtimeAnimatorController = detective1AnimationController;
		Detectives[0].GetComponent<Detective>().setActionKey(KeyCode.RightShift);
        Detectives[0].GetComponent<Movement>().conNum = 1;
		
        ++locationIndex;
        
        if (numPlayers == 4)
        {
            Detectives.Add(Instantiate(detectivePrefab, startLoc[locationIndex], Quaternion.identity) as GameObject);

            Detectives[1].GetComponent<Movement>().setUDLRKeys(KeyCode.I, KeyCode.K, KeyCode.J, KeyCode.L);
            Detectives[1].GetComponent<Movement>().setBoostKey(KeyCode.H, KeyCode.O);
            Detectives[1].GetComponent<Movement>().isDetective = true;                      
            Detectives[1].GetComponent<Detective>().setActionKey(KeyCode.U);     
			Detectives[1].GetComponent<Movement>().conNum = 3;
            // Set the art/animation
            Detectives[1].GetComponent<Animator>().runtimeAnimatorController = detective2AnimationController;

            ++locationIndex;
        }

        //Set targets
        /*for (int i = 0; i < 4; i++)
        {
            int newIndex = UnityEngine.Random.Range(0, 8);
            while (Array.Exists(targetIndices, element => element == newIndex)) {
                newIndex = UnityEngine.Random.Range(0, 8);
            }
            targetIndices[i] = newIndex;
            NPCs[i].GetComponent<NPC>().target = true;
        }*/

        //Place Environmental Objects
        /*EnvironmentalObjects.Add (Instantiate(chandelierPrefab, new Vector3(4.9f, 2.059f, 0), Quaternion.Euler (0,0,90)) as GameObject);
		EnvironmentalObjects.Add (Instantiate(toxicAreaPrefab, new Vector3(-4.64f, -3.77f, 1), Quaternion.identity) as GameObject);
		EnvironmentalObjects.Add (Instantiate(flamethrowerPrefab, new Vector3(1.46f	, -1.37f, 0), Quaternion.Euler (0,0,90)) as GameObject);
		EnvironmentalObjects.Add (Instantiate(knightAxePrefab, new Vector3(-4.06f, 0.95f, 0), Quaternion.identity) as GameObject);
		EnvironmentalObjects.Add (Instantiate(pianoPrefab, new Vector3(-0.4f, 3.7f, 0), Quaternion.Euler (0,0,-50)) as GameObject);*/
        //print (EnvironmentalObjects [0]);

        // Place switches
        //Chandelier Switch
        /*Switches.Add (Instantiate(switchPrefab, new Vector3(4.91f, 1.12f, 0), Quaternion.identity) as GameObject);
		Switches [0].GetComponent<Switch> ().switchNum = 1;
		//Hole in ground Switch
		Switches.Add (Instantiate(switchPrefab, new Vector3(0.05f, -3.91f, 0), Quaternion.identity) as GameObject);
		Switches [1].GetComponent<Switch> ().switchNum = 2;
		//Toxic air Switch
		Switches.Add (Instantiate(switchPrefab, new Vector3(-4.32f, -3.85f, 0), Quaternion.identity) as GameObject);
		Switches [2].GetComponent<Switch> ().switchNum = 3;
		//Flamethrower Switch
		Switches.Add (Instantiate(switchPrefab, new Vector3(2.5f, -1.37f, 0), Quaternion.identity) as GameObject);
		Switches [3].GetComponent<Switch> ().switchNum = 4;
		//Poison water Switch
		Switches.Add (Instantiate(poisonWaterPrefab, new Vector3(-4.23f, -1.29f, 0), Quaternion.identity) as GameObject);
		Instantiate (switchPrefab, new Vector3 (-4.23f, -1.29f, 0), Quaternion.identity);
		Switches [4].GetComponent<Switch> ().switchNum = 5;
		//Knight axe Switch
		Switches.Add (Instantiate(switchPrefab, new Vector3(-4.83f, 1.2f, 0), Quaternion.identity) as GameObject);
		Switches [5].GetComponent<Switch> ().switchNum = 6;
		//Piano Switch
		Switches.Add (Instantiate(switchPrefab, new Vector3(0.08f, 3.62f, 0), Quaternion.Euler (0,0,20)) as GameObject);
		Switches [6].GetComponent<Switch> ().switchNum = 7;*/

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

        if (TotalGame.S.round > 2)
        {
            roundTime = regularRoundTime;
        }
        timerText.text = "";
        scoreText.text = "";   
    }

    void Update()
    {
        if(!gameOver)
        {
            timerText.text = "Time: " + (roundTime - (int)(Time.time - starttime)).ToString();
        }
        if(TotalGame.S.round > 2 && !gameOver)
        {
            scoreText.text = "Score: " + (TotalGame.S.bodyCount[TotalGame.S.round - 3] * 100).ToString();        
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
            int i = UnityEngine.Random.Range(0, 7);    
            Instantiate(bookshelfPrefab, bookshelfLoc[i], Quaternion.identity);      
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
        Application.LoadLevel("RoundEnd");
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
            return (Mathf.Abs(loc[0].x - loc[1].x) < 4);
        }
        // if there's four players, check ghost 1 pos vs detective 1 & 2 pos
        // then check ghost 2 pos vs detective 1 & 2 pos 
        else
        {
            if(((Mathf.Abs(loc[0].x - loc[2].x) < 7) && (Mathf.Abs(loc[0].y - loc[2].y) < 1))
                || ((Mathf.Abs(loc[0].x - loc[3].x) < 7) && (Mathf.Abs(loc[0].y - loc[3].y) < 1)))
            {
                return true;
            }
            if(((Mathf.Abs(loc[1].x - loc[2].x) < 7) && (Mathf.Abs(loc[1].y - loc[2].y) < 1))
                || ((Mathf.Abs(loc[1].x - loc[3].x) < 7) && (Mathf.Abs(loc[1].y - loc[3].y) < 1)))
            {
                return true;
            }
        }
        return false;
    }

    List<Vector3> generateBookshelfLoc()
    {
        List<Vector3> bl = new List<Vector3>();
        bl.Add(new Vector3(-1f, 3.5f, 0)); 
        bl.Add(new Vector3(-4f, 1f, 0));     
        bl.Add(new Vector3(3.5f, 1f, 0));
        bl.Add(new Vector3(-6.5f, -1.5f, 0));
        bl.Add(new Vector3(1.5f, -1.5f, 0));
        bl.Add(new Vector3(-4f, -4f, 0));
        bl.Add(new Vector3(4f, -4f, 0));
        return bl;
    }

    //bool checkForMurdererWin()
    //{
    //    foreach (int i in targetIndices)
    //    {
    //        if (NPCs[i].GetComponent<NPC>().alive)
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}

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

}
