using UnityEngine;
using System.Collections;

public class KeyScript : MonoBehaviour {

	string keyId;
	string doorId;
	AudioSource sound;
	bool collected;
	bool fin;

	// Use this for initialization
	void Start () {
		keyId = this.gameObject.name;
		char[] trimId = {'K','e','y'};
		string id = name.Trim (trimId);

		Debug.Log ("KEY ID: " + keyId);
		doorId = "LockedDoor" + id;
		sound = GetComponent<AudioSource> ();
		collected = false;
		fin = false;
	}

	void OnTriggerEnter(Collider col){


		if (col.gameObject.tag == "Player" && !collected) {
			sound.Play();
			col.GetComponent<Player>().setKey(doorId); 
			Debug.Log("PICKED UP " + keyId);
			collected = true;
			fin = true;

		}
	}

	
	// Update is called once per frame
	void Update () {
		if (fin) {
			DestroyObject(this.gameObject);
				}
	
	}
}
