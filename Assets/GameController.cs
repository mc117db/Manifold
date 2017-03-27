using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{

}

public class GameController : MonoBehaviour {

	public RingFactory RingFactoryComponent;
	public ColorManager ColorManagerComponent;
	public int setsRefreshed = 0;
	public int setsToNextLevel = 3;
	public int currentLevel = 1;
	void Start () {
		RingFactory.onRefreshSetEvent += SpawnRandomRingAtRandomPoint;
		RingFactory.onRefreshSetEvent += AdvanceGameState;
	}
	public void SpawnRandomRingAtRandomPoint ()
	{
		Debug.Log("GAME ACTION: Spawning Random Ring at Random Point");
		List<ReferencePointBehaviour> availablePoints = new List<ReferencePointBehaviour>();
		foreach (ReferencePointBehaviour node in TransformationGrid.NODES)
		{
			//Debug.Log("CHECK");
			if (!node.HaveRing())
			{
				//Debug.Log("ADD NODE");
				availablePoints.Add(node);
			}
		}
		if (availablePoints.Count > 0)
		{
			if (RingFactoryComponent != null)
			{
				Debug.Log("Available Points: "+availablePoints.Count);
				RingFactoryComponent.SpawnRandomRingAtPoint(availablePoints[Random.Range(0,availablePoints.Count-1)]);
			}
		}
	}
	public void AdvanceGameState()
	{
		setsRefreshed++;
		setsToNextLevel--;
		if (setsToNextLevel <= 0)
		{
			if (currentLevel-1 < 3)
			{
				Debug.Log("ADVANCING LEVEL");
				ColorManagerComponent.CurrentLevel++;
				setsToNextLevel = 3;
			}
			
		}
	}
	// Use this for initialization
	
	
	// Update is called once per frame
	void Update () {
		
	}
}
