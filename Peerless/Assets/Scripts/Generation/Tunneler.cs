using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random=System.Random;

public class Tunneler{

	// TUNNELER PARAMETERS
	public RandInt battery = new RandInt(50, 100);		// The tunneler is powered to dig {battery} tiles before being dismantled for eternity.
	public RandInt corridorLength = new RandInt(4, 8);	// The tunneler digs straight for {corridorLength} tiles before changing directions.

	public int x;										// Tunneler x-and-y position (aka rowLength-and-row position).
	public int y;
	public int lifespan;								// How far can the tunneler dig before it's killed off.
	public int corridor;								// Number of turns the tunneler digs in a direction.
	public string[] direction = {"North", "South", "West", "East"};	// The direction the tunneler should dig in. N = [-1][], S = [+1][], W = [][-1], E = [][+1]

	public Tunneler(int x, int y){
		this.x = x;
		this.y = y;
		this.lifespan = battery.random;
		this.corridor = corridorLength.random;
		Debug.Log ("Instantiated with lifespan " + this.lifespan);
	}

	// The Tunneler is born! It digs in a certain direction, then rotates, continuing until killed.
	public void Dig(ref Tile[][] board){
		string dir = getDirection ();
		while (this.lifespan > 0) {
			while (this.corridor > 0) {
				board [this.x] [this.y].test = ".";
				switch (dir) {
				case "North":
					this.y -= 1;
					Debug.Log ("North");
					break;
				case "South":
					this.y += 1;
					Debug.Log ("South");
					break;
				case "West":
					this.x -= 1;
					Debug.Log ("West");
					break;
				case "East":
					this.x += 1;
					Debug.Log ("East");
					break;
				}
				this.lifespan -= 1;
				this.corridor -= 1;
			}
			this.corridor = corridorLength.random;
			dir = getDirection (dir);
		}
	}

	// Change the direction of the tunneler.
	// The tunneler should never choose a direction opposite its previous one. (ex.: If prev. dir. was South, don't go North).
	// Likewise, the tunneler should never CONTINUE in its current direction. (ex.: If prev. dir. was South, don't go South).
	public string getDirection(string prevDirection = null){
		switch (prevDirection) {
		case "North":
		case "South":
			return direction [new Random ().Next (2, direction.Length)];	// Returns index 2/3.
			break;
		case "West":
		case "East":
			return direction [new Random ().Next (0, 2)];	// The direction array is 4 elements long, but Random().Next() gets from [0, 2), for effectively 0 or 1.
			break;
		default:
			return "North";
			break;
		}
	}
}
