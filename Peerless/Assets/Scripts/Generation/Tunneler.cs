using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random=System.Random;

// The tunneler digs corridors. For room tunneler adjustments, check RoomTunneler.cs.
public class Tunneler{

	// TUNNELER PARAMETERS
	public const int BUFFER = 4;							// The tunneler will not dig tunnels within {BUFFER} tiles of the board's edge.
	public const int CHANCE_INTERSECTION = 25;				// % chance to dig out an intersection (5x5 area) after corridor completion. If this occurs another tunneler is spawned!
	public const double NEW_TUNNELER_LIFESPAN = 0.5;		// Any child tunnelers have (multiplicative) {NEW_TUNNELER_LIFESPAN} of their parent's lifespan.
	public const float OPTIMAL_RATIO = 0.55f;				// Any possible tunneling direction which has >={OPTIMAL_RATIO}% tiles already dug out in the path will not be included.
	public const int CHANCE_SPAWN_ROOM_TUNNELER = 3;		// Every time a tile is successfully dug out there is a {CHANCE_SPAWN_ROOM_TUNNELER}% chance to add room tunneler spawn coordinates to the board generator.
	public RandInt battery = new RandInt(300, 350);			// The tunneler is powered to dig {battery} tiles before being dismantled for eternity.
	public RandInt corridorLength = new RandInt(8, 16);		// The tunneler digs straight for {corridorLength} tiles before changing directions.

	// Tunneler properties, do not modify.
	public static Random rng = new Random ();
	public char[] direction = {'N', 'S', 'W', 'E'};		// The direction the tunneler should dig in. N = [-1][], S = [+1][], W = [][-1], E = [][+1]

	public int x;										// Tunneler x-and-y position (aka rowLength-and-row position).
	public int y;
	public int lifespan;								// How far can the tunneler dig before it's killed off.
	public int corridorLen;								// Number of turns the tunneler digs in a direction.
	public char curDirection;							// Current direction of the tunneler.

	public Tunneler(int x, int y){
		this.x = x;		// The tunneler's coordinates are provided via (x, y) but the tunneler itself moves via (y, x) coordinates.
		this.y = y;
		this.lifespan = battery.random;
		this.corridorLen = corridorLength.random;
		this.curDirection = direction [rng.Next (0, direction.Length)];
	}

	public Tunneler(int x, int y, int lifespan){
		this.x = x;		// The tunneler's coordinates are provided via (x, y) but the tunneler itself moves via (y, x) coordinates.
		this.y = y;
		this.lifespan = lifespan;
		this.corridorLen = corridorLength.random;
		this.curDirection = direction [rng.Next (0, direction.Length)];
	}

	// The Tunneler is born! It digs in a certain direction, then rotates, continuing until killed.
	// The specific schedule for this Dig() is as follows:
	// Dig/DigArea -> GetDirection -> DigArea.
	public void Dig(ref Tile[][] board){
		while (this.lifespan > 0) {
			this.corridorLen = corridorLength.random;
			curDirection = GetDirection (this.corridorLen, board[0].Length, board.Length, curDirection, board);
			if (curDirection == 'X') {			// If no optimal direction was found, terminate dig early.
				this.lifespan = 0;
				break;
			}
			while (this.corridorLen > 0) {
				DigArea (ref board, this.y, this.x, 1);
				switch (curDirection) {
				case 'N':
					this.y -= 1;
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
				this.lifespan -= 1;
				this.corridorLen -= 1;
			}
			if (rng.Next (0, 100) < CHANCE_INTERSECTION) {		// {CHANCE_INTERSECTION}% chance of generating an intersection after corridor complete.
				DigArea (ref board, this.y, this.x, 2);
			}
		}
		int[] coordinates = { this.x, this.y };

		BoardGenerator.RoomDiggers.Add (coordinates);		// Attempt to end a tunneler with a room by adding coordinates for a roomtunneler.
	}

	// Dig out a (dimension * 2 + 1) x (dimension * 2 + 1) area from current position.
	// dimensions: 0 = 1x1 area
	// dimensions: 1 = 3x3 area
	// dimensions: 2 = 5x5 area
	// dimensions: 3 = 7x7 area
	public void DigArea(ref Tile[][] board, int curY, int curX, int dimensions){
		for (int i = -dimensions; i < dimensions + 1; i++) {
			for (int j = -dimensions; j < dimensions + 1; j++) {
				if (board [curY + j] [curX + i].property == Tile.TileState.IS_WALL) {
					board [curY + j] [curX + i].property = Tile.TileState.IS_FLOOR;
					if (rng.Next (0, 100) < CHANCE_SPAWN_ROOM_TUNNELER) {
						BoardGenerator.RoomDiggers.Add (new int[] { this.x, this.y });
					}
				}
			}
		}
		if (dimensions >= 2) {
			// Newly spawned tunnelers have 0.5x lifespan compared to their parent's lifespan.
			Tunneler babyTunneler = new Tunneler (this.x, this.y, (int)Math.Round(this.lifespan * NEW_TUNNELER_LIFESPAN));		
			babyTunneler.Dig (ref board);
			BoardGenerator.RoomDiggers.Add (new int[] {this.x, this.y});
		}
	}
		
	// Change the direction of the tunneler.
	// The tunneler should never choose a direction opposite its previous one. (ex.: If prev. dir. was S, don't go N).
	// The direction the tunneler chooses is based on the number of already-dug spaces (minimizing those) and board boundaries (completely avoids those).
	public char GetDirection(int length, int limitX, int limitY, char x, Tile[][] board){
		List<char> dirArray = new List<char>();
		switch (x) {
		case 'N':
			if (this.y - length >= BUFFER) {
				if (DigRatio (board, length, 'N') < OPTIMAL_RATIO) { dirArray.Add ('N'); }
			}
			if (this.x - length >= BUFFER) {
				if (DigRatio (board, length, 'W') < OPTIMAL_RATIO) { dirArray.Add ('W'); }
			}
			if (this.x + length < limitX - BUFFER) {
				if (DigRatio (board, length, 'E') < OPTIMAL_RATIO) { dirArray.Add ('E'); }
			}
			break;
		case 'S':
			if (this.y + length < limitY - BUFFER) {
				if (DigRatio (board, length, 'S') < OPTIMAL_RATIO) { dirArray.Add ('S'); }
			}
			if (this.x - length >= BUFFER) {
				if (DigRatio (board, length, 'W') < OPTIMAL_RATIO) { dirArray.Add ('W'); }
			}
			if (this.x + length < limitX - BUFFER) {
				if (DigRatio (board, length, 'E') < OPTIMAL_RATIO) { dirArray.Add ('E'); }
			}
			break;
		case 'W':
			if (this.y - length >= BUFFER) {
				if (DigRatio (board, length, 'N') < OPTIMAL_RATIO) { dirArray.Add ('N'); }
			}
			if (this.y + length < limitY - BUFFER) {
				if (DigRatio (board, length, 'S') < OPTIMAL_RATIO) { dirArray.Add ('S'); }
			}
			if (this.x - length >= BUFFER) {
				if (DigRatio (board, length, 'W') < OPTIMAL_RATIO) { dirArray.Add ('W'); }
			}
			break;
		case 'E':
			if (this.y - length >= BUFFER) {
				if (DigRatio (board, length, 'N') < OPTIMAL_RATIO) { dirArray.Add ('N'); }
			}
			if (this.y + length < limitY - BUFFER) {
				if (DigRatio (board, length, 'S') < OPTIMAL_RATIO) { dirArray.Add ('S'); }
			}
			if (this.x + length < limitX - BUFFER) {
				if (DigRatio (board, length, 'E') < OPTIMAL_RATIO) { dirArray.Add ('E'); }
			}
			break;
		}
		direction = dirArray.ToArray ();
		if (direction.Length == 0) {
			return 'X';
		}
		return direction[rng.Next(0, direction.Length)];
	}

	// Given a direction, read through the relevant board sections and returns a ratio of already-dug tiles divided by direction length.
	// findApplicableDirection() will compare this ratio to a provided OPTIMAL_RATIO parameter to determine if this corridor direction should be chosen.
	public double DigRatio(Tile[][] board, int corridorLength, char direction){
		float occupiedTiles = 0.0f;
		switch(direction){
		case 'N':
			for (int i = 0; i < corridorLength; i++) {
				if (board [this.y - i] [this.x].property == Tile.TileState.IS_FLOOR) {occupiedTiles += 1.0f;}
			}
			break;
		case 'S':
			for (int i = 0; i < corridorLength; i++) {
				if (board [this.y + i] [this.x].property == Tile.TileState.IS_FLOOR) {occupiedTiles += 1.0f;}
			}
			break;
		case 'W':
			for (int i = 0; i < corridorLength; i++) {
				if (board [this.y] [this.x - i].property == Tile.TileState.IS_FLOOR) {occupiedTiles += 1.0f;}
			}
			break;
		case 'E':
			for (int i = 0; i < corridorLength; i++) {
				if (board [this.y] [this.x + i].property == Tile.TileState.IS_FLOOR) {occupiedTiles += 1.0f;}
			}
			break;
		}
		return occupiedTiles / corridorLength;
	}
}