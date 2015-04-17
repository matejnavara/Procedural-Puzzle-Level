using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Puzzle01 : MonoBehaviour{

	int[,] map;
	int unlockID;

	public GameObject[] lockedDoors;
	public GameObject switch01;

	private bool isSwitched;

	public Puzzle01(int[,] _map, int _unlockID){

		map = _map;
		unlockID = _unlockID;
		isSwitched = false;
		bool spawned = false;

		List<Vector2> choice = new List<Vector2>();

		int sizeX = _map.GetUpperBound (0) + 1;
		int sizeY = _map.GetUpperBound (1) + 1;

		for (int i = 0; i < sizeX; i++) {
						for (int j = 0; j < sizeY; j++) {
				if (_map [i, j] == 1 && _map [i+2, j] != 0 &&_map [i, j+2] != 0) {

					choice.Add(new Vector2(i,j));

								}
						}
				}

		int rand = Random.Range(0,choice.Count);
		Vector2 select = choice[rand];

		if(!spawned){
			switch01 = (GameObject)Instantiate (Resources.Load ("Switch"), new Vector3 (select.x, 0, select.y), Quaternion.identity);
			switch01.name = "Switch"+_unlockID.ToString();
			switch01.GetComponent<Renderer>().material.color = new Color (1,0,0);
			
			spawned = true;
		}
	}

	void Awake(){

		}

	void onSwitch(){
		if (!isSwitched) {
			switch01.GetComponent<Renderer>().material.color = new Color (0,1,0);
			isSwitched = true;


				}

		}



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	
	}
}
