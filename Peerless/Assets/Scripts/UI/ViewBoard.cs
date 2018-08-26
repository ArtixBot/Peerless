using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// THIS CLASS SHOULD BE USED FOR TESTING PURPOSES RIGHT NOW.
public class ViewBoard : MonoBehaviour {

	public Text boardText;
	public BoardGenerator boardGen;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		boardText.text = boardGen.PrintBoard();
	}
}
