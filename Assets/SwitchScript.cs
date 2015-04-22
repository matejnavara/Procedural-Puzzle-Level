using UnityEngine;
using System.Collections;

public class SwitchScript : MonoBehaviour {

	bool isSwitched;
	string switchId;
	string doorId;
	GameObject[] doors;
	AudioSource sound;

	// Use this for initialization
	void Start () {	
		switchId = this.gameObject.name;
		char[] trimId = {'S','w','i','t','c','h'};
		string id = name.Trim (trimId);
		doorId = "LockedDoor" + id;
		//Debug.Log("Door Id: " + doorId);
		sound = GetComponent<AudioSource> ();

	}

	public void Activate(){


	doors = GameObject.FindGameObjectsWithTag ("Door");

		foreach (GameObject element in doors) {

			DoorScript door = element.GetComponent<DoorScript>();
			door.Change (doorId);

				}

		}

	void OnMouseDown()
	{
			 if (isSwitched) {
						this.gameObject.GetComponent<Renderer> ().material.color = new Color (1, 0, 0);
						Debug.Log ("I PRESSED A SWITCH LOCKING: " + doorId);
						Activate ();
						isSwitched = false;
	
				} else if (!isSwitched) {
				this.gameObject.GetComponent<Renderer>().material.color = new Color (0,1,0);
				Debug.Log("I PRESSED A SWITCH UNLOCKING: " + doorId);
				Activate();
				isSwitched = true;
				
			} 

		sound.Play ();
		}
		
		
		
	

}
