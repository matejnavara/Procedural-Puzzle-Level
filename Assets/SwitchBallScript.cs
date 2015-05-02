using UnityEngine;
using System.Collections;

public class SwitchBallScript : MonoBehaviour {

	bool isSwitched;
	string switchId;
	string doorId;
	GameObject[] doors;
	AudioSource sound;
	
	// Use this for initialization
	void Start () {	
		switchId = this.gameObject.name;
		char[] trimId = {'S','w','i','t','c','h','B','a','l','l'};
		string id = name.Trim (trimId);
		doorId = "LockedDoor" + id;
		sound = GetComponent<AudioSource> ();
		
	}
	
	public void Activate(){
		
		
		doors = GameObject.FindGameObjectsWithTag ("Door");
		
		foreach (GameObject element in doors) {
			
			DoorScript door = element.GetComponent<DoorScript>();
			door.Change (doorId);
			
		}
		
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Ball" && !isSwitched) {

				this.gameObject.GetComponent<Renderer> ().material.color = new Color (0, 1, 0);
				col.gameObject.GetComponent<Rigidbody>().drag = 5;
				Debug.Log ("BALL PUZZLE UNLOCKED: " + doorId);
				Activate();
				isSwitched = true;

			sound.Play ();	
			}
	}

	void OnTriggerExit(Collider col){

		if(col.gameObject.tag == "Ball"  && isSwitched){
			this.gameObject.GetComponent<Renderer> ().material.color = new Color (1, 0, 0);
			col.gameObject.GetComponent<Rigidbody>().drag = 1;
			Debug.Log ("BALL PUZZLE LOCKED: " + doorId);
			Activate();
			isSwitched = false;

		}

	}
	

	

}
