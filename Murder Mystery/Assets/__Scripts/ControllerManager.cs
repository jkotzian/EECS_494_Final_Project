using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class ControllerManager : MonoBehaviour {
    public static ControllerManager S;
	
	public List<InputDevice> allControllers = new List<InputDevice>();
	public List<int> setPlayers = new List<int>();
	public List<int> buttonsPressed = new List<int> ();

	public int detectiveOne;
	public int detectiveTwo;
	public int ghostOne;
	public int ghostTwo;

	void Start () {
		S = this;
		print ("Number of Devices: " + InputManager.Devices.Count);
		for(int i = 0; i < InputManager.Devices.Count; i++){
			allControllers.Add(InputManager.Devices[i]);
			//print("Name of controller: " + allControllers[i].Name);
		}
	}

	void Update () {

		for(int i = 0; i < allControllers.Count; i++){
			if(setPlayers.Contains(i)){
				continue;
			}

			if(allControllers[i].Action1 && !buttonsPressed.Contains(1)){//X which is detective 1
				print ("X was pressed by controller number: " + i);
				setPlayers.Add(i);
				buttonsPressed.Add(1);
				detectiveOne = i;
				//GamePlay.S.Detectives[0].GetComponent<Movement>().inputDevice = allControllers[i];
			}

//			if(allControllers[i].DPadRight && !buttonsPressed.Contains(i){//X which is detective 1
//				print ("Right was pressed by controller " + i);
//				//setPlayers.Add(i);
//				//detectiveOne = i;
//				//GamePlay.S.Detectives[0].GetComponent<Movement>().inputDevice = allControllers[i];
//			}

			if(allControllers[i].Action2 && !buttonsPressed.Contains(2)){//O which is detective 2
				print ("O pressed by controller number: " + i);
				setPlayers.Add(i);
				buttonsPressed.Add(2);
				detectiveTwo = i;
				//GamePlay.S.Detectives[1].GetComponent<Movement>().inputDevice = allControllers[i];				
			}
			
			if(allControllers[i].Action3 && !buttonsPressed.Contains(3)){//Square which is ghost 1
				print ("Square pressed by controller number: " + i);
				setPlayers.Add(i);
				buttonsPressed.Add(3);
				ghostOne = i;
				//GamePlay.S.Ghosts[0].GetComponent<Movement>().inputDevice = allControllers[i];
			}

			if(allControllers[i].Action4 && !buttonsPressed.Contains(4)){//Triangle which is ghost 2
				print ("Triangle pressed by controller number: " + i);
				setPlayers.Add(i);
				buttonsPressed.Add(4);
				ghostTwo = i;
				//GamePlay.S.Ghosts[1].GetComponent<Movement>().inputDevice = allControllers[i];
			}
		}
	}
}
