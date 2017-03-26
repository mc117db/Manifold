using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public RingFactory RingFactoryComponent;
	public void SpawnRandomRingAtRandomPoint ()
	{
		Debug.Log("GAME ACTION: Spawning Random Ring at Random Point");
		List<ReferencePointBehaviour> availablePoints = new List<ReferencePointBehaviour>();
		foreach (ReferencePointBehaviour node in TransformationGrid.NODES)
		{
			//Debug.Log("CHECK");
			if (!node.HaveRing())
			{
				Debug.Log("ADD NODE");
				availablePoints.Add(node);
			}
		}
		if (availablePoints.Count > 0)
		{
			if (RingFactoryComponent != null)
			{
				Debug.Log("SPAWNING!");
				RingFactoryComponent.SpawnRandomRingAtPoint(availablePoints[Random.Range(0,availablePoints.Count-1)]);
			}
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
