using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHUD : MonoBehaviour {

	int playerShards;
	int shardTotal;
	int keyTotal;

	private bool isGameOver = false;
	GameObject player;
	GameObject[] shards;


	GameObject canvas;
	GameObject panel;
	GameObject shardText;

	GameObject ringImg;
	Vector3 ringPos;

	GameObject keyImg;
	Vector3 keyPos;



	// Use this for initialization
	void Start () {
		player = this.gameObject;
		canvas = GameObject.Find ("Canvas");

		panel = GameObject.Find ("GameOverPanel");


		playerShards = this.gameObject.GetComponent<Player> ().hasShards ();
		shards = GameObject.FindGameObjectsWithTag ("Shard");
		shardTotal = shards.Length;
		shardText = GameObject.Find ("ShardsCollected");
		shardText.GetComponent<Text> ().text = playerShards + " / " + shardTotal;

		ringImg = GameObject.Find ("RingImg");
		ringPos = new Vector3 (ringImg.transform.position.x,
		                      ringImg.transform.position.y,
		                      ringImg.transform.position.z);
		panel.SetActive (false);

		keyImg = GameObject.Find ("KeyImg");
		keyPos = new Vector3 (keyImg.transform.position.x,
		                      keyImg.transform.position.y,
		                      keyImg.transform.position.z);
		keyTotal = 0;

	}
	
	// Update is called once per frame
	void Update () {
		if (isGameOver && Input.GetKeyUp(KeyCode.R)) {
			Application.LoadLevel(Application.loadedLevel);
			Debug.Log("RESTARTING GAME");
			}		    
				}





	public void addShard(){
		playerShards++;
		shardText.GetComponent<Text> ().text = playerShards + " / " + shardTotal;
		
		if (playerShards == shardTotal && !isGameOver) {
			panel.SetActive(true);
			for(int i = 0; i < shardTotal; i++){
				GameObject newRingImg = (GameObject) Instantiate(ringImg, new Vector3(ringPos.x + (150*i), ringPos.y, ringPos.z), Quaternion.identity);
				newRingImg.transform.parent = GameObject.Find("GameOverPanel").transform;
				isGameOver = true;
			}
		}
	}

	public void addKey(Color c){
		keyTotal++;
		GameObject newKeyImg = (GameObject) Instantiate (keyImg, new Vector3 (keyPos.x, keyPos.y + (100*keyTotal), keyPos.z), Quaternion.identity);
		newKeyImg.GetComponent<Image> ().color = c;
		newKeyImg.transform.parent = canvas.transform;

	}

}
