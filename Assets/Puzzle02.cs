using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Puzzle02 : MonoBehaviour {

	int[,] map;
	int unlockID;
	
	public GameObject[] lockedDoors;
	public GameObject switchBall;
	public GameObject ball;
	
	private bool isSwitched;
	
	public Puzzle02(int[,] _map, int _unlockID){
		
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
		
		int rand1 = Random.Range(0,choice.Count);
		int rand2 = Random.Range (0, choice.Count);
		Vector2 select1 = choice[rand1];
		Vector2 select2 = choice[rand2];
		
		if(!spawned){
			switchBall = (GameObject)Instantiate (Resources.Load ("SwitchBall"), new Vector3 (select1.x, 0, select1.y), Quaternion.identity);
			switchBall.name = "SwitchBall"+_unlockID.ToString();
			switchBall.GetComponent<Renderer>().material.color = new Color (1,0,0);

			ball = (GameObject)Instantiate(Resources.Load("Ball"), new Vector3(select2.x, 0, select2.y), Quaternion.identity);
			ball.name = "Ball"+_unlockID.ToString();
			
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
