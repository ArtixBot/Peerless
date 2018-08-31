using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* =================
 * BOARDGENERATOR.CS
 * =================
 * 
 * I guess this is where the stew is made.
 * Tunneler parameters are stored within Tunneler.cs.
 * Room tunneler parameters are stored within RoomTunneler.cs (Yes, there's significant difference in how they operate...).
 * 
 * The board generator itself just initializes the 2d tile array, then spawns one tunneler.
 * This one tunneler, like some sort of broodmother, spawns more tunnelers that attempt to go in other directions.
 * As tunnelers tunnel (wow!) they insert coordinate markers into BoardGenerator's RoomDiggers list.
 * Presumably, after all tunnelers are finished, BoardGenerator then iterates over RoomDiggers, spawning room tunnelers in all marked locations.
 * The room tunnelers go off and build rooms. If successful they may also spawn more room tunnelers.
 */

public class BoardGenerator : MonoBehaviour {

	public const int ROWS = 45;		                   		// Determines # of rows allowed for the board.
	public const int ROW_LENGTH = 200;			            // Determines # of columns allowed for the board.
	public const int NUM_TUNNELERS = 1;						// Determines # of tunnelers which will dig out an area. Note that tunnelers summon more tunnelers so increasing can get out of hand!

	public static List<int[]> RoomDiggers = new List<int[]> ();	// Contains a list of coordinates. Room tunnelers will be created and activated at these coordinates.

	public static List<int[]> ListOfRooms = new List<int[]> ();	// Contains a list of all generated rooms. Each room may be subject to prefab generation.
	public Tile[][] tiles;             	               			// A jagged array of tile types representing the board, like a grid.
    private GameObject boardHolder;         	            	// GameObject that acts as a container for all other tiles.
	
	// Commence stewmaking
	void Start () {
		float startUp = Time.realtimeSinceStartup;
		boardHolder = new GameObject("BoardHolder");

		SetUpBoard(ROWS, ROW_LENGTH);

		Tunneler tunnelLad = new Tunneler (100, 25);	
		tunnelLad.Dig (ref tiles);

		for (int i = 0; i < RoomDiggers.Count; i++) {
			//Debug.Log (RoomDiggers [i] [0] + " " + RoomDiggers [i] [1]);
			RoomTunneler roomTunnel = new RoomTunneler (RoomDiggers [i] [0], RoomDiggers [i] [1]);
			roomTunnel.Activate (ref tiles);
		}


		print("Execution time of all board-gen scripts took " + (Time.realtimeSinceStartup - startUp) + " seconds.");
	}

    // Sets up gameplay board.
	/* In a wholly unintuitive sense (to me, anyways), accessing the board should be board[y][x].
	 * Think of the board like this:
    [
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

