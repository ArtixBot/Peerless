using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour {

	public const int ROWS = 45;		                   		// Determines # of rows allowed for the board.
	public const int ROW_LENGTH = 200;			            // Determines # of columns allowed for the board.

	// TUNNELER PARAMETERS ARE CONTAINED WITHIN TUNNELER.CS.
	// ROOM TUNNELER PARAMETERS ARE CONTAINED WITHIN ROOMTUNNELER.CS
	public const int NUM_TUNNELERS = 1;						// Determines # of tunnelers which will dig out an area. Note that tunnelers summon more tunnelers so increasing can get out of hand!

    public RandInt roomWidth = new RandInt(2, 5);      		// Range for room generation width.
    public RandInt roomHeight = new RandInt(2, 5);     		// Range for room generation height.
    public RandInt numRooms = new RandInt(8, 15);      		// Number of rooms generated per level.

	public static List<int[]> RoomDiggers = new List<int[]> ();	// Contains a list of coordinates. Room tunnelers will be created and activated at these coordinates.
	public Tile[][] tiles;             	                // A jagged array of tile types representing the board, like a grid.
    private GameObject boardHolder;         	            // GameObject that acts as a container for all other tiles.
	
	// Use this for initialization
	void Start () {
		float startUp = Time.realtimeSinceStartup;
		boardHolder = new GameObject("BoardHolder");

		SetUpBoard(ROWS, ROW_LENGTH);


		Tunneler tunnelLad = new Tunneler (100, 25);	
		tunnelLad.Dig (ref tiles);

		RoomTunneler testRoom = new RoomTunneler (100, 25);
		testRoom.Activate (ref tiles);
		print("Execution time of all board-gen scripts took " + (Time.realtimeSinceStartup - startUp) + " seconds.");

		for (int i = 0; i < RoomDiggers.Count; i++) {
			//Debug.Log (RoomDiggers [i] [0] + " " + RoomDiggers [i] [1]);
			RoomTunneler roomTunnel = new RoomTunneler (RoomDiggers [i] [0], RoomDiggers [i] [1]);
			roomTunnel.Activate (ref tiles);
		}
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
	void SetUpBoard (int rows, int rowLength)
    {
        // Establish a array of {rows} arrays overall.
        tiles = new Tile[ROWS][];
        // Go through each row...
        for (int y = 0; y < tiles.Length; y++)
        {
			// ... and set each row to equal {rowLength}.
            // We now have an array of {rows} arrays, with each of those arrays of length {rowLength}.
            tiles[y] = new Tile[ROW_LENGTH];
            for (int x = 0; x < tiles[y].Length; x++){
                tiles[y][x] = new Tile(y, x);
            }
        }
	}
}

