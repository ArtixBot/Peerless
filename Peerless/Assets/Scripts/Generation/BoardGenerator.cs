using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour {

	public int rows;		                           // Determines the number of rows allowed for the board.
	public int rowLength;			                   // Determines the number of columns allowed for the board.
    public RandInt roomWidth = new RandInt(2, 5);      // Range for room generation width.
    public RandInt roomHeight = new RandInt(2, 5);     // Range for room generation height.
    public RandInt numRooms = new RandInt(8, 15);      // Number of rooms generated per level.
    public GameObject player;
    
    private Room[] roomList;                           // Contains the list of rooms included per board.
	private Tile[][] tiles;                            // A jagged array of tile types representing the board, like a grid.
    private GameObject boardHolder;                    // GameObject that acts as a container for all other tiles.
	
	// Use this for initialization
	void Start () {
		boardHolder = new GameObject("BoardHolder");
		setUpBoard(rows, rowLength);
        createRooms();

        initalizeRoomsInBoard();
	}

    // Sets up gameplay board.
    /* Think of the board like this:
    [
        [0, 0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 0],
        ...
    ]
     */
	void setUpBoard (int rows, int rowLength)
    {
        // Establish a array of ${rows} arrays overall.
        tiles = new Tile[rows][];
        // Go through all the tile arrays...
        for (int y = 0; y < tiles.Length; y++)
        {
            // ... and set each tile array is the correct height.
            // We now have an array of ${rows} arrays, with each of those arrays of length ${rowLength}
            tiles[y] = new Tile[rowLength];
            for (int x = 0; x < tiles[y].Length; x++){
                tiles[y][x] = new Tile(y, x, "#");
            }
        }
    }

    // Generates rooms.
    void createRooms(){
        // Determine the number of rooms to spawn.
        roomList = new Room[numRooms.random];
        Debug.Log("Creating " + roomList.Length + " rooms.");

        // The first room should use setupStartingRoom.
        // Debug statement values are based on indices (start from 0!)
        Room initRoom = new Room();
        roomList[0] = initRoom;
        initRoom.setupStartingRoom(roomWidth, roomHeight, rowLength, rows);

        Debug.Log("Starting room built at row " + initRoom.xPos + ", column " + initRoom.yPos + ", with a width of " + initRoom.roomWidth + " and height of " + initRoom.roomHeight);

        // After first room is generated, generate all future rooms.
        for (int i = 1; i < roomList.Length; i++){
            Room r = new Room();
            roomList[i] = r;
            //r.setupRoom(roomWidth, roomHeight, rowLength, rows);
        }
    }

    // Instantiates rooms so that they appear on the board.
    void initalizeRoomsInBoard(){
        foreach (Room room in roomList){
            //Debug.Log("Initalizing room. (x, y) = (" + room.xPos + "," + room.yPos +"), (w, h) = (" + room.roomWidth + ", " + room.roomHeight +").");
            for (int row = room.xPos; row < room.xPos + room.roomWidth; row++){
                for (int rowIndex = room.yPos; rowIndex < room.yPos + room.roomHeight; rowIndex++){
                    tiles[row][rowIndex].test = ".";
                }
            }
        }
    }
    
    // ===UI===
    // Used to make the board visible.
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

