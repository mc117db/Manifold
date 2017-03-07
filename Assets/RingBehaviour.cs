using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBehaviour : MonoBehaviour {

	RingData currentRingData;

	public bool CombineRings (RingData other)
	{
		if (other.Outer == true && currentRingData.Outer == true)
		{
			return false;
		}
		else if (other.Outer != currentRingData.Outer)
		{

		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
