using UnityEngine;
using System.Collections;



public class BSPTree : MonoBehaviour {

	public int mapsize = 100;
	public int splitsize = 4;
	public float splitchance;
	public BSPNode parentNode;
	
	public static Grid levelGrid;
	private int roomID = 0;
	
	// Use this for initialization
	void Start () {
		GameObject startCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		startCube.transform.localScale = new Vector3 (mapsize,1,mapsize);
		//startCube.tag = "GenSection";
		startCube.transform.position = new Vector3(transform.position.x + startCube.transform.localScale.x/2,
			transform.position.y,
			transform.position.z + startCube.transform.localScale.z/2);
		
		levelGrid = new Grid((int) startCube.transform.localScale.x,(int) startCube.transform.localScale.z);
		
		for (int i = 0; i < levelGrid.Width(); i++){
			for (int j = 0; j < levelGrid.Height(); j++){
				levelGrid.setTile(i,j,0);
			}
		}
		
		parentNode = new BSPNode();
		parentNode.setCube(startCube);
		
		
		for (int i = 0; i < splitsize; i++){
			split (parentNode);	
		}
		
		//create the rooms
		createRooms(parentNode);
		
		//connect the rooms
		connectRooms(parentNode);

		//construct the walls/tiles
		createLevel();

		//spawn player
		GameObject player = (GameObject) Instantiate(Resources.Load("Player"),new Vector3(parentNode.getRoom().transform.position.x+5,
		                                                                                  parentNode.getRoom().transform.position.y+5,
		                                                                                  parentNode.getRoom().transform.position.z+5)
		                                                                                  ,Quaternion.identity);
		
	}
	
	//split the tree
	public void split(BSPNode _aNode){
		if (_aNode.getLeftNode() != null){
			split(_aNode.getLeftNode());	
		}else{
		 	_aNode.cut();
			return;
		}
		
		if (_aNode.getLeftNode() != null){
			split(_aNode.getRightNode());	
		}
		
	}
	
	public static Grid getGrid(){
		return levelGrid;
	}
	
	public static void setTile(int _x, int _y, int _value){
		levelGrid.setTile(_x,_y, _value);	
	}
	
	private void addRoom(BSPNode _aNode){
		
		GameObject aObj = _aNode.getCube();
		
		GameObject aRoom = (GameObject) Instantiate(Resources.Load("BaseRoom"),aObj.transform.position,Quaternion.identity);
		aRoom.transform.localScale = new Vector3(
			(int)(Random.Range(10, aObj.transform.localScale.x-5)),
			aRoom.transform.localScale.y,
			(int)(Random.Range(10, aObj.transform.localScale.z-5)));
		aRoom.GetComponent<RoomCreator>().setup();
		aRoom.GetComponent<RoomCreator>().setID(roomID);
		aRoom.GetComponent<RoomCreator>().setParentNode(_aNode);
		_aNode.setRoom(aRoom);
		Debug.Log ("Room number: " + roomID);
		roomID++;
	}
	
	private void createRooms(BSPNode _aNode){
		if (_aNode.getLeftNode() != null){
			createRooms(_aNode.getLeftNode());	
		}else{
			addRoom(_aNode);
			return;
		}
		
		if (_aNode.getRightNode() != null){
			createRooms(_aNode.getRightNode());	
		}
	}
	
	private void connectRooms(BSPNode _aNode){
		if (_aNode.getLeftNode() != null){
			connectRooms(_aNode.getLeftNode());	
			
			if (_aNode.getRoom() != null){
				_aNode.getRoom().GetComponent<RoomCreator>().connect();
						
				return;
			}
			
		}else{
			if (_aNode.getRoom() != null){
				_aNode.getRoom().GetComponent<RoomCreator>().connect();
						
				return;
			}
		}
		
		if (_aNode.getRightNode() != null){
				connectRooms(_aNode.getRightNode());
			
			if (_aNode.getRoom() != null){
				_aNode.getRoom().GetComponent<RoomCreator>().connect();
						
				return;
			}
		}else{
			if (_aNode.getRoom() != null){
				_aNode.getRoom().GetComponent<RoomCreator>().connect();		
				
				return;
			}
		}
		
	}
	
	private void createLevel(){
		for (int i = 0; i < levelGrid.Width(); i++){
			for (int j = 0; j < levelGrid.Height(); j++){
				
				switch(levelGrid.getTile(i,j)){
				case 1:
					GameObject tile = (GameObject)Instantiate(Resources.Load("FloorTile"), new Vector3(transform.position.x - (transform.localScale.x/2) + i, transform.position.y + transform.localScale.y/2, transform.position.z - (transform.localScale.z/2) + j), Quaternion.identity);
					tile.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
					//new Color(tile.GetComponent<RoomCreator>().getColor().r, tile.GetComponent<RoomCreator>().getColor().g, tile.GetComponent<RoomCreator>().getColor().b);
					break;
				case 2:
					Instantiate(Resources.Load("WallTile"), new Vector3(transform.position.x - (transform.localScale.x/2) + i, transform.position.y + transform.localScale.y/2, transform.position.z - (transform.localScale.z/2) + j), Quaternion.identity);
					break;
				}
	
			}
		}
	}

	
}
