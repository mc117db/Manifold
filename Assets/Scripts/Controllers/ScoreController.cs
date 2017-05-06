using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScoreImplementationData
{
	[SerializeField]
	public int ringValue = 5;
	[SerializeField]
	public float baseComboMultiplier = 1.5f;
	[SerializeField]
	public float compoundComboMultiplier = 1.5f;
    public float anomalyMatchEventScoreMultiplier = 2f;
    public float forceMatchEventScoreMultiplier = 0.5f;
    private float currentScoreMultiplier = 1f;

	public ScoreImplementationData(int ringVal,float baseMult,float compMult)
	{
		ringValue = ringVal;
		baseComboMultiplier = baseMult;
		compoundComboMultiplier = compMult;
	}
	public ScoreImplementationData ()
	{
		ringValue = 5;
		baseComboMultiplier = 1.5f;
		compoundComboMultiplier = 1.5f;
	}
	public int ModifyScore(int ringConsumed,int curCombo)
	{
		int baseScore =  ringValue*ringConsumed;
        int finalScore = 0;
		if (curCombo > 1)
		{
            finalScore = Mathf.FloorToInt(baseScore * (baseComboMultiplier + compoundComboMultiplier*(curCombo-1)));
		}
		else
		{
            finalScore = baseScore; // No Combo
		}
        return Mathf.RoundToInt(finalScore * currentScoreMultiplier);
	}
    public void ModifyScoreMultiplierOnEvent (SpawnType eventtype)
    {
        switch (eventtype)
        {
            case SpawnType.Normal:
                currentScoreMultiplier = 1f;
                break;
            case SpawnType.Anomaly:
                currentScoreMultiplier = anomalyMatchEventScoreMultiplier;
                break;
            case SpawnType.ForceDrop:
                currentScoreMultiplier = forceMatchEventScoreMultiplier;
                break;
        }
    }
}


public class ScoreController : MonoBehaviour {

	private int currentScore = 0;
    private int currentComboNumber = 0;
    public int CurrentScore{
		get {
			return currentScore;
		}
		set{
			currentScore = value;
            if (ScoreValueUpdateEvent != null)
            {
                ScoreValueUpdateEvent(currentScore);
            }
			if (ScoreUpdateEvent != null)
			{
				ScoreUpdateEvent();
			}
            if (PlayerPrefs.HasKey("Local_HighScore") && !highScoreBeatCurrentRun)
            {
                if (currentScore > PlayerPrefs.GetInt("Local_HighScore"))
                {
                    highScoreBeatCurrentRun = true;
                    if (HighScoreBeatDuringCurrentRun != null)
                    {
                        HighScoreBeatDuringCurrentRun();
                    }
                }
            }
        }
	}
    public int CurrentComboNumber{
		get {
			return currentComboNumber;
		}
		set{
			currentComboNumber = value;
			if (ComboUpdateEvent != null)
			{
				ComboUpdateEvent(currentComboNumber);
			}
            if (ComboIncreaseEvent != null)
            {
                if (currentComboNumber > 0)
                {
                    ComboIncreaseEvent();
                }
            }
		}
	}

    public bool matchLastTurn;
    public bool highScoreBeatCurrentRun;

    private ScoreImplementationData currentScoreImplementation = new ScoreImplementationData(5,1.5f,1.5f);
	public static ScoreController instance;

    #region Events
    public delegate void OnUpdateEvent();
    public delegate void OnUpdateEventReturnInt(int valueToReturn);

    public static event OnUpdateEventReturnInt ScoreValueUpdateEvent;
    public static event OnUpdateEvent ScoreUpdateEvent;
    public static event OnUpdateEventReturnInt ComboUpdateEvent;
    public static event OnUpdateEvent ComboIncreaseEvent;
    public static event OnUpdateEvent HighScoreUpdateEvent;
    public static event OnUpdateEvent HighScoreBeatDuringCurrentRun;
    public static event OnUpdateEvent ComboResetEvent;
    #endregion
    // Use this for initialization
    private void OnDestroy()
    {
        instance = null;
        ScoreValueUpdateEvent = null;
        ScoreUpdateEvent = null;
        ComboUpdateEvent = null;
        ComboIncreaseEvent = null;
        HighScoreUpdateEvent = null;
        HighScoreBeatDuringCurrentRun = null;
        ComboResetEvent = null;
    }
    void Awake()
	{
		instance = this;
	}
	void Start () {
		if (instance!=this)
		{
			Debug.Log("You cannot have more than one ScoreManager!");
			Destroy(this);
		}
        SceneController.CleanUp += OnDestroy;
		MatchController.OnMatchEventTotalItemsRemoved += RingsConsumed;

        SceneController.CleanUp += StoreHighScore;
        GameController.LoseEvent += StoreHighScore;

        MatchController.PendingMatchClearedEventType += ModifyScoreMultiplier;
        MatchController.PendingMatchClearedEvent += ResetModifier;
	}
    void ModifyScoreMultiplier(SpawnType eventtype)
    {
        currentScoreImplementation.ModifyScoreMultiplierOnEvent(eventtype);
    }
    void ResetModifier()
    {
        currentScoreImplementation.ModifyScoreMultiplierOnEvent(SpawnType.Normal);
    }
	public void RingsConsumed(int totalRingsConsumed)
	{
		if (currentScoreImplementation != null)
		{
			if (matchLastTurn)
			{
				CurrentComboNumber++;
			}
			CurrentScore += currentScoreImplementation.ModifyScore(totalRingsConsumed,CurrentComboNumber);
			matchLastTurn = true;
		}
		else
		{
			Debug.Log("SCORE CONTROLLER DOESN'T HAVE A ScoreImplementationData ASSIGNED!");
		}	
	}
    void StoreHighScore()
    {
        if (PlayerPrefs.HasKey("Local_HighScore"))
        {
            if (currentScore > PlayerPrefs.GetInt("Local_HighScore"))
            {
                PlayerPrefs.SetInt("Local_HighScore", currentScore);
                if (HighScoreUpdateEvent != null)
                {
                    HighScoreUpdateEvent();
                }
            }
        }
        else
        {
            PlayerPrefs.SetInt("Local_HighScore", currentScore);
            if (HighScoreUpdateEvent != null)
            {
                HighScoreUpdateEvent();
            }
        }
    }
    void ClearHighScore()
    {
        Debug.LogWarning("DEVACTION: CLEARING HIGHSCORE");
        PlayerPrefs.DeleteKey("Local_HighScore");
    }
	public void ResetCombo()
	{
		//Debug.Log("RESET COMBO");
        // REFERENCEPOINTBEHAVIOUR IS RESETTING THE COMBO (LINE:157)
        if (ComboResetEvent != null)
        {
            ComboResetEvent();
        }
		matchLastTurn = false;
		CurrentComboNumber = 0;
	}
	public void Reset()
	{
		Debug.Log("RESETTING SCORE CONTROLLER");
		CurrentScore = 0;
		CurrentComboNumber = 0;
		matchLastTurn = false;
        highScoreBeatCurrentRun = false;
	}
    public void Continue()
    {
        CurrentComboNumber = 0;
        matchLastTurn = false;
        highScoreBeatCurrentRun = false;
    }
	// Update is called once per frame

}
