using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {

	GameObject door;
	Animator animator;
	AudioSource sound;
	bool doorOpen;

	string doorId;

	void Start(){
		door = this.gameObject;
		doorOpen = false;
		animator = GetComponent<Animator> ();
		sound = GetComponent<AudioSource> ();
		doorId = door.name;


	}

	void OnTriggerEnter(Collider col){
		if (!doorOpen) {
						if (col.gameObject.tag == "Player" && col.gameObject.GetComponent<Player> ().hasKey (doorId)) {
								doorOpen = true;
								DoorControl ("Open");
								Debug.Log ("DOOR OPENED WITH KEY");
								sound.Play ();
						}
				}
		}
	


	void DoorControl(string instruction){
		animator.SetTrigger (instruction);
		}

	public void Change(string Id){
		if (doorOpen && doorId == Id) {
			DoorControl("Close");
			Debug.Log("DOOR CLOSING");
			doorOpen = false;
				} else if (!doorOpen && doorId == Id) {
			DoorControl("Open");
			Debug.Log("DOOR OPENING");
			doorOpen = true;
				}
		}
}
