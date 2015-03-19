using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{

		public int sizeX = 200;
		public int sizeY = 200;
		public int nbrRooms = 18;

		// Use this for initialization
		void Start ()
		{
				int[,] map;
				map = CreateLevelSquares (sizeX, sizeY, nbrRooms);

				int roomCount;
				roomCount = FindRooms (map, sizeX, sizeY);



		int[,] start;
		start = getRoom (map, 1);
		bool playerSpawned = false;

		for (int i = 0; i < sizeX; i++) {
						for (int j = 0; j < sizeY; j++) {
						if(start[i,j] == 1 && !playerSpawned){
					GameObject player = (GameObject) Instantiate(Resources.Load("Player"),new Vector3(i+5,5,j+5),Quaternion.identity);
					playerSpawned = true;
				}
			}
		}

		for (int i = 1; i <= roomCount; i++) {
			FindNeighbour (map, i);
		}

		BuildFloor (map, sizeX, sizeY, roomCount);
		BuildWalls (map, sizeX, sizeY);
		
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
//										GameObject tile = (GameObject)Instantiate (Resources.Load ("FloorTile"), new Vector3 (j, 0, k), Quaternion.identity);
//										tile.renderer.material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
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
	
	static void FindNeighbour(int[,] _map, int _roomID){
		int sizeX = _map.GetUpperBound (0)+1;
		int sizeY = _map.GetUpperBound (1)+1;

		int maxX = 0;
		int maxY = 0;
		int minX = sizeX;
		int minY = sizeY;

		for (int i = 0; i < sizeX; i++) {
			for (int j = 0; j < sizeY; j++) {
				if(_map[i,j] == _roomID){
					if(i > maxX)
						maxX = i;
					if(i < minX)
						minX = i;
					if(j > maxY)
						maxY = j;
					if(j < minY)
						minY = j;
				}
			}
		}

		string stats = " Max X: " + maxX.ToString() + " Min X: " + minX.ToString() + " Max Y: " + maxY.ToString() + " Min Y: " + minY.ToString() ;
		Debug.Log (stats);

		//Check right
		bool foundRight = false;

		int rightMax = 0;
		int rightMin = sizeY;
		int rightRoom = 0;
		int rightX = 0;

		for (int i = maxX; i < sizeX; i++) {
			if(!foundRight){
			for (int j = minY; j < maxY; j++) {
				if(_map[i,j] != _roomID && _map[i,j] != 0){

						rightRoom = _map[i,j];
						rightX = i;
						foundRight = true;

						if(j > rightMax)
							rightMax = j;
						if(j < rightMin)
							rightMin = j;
				}
			}	
		}

		}

		if(foundRight){
			int rightRandom = Random.Range (rightMin, rightMax - 3);
			connectRight (_map, _roomID, rightRandom, rightX);

			string foundR = "RIGHT: Found room " + rightRoom.ToString() + "  between Y: " + rightMin.ToString() + " and " + rightMax.ToString() + " at X: " + rightX.ToString();
			Debug.Log(foundR);
		}


		//Check down
		bool foundDown = false;
		
		int downMax = 0;
		int downMin = sizeX;
		int downRoom = 0;
		int downY = 0;
		
		for (int j = minY; j > 0; j--) {
			if(!foundDown){
				for (int i = minX; i < maxX; i++) {
					if(_map[i,j] != _roomID && _map[i,j] != 0 && _map[i,j] != -1){
						
						downRoom = _map[i,j];
						downY = j;
						foundDown = true;
						
						if(i > downMax)
							downMax = i;
						if(i < downMin)
							downMin = i;
					}
				}	
			}
			
		}

		if(foundDown ){
			int downRandom = Random.Range (downMin , downMax - 3);
			string foundD = "DOWN: Found room " + downRoom.ToString() + "  between X: " + downMin.ToString() + " and " + downMax.ToString() + " at Y: " + downY.ToString();
			connectDown(_map, _roomID, downRandom, downY);

			Debug.Log(foundD);
		}

	}

	static void connectRight(int[,] _map, int _roomID, int _rand, int _startX){
		int sizeX = _map.GetUpperBound (0)+1;
		int sizeY = _map.GetUpperBound (1)+1;

		bool connected = false;

		for (int i = _startX; i > 0; i--) {
			if(!connected){
				for (int j = _rand; j < _rand+3; j++) {
					if(_map[i,j] == 0){
						_map[i,j] = -1;
					} else if( _map[i,j] == _roomID){
						connected = true;
					}

				}
			}
		}

	}

	static void connectDown(int[,] _map, int _roomID, int _rand, int _startY){
		int sizeX = _map.GetUpperBound (0)+1;
		int sizeY = _map.GetUpperBound (1)+1;
		
		bool connected = false;
		
		for (int j = _startY; j < sizeY; j++) {
			if(!connected){
				for (int i = _rand; i < _rand+3; i++) {
					if(_map[i,j] == 0){
						_map[i,j] = -1;
					} else if( _map[i,j] == _roomID){
						connected = true;
					}
					
				}
			}
		}
		
	}

	
	static void BuildFloor (int[,] _map, int _sizeX, int _sizeY, int rooms)
	{
		
		for (int k = 1; k <= rooms; k++) {
			Color roomColor = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			for (int i = 0; i < _sizeX; i++) {
				for (int j = 0; j < _sizeY; j++) {
					if (_map [i, j] == k) {
						GameObject tile = (GameObject)Instantiate (Resources.Load ("FloorTile"), new Vector3 (i, 0, j), Quaternion.identity);
						tile.renderer.material.color = roomColor;
					} else if(_map [i, j] == -1){
						GameObject tile = (GameObject)Instantiate (Resources.Load ("FloorTile"), new Vector3 (i, 0, j), Quaternion.identity);
						tile.renderer.material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
					}
					
				}
			}
			

				}
		}

	static void BuildWalls(int[,] _map, int _sizeX, int _sizeY){
		for (int i = 1; i < _sizeX - 1; i++) {
			for (int j = 1; j < _sizeY - 1; j++) {
				if(_map[i,j] == 0 && (_map[i,j+1] != 0 || _map[i,j-1] != 0)){
					GameObject wall = (GameObject)Instantiate (Resources.Load ("WallTile"), new Vector3 (i, 0, j), Quaternion.identity);
				}
				if(_map[i,j] == 0 && (_map[i+1,j] != 0 || _map[i-1,j] != 0)){
					GameObject wall = (GameObject)Instantiate (Resources.Load ("WallTile"), new Vector3 (i, 0, j), Quaternion.identity);
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

}
