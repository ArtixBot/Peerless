using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// THIS CLASS SHOULD BE USED FOR TESTING PURPOSES RIGHT NOW.
public class ViewBoard : MonoBehaviour {

	public Text board;
	public BoardGenerator genBoard;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		board.text = genBoard.printBoard();
	}
}
