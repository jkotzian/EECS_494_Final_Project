﻿using UnityEngine;
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

    public int numNPCs = 8;
    public int numFloors = 4;
    public List<GameObject> NPCs;
    public List<GameObject> Ghosts;
    public List<GameObject> Detectives;

    private List<Vector3> startLoc;

    private int[] targetIndices;

    void Awake()
    {
        S = this;
        NPCs = new List<GameObject>();
        Ghosts = new List<GameObject>();
        Detectives = new List<GameObject>();
        targetIndices = Enumerable.Repeat(-1, 4).ToArray();
        startLoc = generateStartLoc();
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
        for(int i = 0; i < numNPCs; i++)
        {
            NPCs.Add(Instantiate(npcPrefab, startLoc[i], Quaternion.identity) as GameObject);
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
        Detectives[0].GetComponent<Movement>().setUDLRKeys(KeyCode.I, KeyCode.K, KeyCode.J, KeyCode.L);
        Detectives[0].GetComponent<Movement>().setBoostKey(KeyCode.H, KeyCode.O);
		Detectives[0].GetComponent<Movement> ().isDetective = true;
        Detectives[0].GetComponent<Detective>().setArrestKey(KeyCode.U);
        Detectives.Add(Instantiate(detectivePrefab, startLoc[numNPCs+3], Quaternion.identity) as GameObject);
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
        //if (checkForMurdererWin())
        //{
        //    GameObject murdererText = GameObject.Find("MurdererText");
        //    murdererText.GetComponent<Text>().text = "You Win!";
        //}
        //if (checkForDetectiveWin())
        //{
        //    GameObject detectiveText = GameObject.Find("DetectiveText");
        //    detectiveText.GetComponent<Text>().text = "You Win!";
        //}
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

    //bool checkForDetectiveWin()
    //{
    //    foreach (GameObject m in Ghosts)
    //    {
    //        if (m.GetComponent<Murderer>().alive)
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}

}
