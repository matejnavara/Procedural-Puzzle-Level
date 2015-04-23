using UnityEngine;
using System.Collections;

public class KeyScript : MonoBehaviour {

	string keyId;
	Color keyColor;
	string doorId;
	AudioClip sound;
	bool collected;


	// Use this for initialization
	void Start () {
		keyId = this.gameObject.name;
		char[] trimId = {'K','e','y'};
		string id = name.Trim (trimId);

		Debug.Log ("KEY ID: " + keyId);
		doorId = "LockedDoor" + id;
		sound = GetComponent<AudioSource> ().clip;
		collected = false;

	}

	void OnTriggerEnter(Collider col){


		if (col.gameObject.tag == "Player" && !collected) {
			AudioSource.PlayClipAtPoint(sound,this.gameObject.transform.position);
			col.GetComponent<Player>().setKey(doorId); 
			Debug.Log("PICKED UP " + keyId);
			collected = true;

			col.GetComponent<PlayerHUD>().addKey(keyColor);

			Destroy(this.gameObject);
		}

	}

	public void setColor(Color c){
		keyColor = c;
		}

	
	// Update is called once per frame
	void Update () {

	
	}
}
