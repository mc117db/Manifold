using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameState { Running, Lose, Paused }
public class GameController : MonoBehaviour {

    public static GameController instance;
    #region PROPERTIES
    [Header("DEPENDECIES")]
    public RingFactory RingFactoryComponent;
    public ColorManager ColorManagerComponent;
    public ScoreController ScoreControllerComponent;
    [Space(20)]
    [Header("GAME STATE")]
    public int setsRefreshed = 0;
    public int setsToNextLevel = 5;
    private int setsNextLevelIntial;
    public int currentLevel = 1;
    private List<RingData> RingsInStagingArea = new List<RingData>();
    [Space(20)]
    [SerializeField]
    private GameState gameState;
    [Space(20)]
    [Header("COUNTDOWN SETTINGS")]
    public float MaxCountdownTime = 30f;
    private float remainingCountdownTime;
    private float remainingTimeLerpVal;
    public float additionalTimePerMatch = 10f;
    public float additionalTimePerCombo = 5f;

    private ReferencePointBehaviour targetReferencePointToSpawnWhenStagingSetUpdate;
    private RingData ringDataToSpawnWhenStagingSetUpdate;
    #endregion

    #region ACCESSORS
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
    public ReferencePointBehaviour TargetReferencePointToSpawnWhenStagingSetUpdate
    {
        set
        {
            targetReferencePointToSpawnWhenStagingSetUpdate = value;
            if (TargetReferencePointToSpawnWhenStagingSetUpdateChange != null)
            {
                TargetReferencePointToSpawnWhenStagingSetUpdateChange(targetReferencePointToSpawnWhenStagingSetUpdate);
            }
        }
    }
    public RingData RingDataToSpawnWhenStagingSetUpdate
    {
        get
        {
            return ringDataToSpawnWhenStagingSetUpdate;
        }

        set
        {
            ringDataToSpawnWhenStagingSetUpdate = value;
            if (RingDataToSpawnWhenStagingSetUpdateChange != null)
            {
                RingDataToSpawnWhenStagingSetUpdateChange(ringDataToSpawnWhenStagingSetUpdate);
            }
        }
    }

    public GameState GAMESTATE
    {
        get
        {
            return gameState;
        }

        set
        {
            gameState = value;
            if (gameState == GameState.Lose)
            {
                if (LoseEvent != null)
                {
                    LoseEvent();
                }
            }
            if (GameStateChange != null)
            {
                GameStateChange(gameState);
            }
        }
    } 
    #endregion

    #region Events
    public delegate void OnLerpChange(float lerpVal);
    public delegate void OnEvent();
    public delegate void OnReferencePoint(ReferencePointBehaviour point);
    public delegate void OnRingData(RingData data);
    public delegate void OnGameState(GameState state);
    public static event OnEvent StartEvent;
    public static event OnEvent LoseEvent;
    public static event OnEvent CountDownOverEvent;
    public static event OnEvent AnomalyEventSuccess;
    public static event OnEvent AnomalyEventFail;
    public static event OnLerpChange CountdownLerpEvent;
    public static event OnReferencePoint TargetReferencePointToSpawnWhenStagingSetUpdateChange;
    public static event OnRingData RingDataToSpawnWhenStagingSetUpdateChange;
    public static event OnGameState GameStateChange;
    #endregion

    private void Awake()
    {
        instance = this;
    }
    void Start () {
        
        // Register to events
		RingFactory.onRefreshSetEvent += onAnomalyEvent;
		RingFactory.onRefreshSetEvent += AdvanceGameState;
        RingFactory.onStagingSetUpdateEvent += LoseStateCheck;
        RingFactory.onStagingSetUpdateEvent += onStagingSetUpdate;

        // Countdown Events Dependecies
        MatchController.OnMatchEventHappen += OnMatchAddTime;
        ScoreController.ComboIncreaseEvent += OnComboAddTime;

        CountDownOverEvent += ForcePlayStagingSet; // Internal method
        setsNextLevelIntial = setsToNextLevel;
        StartGame();

	}
    public void StartGame ()
    {
            RemainingCountdownTime = MaxCountdownTime;
            setsToNextLevel = setsNextLevelIntial;
            ColorManagerComponent.CurrentLevel = 1;
            RingFactoryComponent.CreateNewSet();
            ScoreControllerComponent.Reset();
            RemoveAllRings();
            if (StartEvent != null)
            {
            StartEvent();
            }
            GAMESTATE = GameState.Running;
    }
    public void TogglePause ()
    {
        if (GAMESTATE != GameState.Paused)
        {
            GAMESTATE = GameState.Paused;
            Debug.Log("GAMEACTION: PAUSE");
        }
        else
        {
            GAMESTATE = GameState.Running;
            Debug.Log("GAMEACTION: RESUME");
        }
    } 
    public void onStagingSetUpdate(List<RingData> listOfRingsInStagingSet)
    {
        RingsInStagingArea.Clear();
        RingsInStagingArea = listOfRingsInStagingSet;
    }
    public void onAnomalyEvent()
    {
        if (targetReferencePointToSpawnWhenStagingSetUpdate == null)
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
                TargetReferencePointToSpawnWhenStagingSetUpdate = availablePoints[Random.Range(0, availablePoints.Count - 1)];
            }
            else
            {
                TargetReferencePointToSpawnWhenStagingSetUpdateChange = null;
                return;
            }
            RingDataToSpawnWhenStagingSetUpdate = RingFactoryComponent.GenerateNewRingData();
        }
        else
        {
            if (!targetReferencePointToSpawnWhenStagingSetUpdate.HaveRing())
            {
                RingFactoryComponent.SpawnRingDataAtPoint(RingDataToSpawnWhenStagingSetUpdate, targetReferencePointToSpawnWhenStagingSetUpdate);
                if (AnomalyEventSuccess != null)
                {
                    AnomalyEventSuccess();
                }
            }
            else
            {
                if (AnomalyEventFail != null)
                {
                    AnomalyEventFail();
                }
            }
            TargetReferencePointToSpawnWhenStagingSetUpdate = null;
            RingDataToSpawnWhenStagingSetUpdate = new RingData();
        }
    }
    #region Spawning Implementation 
    public void SpawnRandomRingAtRandomPoint()
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
                RingFactoryComponent.SpawnRandomRingAtPoint(availablePoints[Random.Range(0, availablePoints.Count - 1)]);
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
                RingFactoryComponent.SpawnRingDataAtPoint(ringData, availablePoints[Random.Range(0, availablePoints.Count - 1)]);
            }
        }
        else
        {
            Lose();
        }
    } 
    #endregion

    public void ForcePlayStagingSet()
    {
        Debug.Log("GAME ACTION: Force play rings in staging area"+" TOTAL: "+RingsInStagingArea.Count);
        for (int i = 0; i < RingsInStagingArea.Count; i++)
        {
            if (GAMESTATE == GameState.Running)
            {
                SpawnRingDataAtRandomPoint(RingsInStagingArea[i]);
            }
        }
        RingFactoryComponent.CreateNewSet();
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
    public void LoseStateCheck(List<RingData> ringDataInStagingSet)
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
        Lose();
    }
    private void Lose()
    {
        if (GAMESTATE != GameState.Lose)
        {
            Debug.LogWarning("GAMEACTION: LOSE!");
            GAMESTATE = GameState.Lose;
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
        if (GAMESTATE == GameState.Running)
        {
            DoCountdown();
        }
	}
}
