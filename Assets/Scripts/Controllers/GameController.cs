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
	public int setsToNextLevel = 5;
    private int setsNextLevelIntial;
	public int currentLevel = 1;
    public delegate void onLoseEvent();
    public static event onLoseEvent LoseEvent;

	void Start () {
        setsNextLevelIntial = setsToNextLevel;
        // Register to events
		RingFactory.onRefreshSetEvent += SpawnRandomRingAtRandomPoint;
		RingFactory.onRefreshSetEvent += AdvanceGameState;
        RingFactory.onStagingSetUpdateEvent += LoseStateCheck;
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
				//Debug.Log("Available Points: "+availablePoints.Count);
				RingFactoryComponent.SpawnRandomRingAtPoint(availablePoints[Random.Range(0,availablePoints.Count-1)]);
			}
		}
        else
        {
            Debug.Log("NO MORE SLOTS TO SPAWN");
        }
	}
    public void LoseStateCheck (List<RingData> ringDataInStagingSet)
    {
        Debug.Log("CHECKING IF LOSE");
        foreach (ReferencePointBehaviour node in TransformationGrid.NODES)
        {
            if (!node.HaveRing())
            {
                Debug.Log("LOSESTATECHECK: THERE IS A POINT STILL OPEN");
                return; // There is still available space in the grid, get out of state check
            }
            else
            {
                foreach (RingData ringData in ringDataInStagingSet)
                {
                    if (node.GetComponent<RingPointManager>().CheckPointIfCanAccept(ringData))
                    {
                        Debug.Log("LOSESTATECHECK: THERE IS A RING STILL CANCOMBINE");
                        return;
                    }
                }
            }
        }
        Debug.Log("YOU LOSE!");

    }
    public void RemoveAllRings ()
    {
        Debug.Log("GAME ACTION: Removing all rings from grid");
        foreach (ReferencePointBehaviour node in TransformationGrid.NODES)
        {
            //Debug.Log("CHECK");
            if (node.HaveRing())
            {
                //Debug.Log("ADD NODE");
                node.GetComponent<RingPointManager>().RemoveRing();
            }
        }
    }
	public void AdvanceGameState()
	{
		setsRefreshed++;
		setsToNextLevel--;
		if (setsToNextLevel <= 0)
		{
				Debug.Log("ADVANCING LEVEL");
				ColorManagerComponent.CurrentLevel++;
				setsToNextLevel = setsNextLevelIntial;
		}
	}
	// Use this for initialization
	
	
	// Update is called once per frame
	void Update () {
		
	}
}
