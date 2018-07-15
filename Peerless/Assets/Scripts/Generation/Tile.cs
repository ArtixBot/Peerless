using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {

	public int x;
	public int y;
	public string test = "";
	public Actor[] actors;
	public Actor[] obstacles;

	public Tile(int x, int y, string icon = "x"){
		this.x = x;
		this.y = y;
		this.test = icon;
	}
}
