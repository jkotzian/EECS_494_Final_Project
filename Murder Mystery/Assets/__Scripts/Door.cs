using UnityEngine;
using System.Collections;
using InControl;

public class Door : MonoBehaviour {

    public Door above;
    public Door below;
	public float reappear;

    public GameObject detectiveGlowObj;
    public GameObject possessedNPCGlowObj;
    private Component detectiveGlow;
    private Component possessedNPCGlow;

    public void Awake()
    {
        detectiveGlow = detectiveGlowObj.GetComponent("Halo");
        possessedNPCGlow = possessedNPCGlowObj.GetComponent("Halo");
    }

    public void MoveUp(GameObject passenger)
    {
        Vector3 dest = new Vector3(above.transform.position.x, above.transform.position.y, -0.2f);
        passenger.transform.position = dest;
		passenger.SetActive (false);
		StartCoroutine (EnableSprite(passenger));
        DeactivateDetectiveGlow(true);
        DeactivateNPCGlow(true);
    }

    public void MoveDown(GameObject passenger)
    {
        Vector3 dest = new Vector3(below.transform.position.x, below.transform.position.y, -0.2f);
        passenger.transform.position = dest;
		passenger.SetActive (false);
		StartCoroutine (EnableSprite(passenger));
        DeactivateDetectiveGlow(true);
        DeactivateNPCGlow(true);
    }

    public void ActivateDetectiveGlow(bool source = false)
    {
        detectiveGlow.GetType().GetProperty("enabled").SetValue(detectiveGlow, true, null);
        if (source)
        {
            if (above)
            {
                above.ActivateDetectiveGlow();
            }
            if (below)
            {
                below.ActivateDetectiveGlow();
            }
        }
    }

    public void DeactivateDetectiveGlow(bool source = false)
    {
        detectiveGlow.GetType().GetProperty("enabled").SetValue(detectiveGlow, false, null);
        if (source)
        {
            if (above)
            {
                above.DeactivateDetectiveGlow();
            }
            if (below)
            {
                below.DeactivateDetectiveGlow();
            }
        }
    }

    public void ActivateNPCGlow(bool source = false)
    {
        possessedNPCGlow.GetType().GetProperty("enabled").SetValue(possessedNPCGlow, true, null);
        if (source)
        {
            if (above)
            {
                above.ActivateNPCGlow();
            }
            if (below)
            {
                below.ActivateNPCGlow();
            }
        }
    }

    public void DeactivateNPCGlow(bool source = false)
    {
        possessedNPCGlow.GetType().GetProperty("enabled").SetValue(possessedNPCGlow, false, null);
        if (source)
        {
            if (above)
            {
                above.DeactivateNPCGlow();
            }
            if (below)
            {
                below.DeactivateNPCGlow();
            }
        }
    }

    IEnumerator EnableSprite(GameObject passenger){
		yield return new WaitForSeconds (.5f);
//		if (passenger.GetComponent<SpriteRenderer> ().enabled == false) {
//			passenger.GetComponent<SpriteRenderer> ().enabled = true;
//		}
		passenger.SetActive (true);
	}

    void OnTriggerStay(Collider other){
        Detective detective = other.GetComponent<Detective>();
        NPC npc = other.GetComponent<NPC>();
        if (detective)
        {
            // Activate glow
            ActivateDetectiveGlow(true);
        }
        if (npc && npc.possessed)
        {
            // Activate glow
            ActivateNPCGlow(true);
        }                                           
        if (detective || (npc && npc.possessed)) {

			//Elevator Kill is glitchy (fix this)
//			if (other.GetComponent<Movement> ().isMurderer == true && Input.GetKeyDown (KeyCode.E)){
//				pickKeyCode = Random.Range (1, 10);
//				print (pickKeyCode);
//				if (pickKeyCode >= 1 && pickKeyCode <= 5 && gameObject.name != "TopDoor"){
//					print ("Going up 1");
//					Vector3 temp = other.transform.position;
//					temp.y += 1.25f;
//					other.transform.position = temp;
//				}
//				if (pickKeyCode >= 6 && pickKeyCode <= 10 && gameObject.name != "BottomDoor"){
//					print ("Going down 1");
//					Vector3 temp = other.transform.position;
//					temp.y -= 1.25f;
//					other.transform.position = temp;
//				}
//				other.GetComponent<Murderer>().Kill();
//			}
		}
	}

    void OnTriggerExit(Collider other)
    {
        Detective detective = other.GetComponent<Detective>();
        NPC npc = other.GetComponent<NPC>();       
        if (detective)
        {
            // Activate glow
            DeactivateDetectiveGlow(true);
        }
        if (npc && npc.possessed)
        {
            // Activate glow
            DeactivateNPCGlow(true);
        }
    }
}
