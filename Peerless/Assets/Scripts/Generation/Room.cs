using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {
	public int xPos;			// Starting coordinate for the upperleft tile of the room.
	public int yPos;			// Starting coordinate for the upperleft tile of the room.
	public int roomWidth;		// Width of the room.
	public int roomHeight;		// Height of the room.

	// Generates the first starting room in the center (or as closely as possible) of the playing field.
	public void setupStartingRoom(RandInt width, RandInt height, int centerX, int centerY){
		roomWidth = width.random;
		roomHeight = height.random;

		xPos = Mathf.RoundToInt(centerX / 2f - roomWidth / 2f);
		yPos = Mathf.RoundToInt(centerY / 2f - roomHeight / 2f);
	}
}
