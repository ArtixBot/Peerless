using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor{

	public BoardGenerator gameBoard;	// Necessary, presumably.

	// Any field marked with a * in the comment should only be utilized by enemies.
	public Sprite actorSprite;			// Unit sprite!

	private string id;				// Unit identifier. CRITICAL.
	private string name;			// Unit name.
	private string desc;			// *Unit description.
	private int free_actions;		// Number of Free Actions currently granted to user.
	private int armor;				// *Armor level.
	private int xPos;				// Current x-position of the unit.
	private int yPos;				// Current y-position of the unit.
	private List<Object> inventory;	// Inventory items held by unit.

	public Actor(int xPos, int yPos, string i = "generic", string nm = "Generic Name", int amr = 0){
		this.xPos = xPos;
		this.yPos = yPos;
		id = i;
		name = nm;
		armor = amr;
	}
	
	public string getId(){
		return id;
	}
	public string getName(){
		return name;
	}
	public void setName(string nm){
		name = nm;
	}
	public int getFreeActions(){
		return free_actions;
	}
	public void setFreeActions(int fa){
		free_actions = fa;
	}

	public bool Update(){
		if (Input.GetKeyDown("up")){
			Debug.Log("Up key pressed by " + getName());
			return true;
		}
		return false;
	}
}
