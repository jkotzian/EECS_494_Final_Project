using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using UnityEngine.UI;

public class GamePlay : MonoBehaviour {
    public static GamePlay S;

	public GameObject		switchPrefab;
    public GameObject       npcPrefab;
    public GameObject       murdererPrefab;
    public GameObject       detectivePrefab;

    public GameObject[] NPCs;
    public GameObject[] Murderers;
    public GameObject[] Detectives;

    private int[] targetIndices;

    void Awake()
    {
        S = this;
        NPCs = new GameObject[18];
        Murderers = new GameObject[2];
        Detectives = new GameObject[2];
        targetIndices = Enumerable.Repeat(-1, 4).ToArray();
    }
    
    void Start () {
        // Place switches
		/*Instantiate(switchPrefab, new Vector3(-4.78f, -3.54f, -0.2f), Quaternion.identity);
		Instantiate(switchPrefab, new Vector3(-7.46f, -1.24f, -0.2f), Quaternion.identity);
		Instantiate(switchPrefab, new Vector3(-1.76f, 1.3f, -0.2f), Quaternion.identity);
		Instantiate(switchPrefab, new Vector3(-4.68f, 3.74f, -0.2f), Quaternion.identity);
		Instantiate(switchPrefab, new Vector3(4.38f, 3.74f, -0.2f), Quaternion.identity);
		Instantiate(switchPrefab, new Vector3(7.09f, 1.3f, -0.2f), Quaternion.identity);
		Instantiate(switchPrefab, new Vector3(1.43f, -1.24f, -0.2f), Quaternion.identity);
		Instantiate(switchPrefab, new Vector3(4.25f, -3.75f, -0.2f), Quaternion.identity);*/
        // Place NPCs
        NPCs[0] = Instantiate(npcPrefab, new Vector3(-4.5f, 3.5f, -0.2f), Quaternion.identity) as GameObject;
        NPCs[1] = Instantiate(npcPrefab, new Vector3(4.5f, 3.5f, -0.2f), Quaternion.identity) as GameObject;
        NPCs[2] = Instantiate(npcPrefab, new Vector3(-4.5f, 1f, -0.2f), Quaternion.identity) as GameObject;
        NPCs[3] = Instantiate(npcPrefab, new Vector3(4.5f, 1f, -0.2f), Quaternion.identity) as GameObject;
        NPCs[4] = Instantiate(npcPrefab, new Vector3(-4.5f, -1.5f, -0.2f), Quaternion.identity) as GameObject;
        NPCs[5] = Instantiate(npcPrefab, new Vector3(4.5f, -1.5f, -0.2f), Quaternion.identity) as GameObject;
        NPCs[6] = Instantiate(npcPrefab, new Vector3(-4.5f, -4f, -0.2f), Quaternion.identity) as GameObject;
        NPCs[7] = Instantiate(npcPrefab, new Vector3(4.5f, -4f, -0.2f), Quaternion.identity) as GameObject;

        // Place Murderers
        Murderers[0] = Instantiate(murdererPrefab, new Vector3(-7f, -1f, -0.2f), Quaternion.identity) as GameObject;
        Murderers[0].GetComponent<Movement>().setUDLRKeys(KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D);
        Murderers[0].GetComponent<Movement>().isMurderer = true;
        Murderers[0].GetComponent<Murderer>().setMurderKey(KeyCode.Q);
        Murderers[1] = Instantiate(murdererPrefab, new Vector3(-7f, -4f, -0.2f), Quaternion.identity) as GameObject;
        Murderers[1].GetComponent<Movement>().setUDLRKeys(KeyCode.T, KeyCode.G, KeyCode.F, KeyCode.H);
        Murderers[1].GetComponent<Movement>().isMurderer = true;
        Murderers[1].GetComponent<Murderer>().setMurderKey(KeyCode.R);
        // Place Detectives
        Detectives[0] = Instantiate(detectivePrefab, new Vector3(7f, -1f, -0.2f), Quaternion.identity) as GameObject;
        Detectives[0].GetComponent<Movement>().setUDLRKeys(KeyCode.I, KeyCode.K, KeyCode.J, KeyCode.L);
        Detectives[0].GetComponent<Movement>().setBoostKey(KeyCode.H, KeyCode.O);
		Detectives[0].GetComponent<Movement> ().isDetective = true;
        Detectives[0].GetComponent<Detective>().setArrestKey(KeyCode.U);
        Detectives[1] = Instantiate(detectivePrefab, new Vector3(7f, -4f, -0.2f), Quaternion.identity) as GameObject;
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
    }

    void Update()
    {
        if (checkForWin())
        {
            GameObject murdererText = GameObject.Find("MurdererText");
            murdererText.GetComponent<Text>().text = "You Win!";
        }
    }

    bool checkForWin()
    {
        foreach(int i in targetIndices)
        {
            if (NPCs[i].GetComponent<NPC>().alive)
            {
                return false;
            }
        }
        return true;
    }
	
}
