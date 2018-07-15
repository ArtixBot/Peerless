using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Holy shit, it actually works. I know there's a bug somewhere in here...
public class TurnSystem : MonoBehaviour {

	Actor currentActor;
	Queue<Actor> actionCycle = new Queue<Actor>();
	private bool cycleTurns = false;

	void Start(){
	}

	// Update is called once per frame
	void Update () {
		// Wait for the user to input something.
		if (cycleTurns){			
			if (currentActor.Update()){
				cycleTurns = false;
			}
		}
		// Once the user has input something, move on to the next user.
		else{
			currentActor = actionCycle.Dequeue();
			actionCycle.Enqueue(currentActor);
			cycleTurns = true;
		}
		
	}
}
