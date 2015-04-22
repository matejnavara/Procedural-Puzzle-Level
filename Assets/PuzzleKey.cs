using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleKey : MonoBehaviour {

	int[,] map;
	int unlockID;
	
	public GameObject[] lockedDoors;
	public GameObject key;
	
	public PuzzleKey(int[,] _map, int _unlockID, Color _keyColor){
		
		map = _map;
		unlockID = _unlockID;
		bool spawned = false;
		
		List<Vector2> choice = new List<Vector2>();
		
		int sizeX = _map.GetUpperBound (0) + 1;
		int sizeY = _map.GetUpperBound (1) + 1;
		
		for (int i = 0; i < sizeX-2; i++) {
			for (int j = 0; j < sizeY-2; j++) {
				if (_map [i, j] == 1 && _map [i+2, j] != 0 && _map [i, j+2] != 0) {

					choice.Add(new Vector2(i,j));
					
				}
			}
		}
		
		int rand = Random.Range(0,choice.Count);
		Vector2 select = choice[rand];
		
		if(!spawned){
			key = (GameObject)Instantiate (Resources.Load ("Key"), new Vector3 (select.x, 0, select.y), Quaternion.identity);
			key.name = "Key"+_unlockID.ToString();
			key.GetComponent<Renderer>().material.color = _keyColor;
			
			spawned = true;
		}
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
