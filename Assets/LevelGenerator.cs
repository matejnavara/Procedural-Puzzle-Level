using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{

		public int sizeX = 200;
		public int sizeY = 200;
		public int nbrRooms = 18;
		public bool thirdPerson = false;
		public bool funkySky = false;
		public bool sonic = false;

		private static Room[] rooms;
		private PuzzleGenerator puzzleGen;
		private Player player;

		// Use this for initialization
		void Start ()
		{

		int[,] map = new int[sizeX, sizeY];
		int roomCount = 0;

		while (roomCount < 6) {
			map = CreateLevelSquares (sizeX, sizeY, nbrRooms);
			roomCount = FindRooms (map, sizeX, sizeY);
			Debug.Log("ROOMCOUNT: " + roomCount.ToString());
				}

		//Debug.Log("ROOMCOUNT: " + roomCount.ToString());

		rooms = new Room[roomCount+1];
		createRooms (map, roomCount);

		FindNeighbour (map, rooms);
		

		BuildFloor (map, sizeX, sizeY, roomCount);
		BuildWalls (map, sizeX, sizeY, roomCount);

		puzzleGen = new PuzzleGenerator ();
		player = new Player ();

		puzzleGen.ChooseLocked (rooms);
		puzzleGen.ChoosePuzzle (rooms);
		puzzleGen.GeneratePuzzles (rooms);
		puzzleGen.LockDoors (rooms);


		
		
		int[,] start = getRoom (map, 1);
		player.spawnPlayer(start, thirdPerson, funkySky, sonic);

		
		printToConsole (map, sizeX, sizeY);

	
		}
	
		// Update is called once per frame
		void Update ()
		{

		}



		public static int[,] CreateLevelSquares (int _sizeX, int _sizeY, int _nbrRooms)
		{

				//Create empty map
				int[,] _newMap = new int[_sizeX, _sizeY];

				for (int i = 0; i < _nbrRooms; i++) {

						int _roomSizeX = Random.Range (10, _sizeX / 5);
						int _roomSizeY = Random.Range (10, _sizeY / 5);
						int _roomPosX = Random.Range (2, _sizeX - _roomSizeX - 1);
						int _roomPosY = Random.Range (2, _sizeY - _roomSizeY - 1);

						for (int j = _roomPosX; j < _roomPosX + _roomSizeX; j++) {
								for (int k = _roomPosY; k < _roomPosY + _roomSizeY; k++) {
										_newMap [j, k] = 1;									
								}
						}

				}
				
				return _newMap;

		}

		static int FindRooms (int[,] _map, int _sizeX, int _sizeY)
		{

				List<Vector2> TestList = new List<Vector2> ();
				int[,] modifiedMap = new int[_sizeX, _sizeY]; 
				int roomNum = 0;

				System.Array.Copy (_map, modifiedMap, _sizeX * _sizeY);

				for (int i = 0; i < _sizeX; i++) {
						for (int j = 0; j < _sizeY; j++) {
								if (modifiedMap [i, j] == 1) {
										TestList.Add (new Vector2 (i, j));

										while (TestList.Count > 0) {
												int tempX = (int)TestList [0].x;
												int tempY = (int)TestList [0].y;
												TestList.RemoveAt (0);

												_map [tempX, tempY] = roomNum + 1;

												for (int xAround = tempX - 1; xAround <= tempX + 1; xAround++) {
														for (int yAround = tempY - 1; yAround <= tempY + 1; yAround++) {
																if (modifiedMap [xAround, yAround] == 1) {
																		TestList.Add (new Vector2 (xAround, yAround));
																		modifiedMap [xAround, yAround] = 0;
																}
														}
												}


										}
										roomNum++;
								}

						}

				}
				return roomNum;
	
		}


	static int[,] getRoom(int[,] _map, int _roomNum){
		
		int sizeX = _map.GetUpperBound (0)+1;
		int sizeY = _map.GetUpperBound (1)+1;
		
		
		
		int[,] room = new int[sizeX,sizeY];
		
		for (int i = 0; i < sizeX; i++) {
			for (int j = 0; j < sizeY; j++) {
				if(_map[i,j] == _roomNum){
					room[i,j] = 1;
				}
			}
		}
		
		return room;
		
	}

	static void createRooms(int[,] _map, int _roomNum){
		
		for (int i = 0; i <= _roomNum; i++) {
			int[,] tempMap = getRoom(_map,i);
			rooms[i] = new Room(tempMap,i);
			printToConsole(tempMap,tempMap.GetUpperBound(0),tempMap.GetUpperBound(1));
		}
	}
	
	static void FindNeighbour(int[,] _map, Room[] _rooms){

			foreach (Room element in _rooms) {
						if (element != null) {

								int roomID = element.getID ();

								int sizeX = _map.GetUpperBound (0) + 1;
								int sizeY = _map.GetUpperBound (1) + 1;

								int maxX = 0;
								int maxY = 0;
								int minX = sizeX;
								int minY = sizeY;

								for (int i = 0; i < sizeX; i++) {
										for (int j = 0; j < sizeY; j++) {
												if (_map [i, j] == roomID) {
														if (i > maxX)
																maxX = i;
														if (i < minX)
																minX = i;
														if (j > maxY)
																maxY = j;
														if (j < minY)
																minY = j;
												}
										}
								}

								//string stats = " For Room " + roomID.ToString () + "Max X: " + maxX.ToString () + " Min X: " + minX.ToString () + " Max Y: " + maxY.ToString () + " Min Y: " + minY.ToString ();
								//Debug.Log (stats);

								//Check right
								bool foundRight = false;

								int rightMax = 0;
								int rightMin = sizeY;
								int rightRoom = 0;
								int rightX = 0;

								for (int i = maxX; i < sizeX; i++) {
										if (!foundRight) {
												for (int j = minY; j < maxY; j++) {
														if (_map [i, j] != roomID && _map [i, j] != 0) {

																rightRoom = _map [i, j];
																rightX = i;
																foundRight = true;

																if (j > rightMax)
																		rightMax = j;
																if (j < rightMin)
																		rightMin = j;
														}
												}	
										}

								}

								if (foundRight) {
										int rightRandom = Random.Range (rightMin, rightMax - 3);
										connectRight (_map, roomID, rightRoom, rightRandom, rightX);
										//rooms[rightRoom].addConnection(roomID);

										//string foundR = "RIGHT: Found room " + rightRoom.ToString () + "  between Y: " + rightMin.ToString () + " and " + rightMax.ToString () + " at X: " + rightX.ToString ();
										//Debug.Log (foundR);
								}


								//Check down
								bool foundDown = false;
		
								int downMax = 0;
								int downMin = sizeX;
								int downRoom = 0;
								int downY = 0;
		
								for (int j = minY; j > 0; j--) {
										if (!foundDown) {
												for (int i = minX; i < maxX; i++) {
														if (_map [i, j] != roomID && _map [i, j] != 0 && _map [i, j] != -1) {
						
																downRoom = _map [i, j];
																downY = j;
																foundDown = true;
						
																if (i > downMax)
																		downMax = i;
																if (i < downMin)
																		downMin = i;
														}
												}	
										}
			
								}

								if (foundDown) {
										int downRandom = Random.Range (downMin, downMax - 3);
										connectDown (_map, roomID, downRoom, downRandom, downY);

										//string foundD = "DOWN: Found room " + downRoom.ToString () + "  between X: " + downMin.ToString () + " and " + downMax.ToString () + " at Y: " + downY.ToString ();
										//Debug.Log (foundD);
								}

						}
				}

	}

	static void connectRooms(int _roomID, int _targetID, Vector2 _door, Vector2 _targetDoor){
		rooms[_roomID].addConnection(_targetID);
		rooms[_targetID].addConnection(_roomID);

		rooms [_roomID].addDoorway (_door);
		rooms [_targetID].addDoorway (_targetDoor);

	}

	static void connectRight(int[,] _map, int _roomID, int _targetID, int _startY, int _startX){
		int sizeX = _map.GetUpperBound (0)+1;
		int sizeY = _map.GetUpperBound (1)+1;

		bool connected = false;
		Vector2 doorway = new Vector2();
		Vector2 targetDoorway = new Vector2();

		for (int i = _startX; i > 0; i--) {
			if(!connected){
				for (int j = _startY; j < _startY+3; j++) {
					if(_map[i,j] == 0){
						_map[i,j] = -1;
					} else if( _map[i,j] == _roomID){
						targetDoorway = new Vector2(_startX, _startY+1);
						doorway = new Vector2(i,_startY+1);

						connected = true;
					}
				}
			}
		}

						connectRooms (_roomID, _targetID, doorway, targetDoorway);
		
						string connectR = "ROOM : " + _roomID.ToString () + " is connected right to " + _targetID.ToString () + " with door at " + doorway.ToString ();
						Debug.Log (connectR);
				

	}

	static void connectDown(int[,] _map, int _roomID, int _targetID, int _startX, int _startY){
		int sizeX = _map.GetUpperBound (0)+1;
		int sizeY = _map.GetUpperBound (1)+1;
		
		bool connected = false;
		Vector2 doorway = new Vector2();
		Vector2 targetDoorway = new Vector2();

		for (int j = _startY; j < sizeY; j++) {
			if(!connected){
				for (int i = _startX; i < _startX+3; i++) {
					if(_map[i,j] == 0){
						_map[i,j] = -1;
					} else if( _map[i,j] == _roomID){

						targetDoorway = new Vector2(_startX+1, _startY);
						doorway = new Vector2(_startX+1, j);
						connected = true;
					}
					
				}
			}
		}
						connectRooms (_roomID, _targetID, doorway,targetDoorway);

		
						string connectD = "ROOM : " + _roomID.ToString () + " is connected down to " + _targetID.ToString () + " with door at " + doorway.ToString ();
						Debug.Log (connectD);
				
		
	}



	
	static void BuildFloor (int[,] _map, int _sizeX, int _sizeZ, int rooms)
	{


		Mesh mesh = new Mesh();
		mesh.name = "Floor";
		GameObject floor = new GameObject ("Floor");
		mesh.vertices = new Vector3[] {new Vector3(0,0,0),new Vector3 (0, 0, _sizeZ),new Vector3 (_sizeX, 0, _sizeZ),new Vector3 (_sizeX, 0, 0)};
		mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0)};
		mesh.triangles = new int[] {0, 1, 2, 0 , 2 , 3};
		mesh.RecalculateNormals ();


		MeshFilter meshFilter = (MeshFilter)floor.AddComponent(typeof(MeshFilter));
		meshFilter.mesh = mesh;

		MeshRenderer renderer = (MeshRenderer)floor.AddComponent(typeof(MeshRenderer));
		//renderer.material.color = new Color (1,1,1);
		Material floorMat = (Material)Resources.Load ("Materials/Materials/BlackBrick");
	
		renderer.material = floorMat;


		MeshCollider collider = (MeshCollider)floor.AddComponent(typeof(MeshCollider));
	
		}

	static void BuildWalls(int[,] _map, int _sizeX, int _sizeY, int rooms){

		Vector3[] colors = new Vector3[rooms+3];

		for(int k = 0 ; k < rooms + 2; k++){
			colors[k] = new Vector3(Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));

		}
	
		int wallColor = 0;

		for (int i = 1; i < _sizeX - 1; i++) {
			for (int j = 1; j < _sizeY - 1; j++) {

				bool built = false;

				if(_map[i,j] == 0 && (_map[i,j+1] != 0 || _map[i,j-1] != 0)){

					if(_map[i,j+1] > _map[i,j-1])
						wallColor = _map[i,j+1] + 2;
					else if(_map[i,j-1] > _map[i,j+1])
						wallColor = _map[i,j-1] + 2;

					GameObject wallAlong = (GameObject)Instantiate (Resources.Load ("WallTile"), new Vector3 (i, 0, j), Quaternion.identity);
					wallAlong.GetComponentInChildren<Renderer>().material.color = new Color (colors[wallColor].x, colors[wallColor].y, colors[wallColor].z,10f);
					built = true;

				}


				if(_map[i,j] == 0 && (_map[i+1,j] != 0 || _map[i-1,j] != 0)){

						if(_map[i+1,j] > _map[i-1,j])
							wallColor = _map[i+1,j] + 2;
						else if(_map[i-1,j] > _map[i+1,j])
							wallColor = _map[i-1,j] + 2;

					GameObject wallUp = (GameObject)Instantiate (Resources.Load ("WallTile"), new Vector3 (i, 0, j), Quaternion.identity);
					wallUp.GetComponentInChildren<Renderer>().material.color = new Color (colors[wallColor].x, colors[wallColor].y, colors[wallColor].z, 5f);
					}
			}
		}
		
				
	}


	
		static void printToConsole (int[,] _map, int _sizeX, int _sizeY)
		{
				string print = "";

				for (int x = 0; x < _sizeX; x++) {
						for (int y = 0; y < _sizeY; y++) {
								string toString = _map [x, y].ToString ();
								print = print + toString;
						}
						print = print + System.Environment.NewLine;
				}
		
				Debug.Log (print);
		}


	static void printRoomData(Room[] _rooms){

		foreach (Room element in _rooms) {
			if(element != null){
				List<int> ls =  element.getConnections();
				Debug.Log("Room " + element.getID().ToString() + " has connections: ");
				foreach(int room in ls)
					Debug.Log(room);
				
				
			}
		}

		}

}
