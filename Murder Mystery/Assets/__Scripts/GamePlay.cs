using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using InControl;

public class GamePlay : MonoBehaviour {
    public static GamePlay S;
    
	public GameObject		switchPrefab;
    public GameObject       npcPrefab;
    public GameObject       ghostPrefab;
    public GameObject       detectivePrefab;
    public List<Material>   disguises;
    public List<Text>       texts;
	public GameObject		chandelierPrefab;
	public GameObject		knightAxePrefab;
	public GameObject		toxicAreaPrefab;
	public GameObject		flamethrowerPrefab;
	public GameObject		poisonWaterPrefab;
	public GameObject		pianoTopPrefab;
	public GameObject		pianoBottomPrefab;
    public GameObject       ghostCameraObj;
    HideLight               ghostCamera;

    public int numNPCs = 8;
    public int numFloors = 4;
    // As of now, this value can only be 2 or 4
    public int numPlayers;
    public List<GameObject> NPCs;
    public List<GameObject> Ghosts;
    public List<GameObject> Detectives;
	public List<GameObject> EnvironmentalObjects;
	public List<GameObject> Switches;

    private List<Vector3> startLoc;

    private int[] targetIndices;
    private float starttime;
    private int roundtime;

    void Awake()
    {
        S = this;
        NPCs = new List<GameObject>();
        Ghosts = new List<GameObject>();
        Detectives = new List<GameObject>();
        targetIndices = Enumerable.Repeat(-1, 4).ToArray();
		EnvironmentalObjects = new List<GameObject> ();
        startLoc = generateStartLoc();
        ghostCamera = ghostCameraObj.GetComponent<HideLight>();
    }
    
    void Start () {
        //print("Num of devices " + InputManager.Devices.Count);
        // Place NPCs
        int locationIndex = 0;
        while (locationIndex < numNPCs)
        {
            NPCs.Add(Instantiate(npcPrefab, startLoc[locationIndex], Quaternion.identity) as GameObject);
            NPCs[locationIndex].GetComponent<Renderer>().material = disguises[0];
            NPCs[locationIndex].gameObject.name = "NPC " + locationIndex.ToString();
            ++locationIndex;
        }

        // Place Ghosts
        Ghosts.Add(Instantiate(ghostPrefab, startLoc[locationIndex], Quaternion.identity) as GameObject);
        Ghosts[0].GetComponent<Movement>().setUDLRKeys(KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D);
        Ghosts[0].GetComponent<Ghost>().alive = true;
        Ghosts[0].GetComponent<Ghost>().setPossessKey(KeyCode.E);
        Ghosts[0].GetComponent<Ghost>().conNum = ControllerManager.S.ghostOne;
		//Ghosts[0].GetComponent<Movement>().inputDevice = ControllerManager.S.allControllers[ControllerManager.S.ghostOne];
		Ghosts[0].GetComponent<Movement>().conNum = ControllerManager.S.ghostOne;
		
        ++locationIndex;

        if (numPlayers == 4)
        {
            Ghosts.Add(Instantiate(ghostPrefab, startLoc[locationIndex], Quaternion.identity) as GameObject);
            Ghosts[1].GetComponent<Movement>().setUDLRKeys(KeyCode.T, KeyCode.G, KeyCode.F, KeyCode.H);
            Ghosts[1].GetComponent<Ghost>().alive = true;
            Ghosts[1].GetComponent<Ghost>().setPossessKey(KeyCode.Y);
       		Ghosts[1].GetComponent<Ghost>().conNum = ControllerManager.S.ghostTwo;
			//Ghosts[1].GetComponent<Movement>().inputDevice = ControllerManager.S.allControllers[ControllerManager.S.ghostTwo];
			Ghosts[1].GetComponent<Movement>().conNum = ControllerManager.S.ghostTwo;
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
        Detectives[0].transform.GetChild(0).GetComponent<Renderer>().material = disguises[0];
        Detectives[0].GetComponent<Movement>().setUDLRKeys(KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow);
        Detectives[0].GetComponent<Movement>().setBoostKey(KeyCode.M, KeyCode.N);
		Detectives[0].GetComponent<Movement>().setLabel(765,380, "Player 1 Detective Mode: ");
		Detectives[0].GetComponent<Movement>().isDetective = true;

        // Hide the light from the ghost
        ghostCamera.light = Detectives[0].GetComponent<Detective>().aura;

		//Detectives[0].GetComponent<Movement>().inputDevice = ControllerManager.S.allControllers[ControllerManager.S.detectiveOne];
		Detectives[0].GetComponent<Detective>().setArrestKey(KeyCode.RightShift);
		Detectives[0].GetComponent<Movement>().conNum = ControllerManager.S.detectiveOne;
		Detectives[0].GetComponent<Detective>().conNum = ControllerManager.S.detectiveOne;
		
        ++locationIndex;
        
        if (numPlayers == 4)
        {
            Detectives.Add(Instantiate(detectivePrefab, startLoc[locationIndex], Quaternion.identity) as GameObject);
            Detectives[1].transform.GetChild(0).GetComponent<Renderer>().material = disguises[0];
            Detectives[1].GetComponent<Movement>().setUDLRKeys(KeyCode.I, KeyCode.K, KeyCode.J, KeyCode.L);
            Detectives[1].GetComponent<Movement>().setBoostKey(KeyCode.H, KeyCode.O);
            Detectives[1].GetComponent<Movement>().setLabel(765, 400, "Player 2 Detective Mode: ");
            Detectives[1].GetComponent<Movement>().isDetective = true;
            Detectives[1].GetComponent<Detective>().setArrestKey(KeyCode.U);
			//Detectives[1].GetComponent<Movement>().inputDevice = ControllerManager.S.allControllers[ControllerManager.S.detectiveTwo];
			Detectives[1].GetComponent<Movement>().conNum = ControllerManager.S.detectiveTwo;
			Detectives[1].GetComponent<Detective>().conNum = ControllerManager.S.detectiveTwo;
            // Hide the light from the ghost
            ghostCamera.light2 = Detectives[1].GetComponent<Detective>().aura;
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
		EnvironmentalObjects.Add (Instantiate(chandelierPrefab, new Vector3(4.9f, 2.059f, 0), Quaternion.Euler (0,0,90)) as GameObject);
		EnvironmentalObjects.Add (Instantiate(toxicAreaPrefab, new Vector3(-4.64f, -3.77f, 1), Quaternion.identity) as GameObject);
		EnvironmentalObjects.Add (Instantiate(flamethrowerPrefab, new Vector3(1.46f	, -1.37f, 0), Quaternion.Euler (0,0,90)) as GameObject);
		EnvironmentalObjects.Add (Instantiate(knightAxePrefab, new Vector3(-4.06f, 0.95f, 0), Quaternion.identity) as GameObject);
		EnvironmentalObjects.Add (Instantiate(pianoTopPrefab, new Vector3(-0.4f, 3.7f, 0), Quaternion.Euler (0,0,-50)) as GameObject);
		//print (EnvironmentalObjects [0]);

		// Place switches
		//Chandelier Switch
		Switches.Add (Instantiate(switchPrefab, new Vector3(4.91f, 1.12f, 0), Quaternion.identity) as GameObject);
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
		//Piano bottom Switch
		//Switches.Add (Instantiate(pianoBottomPrefab, new Vector3(0.08f, 3.62f, 0), Quaternion.Euler (0,0,20)) as GameObject);
		Switches.Add (Instantiate(switchPrefab, new Vector3(0.08f, 3.62f, 0), Quaternion.Euler (0,0,20)) as GameObject);
		Switches [6].GetComponent<Switch> ().switchNum = 7;

        starttime = Time.time;
        TotalGame.S.round++;
        roundtime = 30;
        if (TotalGame.S.round > 2)
        {
            roundtime += 90;
        }
        for (int i = 2; i < 4; i++)
        {
            texts[i].text = "";
        }
    }

    void Update()
    {
        for (int i = 0; i < 2; i++)
        {
            texts[i].text = (roundtime - (int)(Time.time - starttime)).ToString();
        }
        if(TotalGame.S.round > 2)
        {
            for (int i = 2; i < 4; i++)
            {
                texts[i].text = "Body Count: " + TotalGame.S.bodyCount[TotalGame.S.round - 3];
            }
        }
        //for (int j = 0; j < texts.Count; j++)
        //    print (texts [j].text);
        if (Time.time > roundtime + starttime || checkForDetectiveWin())
        {
            Application.LoadLevel("RoundEnd");
        }
    }

    List<Vector3> generateStartLoc()
    {
        int numPeople = numNPCs + numPlayers;
        List<Vector3> sL = new List<Vector3>();
        int numPerFloor = (int)Mathf.Ceil((float)((numPeople) / numFloors));
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
        return sL.OrderBy(item => UnityEngine.Random.value).ToList<Vector3>();
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
