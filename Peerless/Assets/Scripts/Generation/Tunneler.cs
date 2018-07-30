using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random=System.Random;

public class Tunneler{

	// TUNNELER PARAMETERS
	public RandInt battery = new RandInt(50, 100);		// The tunneler is powered to dig {battery} tiles before being dismantled for eternity.
	public RandInt corridorLength = new RandInt(6, 8);	// The tunneler digs straight for {corridorLength} tiles before changing directions.

	public static Random rng = new Random ();
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
	}

	// The Tunneler is born! It digs in a certain direction, then rotates, continuing until killed.
	public void Dig(ref Tile[][] board, string dir = null){
		while (this.lifespan > 0) {
			dir = getDirection (dir);
			this.corridor = corridorLength.random;
			while (this.corridor > 0) {
				board [this.y] [this.x].test = ".";
				switch (dir) {
				case "North":
					this.y -= 1;
					break;
				case "South":
					this.y += 1;
					break;
				case "West":
					this.x -= 1;
					break;
				case "East":
					this.x += 1;
					break;
				}
				this.lifespan -= 1;
				this.corridor -= 1;
			}
		}
	}

	// Change the direction of the tunneler.
	// The tunneler should never choose a direction opposite its previous one. (ex.: If prev. dir. was South, don't go North).
	// Likewise, the tunneler should never CONTINUE in its current direction. (ex.: If prev. dir. was South, don't go South).
	public string getDirection(string prevDirection = null){
		switch (prevDirection) {
		case "North":
		case "South":
			return direction [rng.Next (2, direction.Length)];
		case "West":
		case "East":
			return direction [rng.Next (0, 2)];		// Remember that Next() goes from [x, y) or [x, y-1]!
		default:
			return "North";
		}
	}
}
