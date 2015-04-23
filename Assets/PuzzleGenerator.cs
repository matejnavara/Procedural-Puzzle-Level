using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleGenerator : MonoBehaviour {

	Puzzle01 puzzle1;
	PuzzleShard shards;

	int lockedRooms;
	int puzzleRooms;

	private int shardsSpawned = 0;
	List<int> freeRooms = new List<int> ();

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

		lockedRooms = 0;
		puzzleRooms = 0;

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

		//Every deadend is a locked room
		foreach (int connection in oneConnect) {
			//Dont choose starting room
			if(connection != 1){
				rooms[connection].setLocked(true);
				lockedRooms++;
				Debug.Log("ROOM " + rooms[connection].getID().ToString() + " IS LOCKED: 1 door");
			}

				}

		//Approx 50% chance of locked room with 2 entrances
		foreach (int connection in twoConnect) {
			float rand = Random.Range(1f,10f);

			if(rand > 5 && connection != 1){
				rooms[connection].setLocked(true);
				Debug.Log("ROOM " + rooms[connection].getID().ToString() + " IS LOCKED: 2 doors");
			}

				}


	}

	public void ChoosePuzzle(Room[] rooms){

		foreach (Room element in rooms) {

			if(element.getLocked()){

				int lockedNum = element.getID();
				int temp = Random.Range(0,element.getConnectionNum());
				int[] connections = element.getConnections().ToArray();
				int roomNum = connections[temp];

				if(!rooms[roomNum].getLocked() && rooms[roomNum].getPuzzle() == 0){
					rooms[roomNum].setPuzzle(lockedNum);
					rooms[lockedNum].assignLock();
					Debug.Log("ROOM " + roomNum.ToString() + " becomes puzzle room to unlock ROOM " + lockedNum.ToString());

				}


			} else if(!element.getLocked() && element.getID() != 0){
				freeRooms.Add(element.getID());
			}
		}


		}

	public void GeneratePuzzles(Room[] rooms){
		foreach (Room element in rooms) {
						if (element.getPuzzle() > 0) {

				puzzle1 = new Puzzle01(element.getArea(),element.getPuzzle());

						} 

						if(element.getLocked()){

				PuzzleShard shard = new PuzzleShard(element.getArea());
				shardsSpawned++;

			}


				}

		Debug.Log("GENERATED: " + shardsSpawned + " shards.");

		}

	public void LockDoors(Room[] rooms){



				foreach (Room element in rooms) {
						 if (element.getLocked ()) {
								Vector2[] doors = element.getDoorways ().ToArray ();
								Color keyColor = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
								foreach (Vector2 pos in doors) {
										
										GameObject lockedDoor = (GameObject)Instantiate (Resources.Load ("LockedDoor"), new Vector3 (pos.x, 0, pos.y), Quaternion.identity);
										lockedDoor.name = "LockedDoor"+element.getID();
										lockedDoor.GetComponentInChildren<Renderer>().material.color = keyColor;
									

								}
								//Creating a coloured locked door with matching key
								if(!element.assigned()){
									int tempRand = Random.Range(0,freeRooms.Count);
									int keyChoice = freeRooms[tempRand];
									PuzzleKey key1 = new PuzzleKey(rooms[keyChoice].getArea(),element.getID(),keyColor);
									
									Debug.Log("Created key for room " + element.getID() + " within room " + keyChoice);
									
								}

						}

				}
		}

	public int getShards(){
		return shardsSpawned;
		}

	public void removeShard(){
		shardsSpawned--;
		Debug.Log("Only " + shardsSpawned + " shards left to collect.");
	}







}