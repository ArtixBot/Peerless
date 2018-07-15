using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour {

	public int columns = 100;		                    // Max board height per level.
	public int rows = 100;			                    // Max row height per level.
    public RandInt roomWidth = new RandInt(5, 12);      // Range for room generation (currently: 5-10).
    public RandInt roomHeight = new RandInt(3, 9);
    public GameObject player;

	private Tile[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
    private GameObject boardHolder;                           // GameObject that acts as a container for all other tiles.
	
	// Use this for initialization
	void Start () {
		boardHolder = new GameObject("BoardHolder");
		setUpBoard(columns, rows);
        createRooms();
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
                tiles[i][j] = new Tile(i, j);
            }
        }
    }

    void createRooms(){
        Room initRoom = new Room();
        initRoom.setupStartingRoom(roomWidth, roomHeight, columns, rows);
        Debug.Log("Starting room built at (" + initRoom.xPos + ", " + initRoom.yPos + "), with a width of " + initRoom.roomWidth + " and height of " + initRoom.roomHeight);
    }
}
