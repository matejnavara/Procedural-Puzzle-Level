using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour{

	public static GameObject player;
	private Vector3 pos;
	private List<string> keys;
	
	// Use this for initialization
	void Start () {
		player.name = "Player";
		player.tag = "Player";
		keys = new List<string> ();
	}
	
	// Update is called once per frame
	void Update () {

	
	}

	
	public void spawnPlayer(int[,] _map, bool perspective, bool skyBool){
			
		
		int sizeX = _map.GetUpperBound (0);
		int sizeZ = _map.GetUpperBound (1);
		
		int skyChoice = (int)(Random.Range(1,8));
		string sky = "Sky/Sky"+skyChoice.ToString();
		Debug.Log ("Chosen sky " + skyChoice);
		
		bool playerSpawned = false;
		string playerView = "";
		
		if(!perspective){
			playerView = "FP_Player";
		} else if(perspective){
			playerView = "Player";
		}
		
		for (int i = 0; i < sizeX; i++) {
			for (int j = 0; j < sizeZ; j++) {
				if(_map[i,j] == 1 && !playerSpawned){
					player = (GameObject) Instantiate(Resources.Load("FP_Player"),new Vector3(i+5,5,j+5),Quaternion.identity);
					Skybox skybox = (Skybox)player.GetComponentInChildren(typeof(Skybox));
					skybox.material = (Material)Resources.Load(sky);
					if(skyBool)
						skybox.material.SetColor("_Tint",new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f)));
					else if(!skyBool)
						skybox.material.SetColor("_Tint",new Color(0.5f,0.5f,0.5f));
					playerSpawned = true;
				}
			}
		}
		
	}

	public Vector3 getPos(){

		pos = this.gameObject.transform.position;
		return pos;
	}

	public void setKey(string i){
		keys.Add (i);
	}

	public bool hasKey(string Id){
		if (keys.Contains (Id)) {
						Debug.Log ("Player has Key to " + Id);
						return true;
				} else {
						Debug.Log ("Player does not have Key to " + Id);
						return false;
				}

	}

}
