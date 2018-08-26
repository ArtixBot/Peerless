using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
	public enum TileState{IS_WALL, IS_FLOOR, IS_DOOR, TEST};

	public int x;
	public int y;
	public TileState property;
	public Actor[] actors;
	public Actor[] obstacles;

	public Tile(int x, int y){
		this.x = x;
		this.y = y;
		this.property = TileState.IS_WALL;
	}
}
