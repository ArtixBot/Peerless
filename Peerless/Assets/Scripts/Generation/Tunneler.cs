using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random=System.Random;

public class Tunneler{

	// TUNNELER PARAMETERS
	public RandInt battery = new RandInt(50, 100);		// The tunneler is powered to dig this many tiles before being dismantled for eternity.


	public int x;										// Tunneler x-and-y position (aka rowLength-and-row position).
	public int y;
	public int lifespan;								// How far can the tunneler dig before it's killed off.
	public string[] direction = {"North", "South", "West", "East"};	// The direction the tunneler should dig in. N = [-1][], S = [+1][], W = [][-1], E = [][+1]

	public Tunneler(int x, int y){
		this.x = x;
		this.y = y;
		this.lifespan = battery.random;
		Debug.Log ("Instantiated with lifespan " + this.lifespan);
	}

	// The Tunneler is born! It digs in a certain direction, then rotates, continuing until killed.
	public void Dig(ref Tile[][] board){
		string dir = direction[new Random().Next(0, direction.Length)];
		switch (dir) {
		case "North":
			Debug.Log ("North");
			break;
		case "South":
			Debug.Log ("South");
			break;
		case "West":
			Debug.Log ("West");
			break;
		case "East":
			Debug.Log ("East");
			break;
		}
	}

}
