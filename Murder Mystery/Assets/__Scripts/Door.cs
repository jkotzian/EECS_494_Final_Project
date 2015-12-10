using UnityEngine;
using System.Collections;
using InControl;

public class Door : MonoBehaviour {

    public Door above;
    public Door below;
	public float reappear;

    private Component glow;

    public void Awake()
    {
        glow = GetComponent("Halo");  
    }

    public void MoveUp(GameObject passenger)
    {
        Vector3 dest = new Vector3(above.transform.position.x, above.transform.position.y, -0.2f);
        passenger.transform.position = dest;
		passenger.SetActive (false);
		StartCoroutine (EnableSprite(passenger));
        DeactivateGlow(true);		
    }

    public void MoveDown(GameObject passenger)
    {
        Vector3 dest = new Vector3(below.transform.position.x, below.transform.position.y, -0.2f);
        passenger.transform.position = dest;
		passenger.SetActive (false);
		StartCoroutine (EnableSprite(passenger));
        DeactivateGlow(true);
    }

    public void ActivateGlow(bool source = false)
    {
        glow.GetType().GetProperty("enabled").SetValue(glow, true, null);
        if (source)
        {
            if (above)
            {
                above.ActivateGlow();
            }
            if (below)
            {
                below.ActivateGlow();
            }
        }
    }

    public void DeactivateGlow(bool source = false)
    {
        glow.GetType().GetProperty("enabled").SetValue(glow, false, null);
        if (source)
        {
            if (above)
            {
                above.DeactivateGlow();
            }
            if (below)
            {
                below.DeactivateGlow();
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
        //sending the characters up and down a level 
        if (detective || (npc && npc.possessed)) {
            // Activate glow
            ActivateGlow(true);

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
        if (detective || (npc && npc.possessed))
        {
            // Activate glow
            DeactivateGlow(true);
        }
    }
}
