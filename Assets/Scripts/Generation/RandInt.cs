using System;

public class RandInt{

	public int randMin;
	public int randMax;

	public RandInt(int min, int max){
		randMin = min;
		randMax = max;
	}

	public int random{
		get{ return UnityEngine.Random.Range(randMin, randMax); }
	}
}
