using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// THIS CLASS SHOULD BE USED FOR TESTING PURPOSES RIGHT NOW.
public class ViewBoardTile : MonoBehaviour {

	//public Tilemap boardMap;
	public BoardGenerator boardGen;
	public Text boardText;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		boardText.text = renderBoard (boardGen.tiles);
	}

	string renderBoard(Tile[][] board){
		string grid = "";
		for (int i = 0; i < board.Length; i++){
			for (int j = 0; j < board[i].Length; j++){
				//grid += tiles[i][j].test;
				if (board[i] [j].property == Tile.TileState.IS_FLOOR) {
					grid += ".";
				} else if (board[i] [j].property == Tile.TileState.IS_WALL) {
					grid += "#";
				} else if (board[i] [j].property == Tile.TileState.IS_DOOR) {
					grid += "D";
				} else if (board[i] [j].property == Tile.TileState.TEST) {
					grid += "T";
				}
			}
			grid += "n";
			grid = grid.Replace("n", System.Environment.NewLine);
		}
		//print(grid);
		return grid;
	}
}
