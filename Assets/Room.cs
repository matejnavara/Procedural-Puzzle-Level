using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room {

	private int roomID;
	private int[,] roomArea;
	private List<int> connections;
	private List<Vector2> doorways;
	private bool isConnected;
	private bool isLocked;
	private bool lockAssigned;
	private int puzzleTo;

	public Room(int[,] _Map, int _roomID){
		this.roomID = _roomID;
		this.roomArea = _Map;
		connections = new List<int> ();
		doorways = new List<Vector2> ();
		isConnected = false;
		isLocked = false;

		puzzleTo = 0;

		}

	public int getID(){
		return roomID;
		}

	public int[,] getArea(){
		return roomArea;
		}

	public void addConnection(int _roomID){
		connections.Add(_roomID);
		}

	public List<int> getConnections(){
		return connections;
		}

	public int getConnectionNum(){
		return connections.Count;
		}

	public void addDoorway(Vector2 _pos){
		doorways.Add (_pos);
		}

	public List<Vector2> getDoorways(){
		return doorways;
		}

	public void setConnected(){
		isConnected = true;
		}

	public bool getConnected(){
		return isConnected;
		}

	public void setLocked(bool b){
		isLocked = b;
	}

	public bool getLocked(){
		return isLocked;
	}

	public void assignLock(){
		lockAssigned = true;
	}

	public bool assigned(){
		return lockAssigned;
		}

	public void setPuzzle(int s){
		puzzleTo = s;
	}
	
	public int getPuzzle(){
		return puzzleTo;
	}

}
