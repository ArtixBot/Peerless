using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour {

	public const int ROWS = 75;		                   		// Determines # of rows allowed for the board.
	public const int ROW_LENGTH = 150;			            // Determines # of columns allowed for the board.

	// TUNNELER PARAMETERS ARE CONTAINED WITHIN TUNNELER.CS.
	public const int NUM_TUNNELERS = 3;						// Determines # of tunnelers which will dig out an area.

    public RandInt roomWidth = new RandInt(2, 5);      		// Range for room generation width.
    public RandInt roomHeight = new RandInt(2, 5);     		// Range for room generation height.
    public RandInt numRooms = new RandInt(8, 15);      		// Number of rooms generated per level.
    
	private Tile[][] tiles;             	                // A jagged array of tile types representing the board, like a grid.
    private GameObject boardHolder;         	            // GameObject that acts as a container for all other tiles.
	
	// Use this for initialization
	void Start () {
		boardHolder = new GameObject("BoardHolder");
		SetUpBoard(ROWS, ROW_LENGTH);

		Tunneler tunnelLad = new Tunneler (50, 50);
		tunnelLad.Dig (ref tiles);
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
                tiles[y][x] = new Tile(y, x, "#");
            }
        }
	}

    // ===UI===
    // Used to make the board visible.
    public string PrintBoard(){
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

