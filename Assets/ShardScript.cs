using UnityEngine;
using System.Collections;

public class ShardScript : MonoBehaviour {

	string shardId;

	AudioClip sound;
	bool collected;
	
	
	// Use this for initialization
	void Start () {
		shardId = this.gameObject.name;

		sound = GetComponent<AudioSource> ().clip;
		collected = false;
		
	}
	
	void OnTriggerEnter(Collider col){
		
		
		if (col.gameObject.tag == "Player" && !collected) {
			AudioSource.PlayClipAtPoint(sound,this.gameObject.transform.position);
			col.GetComponent<Player>().collectShard(); 
			Debug.Log("PICKED UP " + shardId);
			collected = true;
			
			Destroy(this.gameObject);
		}
		
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
