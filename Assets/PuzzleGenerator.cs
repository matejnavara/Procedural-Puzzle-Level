using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleGenerator : MonoBehaviour {

	Puzzle01 puzzle1;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChooseLocked(Room[] rooms){

		List<int> oneConnect = new List<int> ();
		List<int> twoConnect = new List<int> ();
		List<int> threeConnect = new List<int> ();

		int lockedRooms;
		int puzzleRooms;

		foreach (Room element in rooms) {
			int connections = element.getConnectionNum();

			if(connections == 1){
				oneConnect.Add(element.getID());
			} else if(connections == 2){
				twoConnect.Add(element.getID());
			} else if(connections == 3){
				threeConnect.Add(element.getID());
				}

		}

		foreach (int connection in oneConnect) {
			if(connection != 1){
				rooms[connection].setLocked(true);
				Debug.Log("ROOM " + rooms[connection].getID().ToString() + " IS LOCKED");
			}

				}


	}

	public void ChoosePuzzle(Room[] rooms){

		List<int> locked;


		foreach (Room element in rooms) {
			if(element.getLocked()){

				int lockedNum = element.getID();
				int temp = Random.Range(0,element.getConnectionNum());
				int[] connections = element.getConnections().ToArray();
				int roomNum = connections[temp];

				if(!rooms[roomNum].getLocked()){
					rooms[roomNum].setPuzzle(lockedNum);
					Debug.Log("ROOM " + roomNum.ToString() + " becomes puzzle room to unlock ROOM " + lockedNum.ToString());

				}


			}
				}
		}

	public void GeneratePuzzles(Room[] rooms){
		foreach (Room element in rooms) {
						if (element.getPuzzle() > 0) {

				puzzle1 = new Puzzle01(element.getArea(),element.getPuzzle());

						}
				}

		}

	public void LockDoors(Room[] rooms){
		Dictionary<GameObject,int> d = new Dictionary<GameObject, int>();
				foreach (Room element in rooms) {
						if (element.getLocked ()) {
								Vector2[] doors = element.getDoorways ().ToArray ();
								foreach (Vector2 pos in doors) {
										
										GameObject lockedDoor = (GameObject)Instantiate (Resources.Load ("LockedDoor"), new Vector3 (pos.x, 0, pos.y), Quaternion.identity);
										lockedDoor.name = "LockedDoor"+element.getID();
								}

						}

				}
		}

}