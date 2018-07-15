using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour {

	public int columns = 100;		                    // Max board height per level.
	public int rows = 100;			                    // Max row height per level.
    public RandInt roomWidth = new RandInt(5, 12);      // Range for room generation width.
    public RandInt roomHeight = new RandInt(3, 9);      // Range for room generation height.
    public RandInt numRooms = new RandInt(8, 15);       // Number of rooms generated per level.
    public GameObject player;
    
    private Room[] roomList;                            // Contains the list of rooms included per board.
	private Tile[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
    private GameObject boardHolder;                           // GameObject that acts as a container for all other tiles.
	
	// Use this for initialization
	void Start () {
		boardHolder = new GameObject("BoardHolder");
		setUpBoard(columns, rows);
        createRooms();

        initalizeRoomsInBoard();

        //printBoard();
	}

	void setUpBoard (int columns, int rows)
    {
        // Set the tiles jagged array to the correct width.
        tiles = new Tile[columns][];
        // Go through all the tile arrays...
        for (int i = 0; i < tiles.Length; i++)
        {
            // ... and set each tile array is the correct height.
            tiles[i] = new Tile[rows];
            for (int j = 0; j < tiles[i].Length; j++){
                tiles[i][j] = new Tile(i, j, "C");
            }
        }
    }

    void createRooms(){
        roomList = new Room[1];
        Room initRoom = new Room();
        roomList[0] = initRoom;
        initRoom.setupStartingRoom(roomWidth, roomHeight, columns, rows);
        Debug.Log("Starting room built at (" + initRoom.xPos + ", " + initRoom.yPos + "), with a width of " + initRoom.roomWidth + " and height of " + initRoom.roomHeight);
    }

    void initalizeRoomsInBoard(){
        foreach (Room room in roomList){
            for (int i = room.yPos; i < room.yPos + room.roomHeight; i++){
                for (int j = room.xPos; j < room.xPos + room.roomWidth; j++){
                    tiles[i][j].test = "R";
                }
            }
        }
    }
    
    public string printBoard(){
        string grid = "";
        for (int i = 0; i < tiles.Length; i++){
            for (int j = 0; j < tiles[i].Length; j++){
                grid += tiles[i][j].test;
            }
            grid += "n";
            grid = grid.Replace("n", System.Environment.NewLine);
        }
        //print(grid);
        return grid;
    }
}

