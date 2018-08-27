using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random=System.Random;

// The room tunneler digs rooms. For tunneler adjustments, check Tunneler.cs.
public class RoomTunneler{

	// ROOM TUNNELER PARAMETERS
	public const int MIN_WIDTH = 2;					// Room borders must be at minimum length [MIN_WIDTH] for generation.
	public const int MIN_HEIGHT = 2;				// Same as above but for height instead.
	public const int MAX_WIDTH = 14;				// Room borders must be at maximum length [MAX_WIDTH]. Room length itself randomly vaiies: [MIN_WIDTH] <= value <= [MAX_WIDTH].
	public const int MAX_HEIGHT = 8;				// Same as above but for height instead.
	public const int IMBALANCE_TOLERANCE = 3;		// The higher the number, the more imbalance tolerance--so a door's entrance could split the room, 35% left and 65% right.
	public const int CHANCE_EXTRA_DOORS = 75;		// If a room is dug out, there's a [CHANCE_EXTRA_DOORS]% chance to dig additional doors randomly along the border.

	// Room tunneler properties, do not modify.
	public static Random rng = new Random ();
	public char[] direction = {'N', 'S', 'W', 'E'};		// The direction the tunneler should dig in. N = [-1][], S = [+1][], W = [][-1], E = [][+1]

	public int x;
	public int y;
	public char curDirection;							// Current direction of the tunneler.

	public RoomTunneler(int x, int y){
		this.x = x;		// The tunneler's coordinates are provided via (x, y) but the tunneler itself moves via (y, x) coordinates.
		this.y = y;
		this.curDirection = direction [rng.Next (0, direction.Length)];
	}

	// A room tunneler digs at most one room and then is killed off.
	public void Activate(ref Tile[][] board){
		int modX = 0;
		int modY = 0;
		switch (this.curDirection) {
		case 'N':
			modY = -1;
			break;
		case 'S':
			modY = 1;
			break;
		case 'W':
			modX = -1;
			break;
		case 'E':
			modX = 1;
			break;
		}
		while (!IsWall(board, this.x, this.y)) {
			this.x += modX;
			this.y += modY;
		}
		board [this.y] [this.x].property = Tile.TileState.TEST;
		ScanAndDig (ref board, this.curDirection);
	}

	// Go one tile past the door; scan in all four cardinal directions to get overall theoretical width and height.
	// Assuming prerequisites are met, build the room leaving space for walls.
	// TODO: adjust things so that it doesn't sound like a fucking Trump speech
	public void ScanAndDig(ref Tile[][] board, char direction){
		int height = 0, up = 0, down = 0;
		int width = 0, left = 0, right = 0;
		int doorX = this.x;
		int doorY = this.y;

		switch (direction) {
		case 'N':
			this.y -= 1;	// Accomodate for the door's (predicted) position. In the case that we approach from the north and south, we want to find the width first.
			break;
		case 'S':
			this.y += 1;
			break;
		case 'W':
			this.x -= 1;
			break;
		case 'E':
			this.x += 1;
			break;
		}
		if (!IsWall (board, this.x, this.y) && this.x > 0 && this.x < board[0].Length && this.y > 0 && this.y < board.Length && rng.Next (0, 100) < CHANCE_EXTRA_DOORS) {
			// Seems we've hit another floor tile or door. Roll for CHANCE_EXTRA_DOOR to possibly place, then terminate self.
			board [doorY] [doorX].property = Tile.TileState.IS_DOOR;
			return;
		} else {
			int[] widthCoords;
			int[] heightCoords;
			if (direction == 'N' || direction == 'S') {
				widthCoords = ScanWidth (board, this.x, this.y);
				width = widthCoords [0] + 1; left = widthCoords [1]; right = widthCoords [2];
				if (direction == 'N') {
					up = Math.Min (DirectionalScan (board, this.x - left, this.y - 1, 'N'), DirectionalScan (board, this.x + right, this.y - 1, 'N'));
					height = up + 1;
				} else {
					down = Math.Min (DirectionalScan (board, this.x - left, this.y + 1, 'S'), DirectionalScan (board, this.x + right, this.y + 1, 'S'));
					height = down + 1;
				}
				heightCoords = new int[] {height, up, down};
			} else {
				heightCoords = ScanHeight (board, this.x, this.y);
				height = heightCoords [0] + 1; up = heightCoords [1]; down = heightCoords [2];
				if (direction == 'W') {
					left = Math.Min (DirectionalScan (board, this.x - 1, this.y + up, 'W'), DirectionalScan (board, this.x - 1, this.y - down, 'W'));
					width = left + 1;
				} else {
					right = Math.Min (DirectionalScan (board, this.x + 1, this.y + up, 'E'), DirectionalScan (board, this.x + 1, this.y - down, 'E'));
					width = right + 1;
				}
				widthCoords = new int[]{ width, left, right };
			}
			int[][] borders = { widthCoords, heightCoords };
			//Debug.Log ("Direction of input: " + direction + ". Width: " + borders [0] [0] + ", left and right components are " + borders[0][1] + " " + borders[0][2] + ". Height: " + borders[1][0] + ", up and down components are " + borders[1][1] + " " + borders[1][2] + ".");
			if (RequirementCheck (borders)) {
				DigOut (ref board, borders);
				board [doorY] [doorX].property = Tile.TileState.IS_DOOR;		// If minimum requirements not met, return the testing tile back to normal.
			} else {
				board [doorY] [doorX].property = Tile.TileState.IS_WALL;		// If minimum requirements not met, return the testing tile back to normal.
			}
			return;
		}
	}

	// Does bounds checking for me. How convenient!
	public bool IsWall(Tile[][] board, int x, int y){
		if (x >= 0 && x < board [0].Length && y >= 0 && y < board.Length) {
			return board [y] [x].property == Tile.TileState.IS_WALL;
		}
		return false;
	}

	public int[] ScanWidth(Tile[][] board, int x, int y){
		int left = 0, right = 0;
		while (IsWall (board, x - (left + 1), y) && IsWall (board, x - (left + 1), y - 1) && IsWall (board, x - (left + 1), y + 1)) {
			left += 1;
		}
		while (IsWall (board, x + (right + 1), y) && IsWall (board, x + (right + 1), y - 1) && IsWall (board, x + (right + 1), y + 1)) {
			right += 1;
		}
		while (left + right > MAX_WIDTH) {
			// Prevent rooms from being like 99% left, 1% right. Or something like that.
			if (left - right >= IMBALANCE_TOLERANCE) {
				left -= 1;
			} else if (right - left >= IMBALANCE_TOLERANCE) {
				right -= 1;
			} else {
				if (rng.Next (0, 100) < 50) {
					left -= 1;
				} else {
					right -= 1;
				}
			}
		}

		// Accomodate for the one buffer tile. Also, leave one remaining tile so that we don't dig adjacent to walls.
		int[] retWidth = {left + right - 1, left - 1, right - 1};		// Index 0 contains width, index 1 the left coordinate's length, 2 the right.
		//Debug.Log("Width: " + (left + right + 1) + ", left direction: " + left + ", right direction: " + right);
		return retWidth;
	}

	public int[] ScanHeight(Tile[][] board, int x, int y){
		int up = 0, down = 0;
		while (IsWall (board, x, y - (up + 1)) && IsWall (board, x - 1, y - (up + 1)) && IsWall (board, x + 1, y - (up + 1))) {
			up += 1;
		}
		while (IsWall (board, x, y + (down + 1)) && IsWall (board, x - 1, y + (down + 1)) && IsWall (board, x + 1, y + (down + 1))) {
			down += 1;
		}
		while (up + down > MAX_HEIGHT) {
			// Prevent rooms from being like 99% left, 1% right. Or something like that.
			if (up - down >= IMBALANCE_TOLERANCE) {
				up -= 1;
			} else if (down - up >= IMBALANCE_TOLERANCE) {
				down -= 1;
			} else {
				if (rng.Next (0, 100) < 50) {
					up -= 1;
				} else {
					down -= 1;
				}
			}
		}
		int[] retHeight = {up + down - 1, up - 1, down - 1};		// Index 0 contains height, index 1 the up coordinate's length, 2 the down.
		//Debug.Log("Height: " + (up + down + 1) + ", up direction: " + up + ", down direction: " + down);
		return retHeight;
	}

	public int DirectionalScan(Tile[][] board, int x, int y, char direction){
		int dirLength = 0;
		switch (direction) {
		case 'N':
			while (IsWall (board, x, y - (dirLength + 1)) && dirLength < MAX_HEIGHT - 1) {
				dirLength += 1;
			}
			break;
		case 'S':
			while (IsWall (board, x, y + dirLength + 1) && dirLength < MAX_HEIGHT - 1) {
				dirLength += 1;
			}
			break;
		case 'W':
			while (IsWall (board, x - (dirLength + 1), y) && dirLength < MAX_WIDTH - 1) {
				dirLength += 1;
			}
			break;
		case 'E':
			while (IsWall (board, x + dirLength + 1, y) && dirLength < MAX_WIDTH - 1) {
				dirLength += 1;
			}
			break;
		}
		return dirLength;
	}

	public bool RequirementCheck(int[][] borders){
//		if (borders [0] [0] >= MIN_WIDTH && borders [0] [0] <= MAX_WIDTH) {
//			Debug.Log ("Passed width check.");
//		}
//		if (borders [0] [1] >= 0 && borders [0] [2] >= 0) {
//			Debug.Log ("Passed width component check.");
//		}
//		if (borders [1] [0] >= MIN_HEIGHT && borders [1] [0] <= MAX_HEIGHT) {
//			Debug.Log ("Passed height check.");
//		}
//		if (borders [1] [1] >= 0 && borders [1] [2] >= 0) {
//			Debug.Log ("Passed height component check.");
//		}
		return borders [0] [0] >= MIN_WIDTH && borders [0] [0] <= MAX_WIDTH && borders [0] [1] >= 0 && borders [0] [2] >= 0 && borders[1][0] >= MIN_HEIGHT
			&& borders[1][0] <= MAX_HEIGHT && borders[1][1] >= 0 && borders[1][2] >= 0;
	}

	public void DigOut(ref Tile[][] board, int[][] dimensions){
		for (int i = -dimensions [0] [1]; i <= dimensions [0] [2]; i++) {
			for (int j = -dimensions [1] [1]; j <= dimensions [1] [2]; j++) {
				board [this.y + j][this.x + i] .property = Tile.TileState.IS_FLOOR;
			}
		}
	}
}