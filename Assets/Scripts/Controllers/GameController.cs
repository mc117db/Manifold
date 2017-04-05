using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{

}

public class GameController : MonoBehaviour {

    [Header("DEPENDECIES")]
	public RingFactory RingFactoryComponent;
	public ColorManager ColorManagerComponent;
    [Space(20)]
    [Header("GAME STATE")]
    public int setsRefreshed = 0;
	public int setsToNextLevel = 5;
    private int setsNextLevelIntial;
	public int currentLevel = 1;
    private List<RingData> RingsInStagingArea = new List<RingData>();
    [Space(20)]
    [Header("COUNTDOWN SETTINGS")]
    public float MaxCountdownTime = 30f;
    private float remainingCountdownTime;
    private float remainingTimeLerpVal;
    public float additionalTimePerMatch = 10f;
    public float additionalTimePerCombo = 5f;

    public float RemainingCountdownTime
    {
        get
        {
            return remainingCountdownTime;
        }

        set
        {
            remainingCountdownTime = value;
            RemainingTimeLerpVal = Mathf.InverseLerp(0, MaxCountdownTime, RemainingCountdownTime);
        }
    }

    public float RemainingTimeLerpVal
    {
        get
        {
            return remainingTimeLerpVal;
        }

        set
        {
            remainingTimeLerpVal = value;
            if (CountdownLerpEvent != null)
            {
                CountdownLerpEvent(remainingTimeLerpVal);
            }
        }
    }

    public delegate void OnLerpChange(float lerpVal);
    public delegate void OnEvent();
    public static event OnEvent LoseEvent;
    public static event OnEvent CountDownOverEvent;
    public static event OnLerpChange CountdownLerpEvent;

	void Start () {
        
        // Register to events
		RingFactory.onRefreshSetEvent += SpawnRandomRingAtRandomPoint;
		RingFactory.onRefreshSetEvent += AdvanceGameState;
        RingFactory.onStagingSetUpdateEvent += LoseStateCheck;
        RingFactory.onStagingSetUpdateEvent += onStagingSetUpdate;

        // Countdown Events Dependecies
        MatchController.OnMatchEventHappen += OnMatchAddTime;
        ScoreController.ComboIncreaseEvent += OnComboAddTime;

        CountDownOverEvent += ForcePlayStagingSet; // Internal method
        Restart();

	}
    void Restart ()
    {
        RemainingCountdownTime = MaxCountdownTime;
        setsNextLevelIntial = setsToNextLevel;
        RingFactoryComponent.CreateNewSet();
    }
    public void onStagingSetUpdate(List<RingData> listOfRingsInStagingSet)
    {
        RingsInStagingArea = listOfRingsInStagingSet;
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
            //Debug.Log("NO MORE SLOTS TO SPAWN");
        }
	}

    public void SpawnRingDataAtRandomPoint(RingData ringData)
    {
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
				RingFactoryComponent.SpawnRingDataAtPoint(ringData, availablePoints[Random.Range(0,availablePoints.Count-1)]);
			}
		}
        else
        {
            Debug.Log("LOSE!");
        }
    }

    public void ForcePlayStagingSet()
    {
        Debug.Log("GAME ACTION: Force play rings in staging area"+" TOTAL: "+RingsInStagingArea.Count);
        for (int i = 0; i < RingsInStagingArea.Count; i++)
        {
            SpawnRingDataAtRandomPoint(RingsInStagingArea[i]);
        }
        RingFactoryComponent.CreateNewSet();
    }

    public void LoseStateCheck (List<RingData> ringDataInStagingSet)
    {
        //Debug.Log("CHECKING IF LOSE");
        foreach (ReferencePointBehaviour node in TransformationGrid.NODES)
        {
            if (!node.HaveRing())
            {
                //Debug.Log("LOSESTATECHECK: THERE IS A POINT STILL OPEN");
                return; // There is still available space in the grid, get out of state check
            }
            else
            {
                foreach (RingData ringData in ringDataInStagingSet)
                {
                    if (node.GetComponent<RingPointManager>().CheckPointIfCanAccept(ringData))
                    {
                        //Debug.Log("LOSESTATECHECK: THERE IS A RING STILL CANCOMBINE");
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

    #region Countdown Implementation
    void DoCountdown()
    {
        RemainingCountdownTime -= Time.deltaTime;
        if (RemainingCountdownTime <= 0)
        {
            if (CountDownOverEvent != null)
            {
                Debug.Log("GAME EVENT: COUNTDOWN OVER");
                CountDownOverEvent();
            }
            RemainingCountdownTime = MaxCountdownTime;
        }
    }
    void OnMatchAddTime()
    {
        AddAdditiontalTime(additionalTimePerMatch);
    }
    void OnComboAddTime()
    {
        AddAdditiontalTime(additionalTimePerCombo);
    }
    void AddAdditiontalTime(float time)
    {
        Debug.Log("GAME ACTION: Additional Time has been granted");
        RemainingCountdownTime = Mathf.Clamp(RemainingCountdownTime + time, 0, MaxCountdownTime);
    } 
    #endregion

    // Update is called once per frame
    void Update () {
        DoCountdown();
	}
}
