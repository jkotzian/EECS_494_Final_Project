using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;

public class GamePlay : MonoBehaviour {
    public static GamePlay S;

	public GameObject		switchPrefab;
    public GameObject       npcPrefab;
    public GameObject       ghostPrefab;
    public GameObject       detectivePrefab;
    public List<Material>   disguises;
    public List<Text>       timerTexts;
	public GameObject		chandelierPrefab;
	public GameObject		knightAxePrefab;
	public GameObject		toxicAreaPrefab;
	public GameObject		flamethrowerPrefab;
	public GameObject		poisonWaterPrefab;
	public GameObject		pianoTopPrefab;
	public GameObject		pianoBottomPrefab;

    public int numNPCs = 8;
    public int numFloors = 4;
    public List<GameObject> NPCs;
    public List<GameObject> Ghosts;
    public List<GameObject> Detectives;
	public List<GameObject> EnvironmentalObjects;
	public List<GameObject> Switches;

    private List<Vector3> startLoc;

    private int[] targetIndices;
    private float starttime;

    void Awake()
    {
        S = this;
        NPCs = new List<GameObject>();
        Ghosts = new List<GameObject>();
        Detectives = new List<GameObject>();
        targetIndices = Enumerable.Repeat(-1, 4).ToArray();
		EnvironmentalObjects = new List<GameObject> ();
        startLoc = generateStartLoc();
    }
    
    void Start () {
        // Place NPCs
        for(int i = 0; i < numNPCs; i++)
        {
            NPCs.Add(Instantiate(npcPrefab, startLoc[i], Quaternion.identity) as GameObject);
            NPCs[i].GetComponent<Renderer>().material = disguises[UnityEngine.Random.Range(0, disguises.Count)];
        }

        // Place Ghosts
        Ghosts.Add(Instantiate(ghostPrefab, startLoc[numNPCs], Quaternion.identity) as GameObject);
        Ghosts[0].GetComponent<Movement>().setUDLRKeys(KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D);
        Ghosts[0].GetComponent<Ghost>().alive = true;
        Ghosts[0].GetComponent<Ghost>().setPossessKey(KeyCode.E);
        Ghosts.Add(Instantiate(ghostPrefab, startLoc[numNPCs+1], Quaternion.identity) as GameObject);
        Ghosts[1].GetComponent<Movement>().setUDLRKeys(KeyCode.T, KeyCode.G, KeyCode.F, KeyCode.H);
        Ghosts[1].GetComponent<Ghost>().alive = true;
        Ghosts[1].GetComponent<Ghost>().setPossessKey(KeyCode.Y);
        // Place Detectives
        Detectives.Add(Instantiate(detectivePrefab, startLoc[numNPCs+2], Quaternion.identity) as GameObject);
        Detectives[0].transform.GetChild(0).GetComponent<Renderer>().material = disguises[UnityEngine.Random.Range(0, disguises.Count)];
        Detectives[0].GetComponent<Movement>().setUDLRKeys(KeyCode.I, KeyCode.K, KeyCode.J, KeyCode.L);
        Detectives[0].GetComponent<Movement>().setBoostKey(KeyCode.H, KeyCode.O);
		Detectives[0].GetComponent<Movement> ().isDetective = true;
        Detectives[0].GetComponent<Detective>().setArrestKey(KeyCode.U);
        Detectives.Add(Instantiate(detectivePrefab, startLoc[numNPCs+3], Quaternion.identity) as GameObject);
        Detectives[1].transform.GetChild(0).GetComponent<Renderer>().material = disguises[UnityEngine.Random.Range(0, disguises.Count)];
        Detectives[1].GetComponent<Movement>().setUDLRKeys(KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow);
        Detectives[1].GetComponent<Movement>().setBoostKey(KeyCode.M, KeyCode.N);
		Detectives[1].GetComponent<Movement> ().isDetective = true;
        Detectives[1].GetComponent<Detective>().setArrestKey(KeyCode.RightShift);

        //Set targets
        for(int i = 0; i < 4; i++)
        {
            int newIndex = UnityEngine.Random.Range(0, 8);
            while (Array.Exists(targetIndices, element => element == newIndex)) {
                newIndex = UnityEngine.Random.Range(0, 8);
            }
            targetIndices[i] = newIndex;
            NPCs[i].GetComponent<NPC>().target = true;
        }

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
		Switches [4].GetComponent<Switch> ().switchNum = 5;
		//Knight axe Switch
		Switches.Add (Instantiate(switchPrefab, new Vector3(-4.83f, 1.2f, 0), Quaternion.identity) as GameObject);
		Switches [5].GetComponent<Switch> ().switchNum = 6;
		//Piano bottom Switch
		Switches.Add (Instantiate(pianoBottomPrefab, new Vector3(0.08f, 3.62f, 0), Quaternion.Euler (0,0,20)) as GameObject);
		Switches [6].GetComponent<Switch> ().switchNum = 7;

        starttime = Time.time;
        TotalGame.S.round++;
    }

    void Update()
    {
        //if (checkForMurdererWin())
        //{
        //    GameObject murdererText = GameObject.Find("MurdererText");
        //    murdererText.GetComponent<Text>().text = "You Win!";
        //}
        //if ()
        //{
        //    GameObject detectiveText = GameObject.Find("DetectiveText");
        //    detectiveText.GetComponent<Text>().text = "You Win!";
        //}
        foreach(Text t in timerTexts)
        {
            t.text = (120 - (int)(Time.time - starttime)).ToString();
        }
        if(Time.time > 120 + starttime || checkForDetectiveWin())
        {
            Application.LoadLevel("RoundEnd");
        }
    }

    List<Vector3> generateStartLoc()
    {
        List<Vector3> sL = new List<Vector3>();
        int numPerFloor = (int)Math.Ceiling((float)((numNPCs + 4) / numFloors));
        float z = -0.2f;
        for (int i = 0; i < numFloors; i++)
        {
            float y = (2.5f * i) - 4;
            for (int j = 0; j < numPerFloor; j++)
            {
                float x = (float)((14f / numPerFloor) * (j + 0.5f)) - 7;
                sL.Add(new Vector3(x, y, z));
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
