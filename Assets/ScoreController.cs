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
		if (curCombo > 1)
		{
			return Mathf.FloorToInt(baseScore * (baseComboMultiplier + compoundComboMultiplier*(curCombo-1)));
		}
		else
		{
			return baseScore; // No Combo
		}
	}
}


public class ScoreController : MonoBehaviour {

	private int currentScore = 0;
	public int CurrentScore{
		get {
			return currentScore;
		}
		set{
			currentScore = value;
			if (ScoreUpdateEvent != null)
			{
				ScoreUpdateEvent();
			}
		}
	}
	public bool matchLastTurn;
	private int currentComboNumber = 0;
	public int CurrentComboNumber{
		get {
			return currentComboNumber;
		}
		set{
			currentComboNumber = value;
			if (ComboUpdateEvent != null)
			{
				ComboUpdateEvent();
			}
		}
	}
	private ScoreImplementationData currentScoreImplementation = new ScoreImplementationData(5,1.5f,1.5f);
	public static ScoreController instance;

	public delegate void OnUpdateEvent ();
	public event OnUpdateEvent ScoreUpdateEvent; //TODO HOOK UP TO UI ELEMENT LATER
	public event OnUpdateEvent ComboUpdateEvent;
	// Use this for initialization
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
		MatchController.instance.OnMatchEventTotalItemsRemoved += RingsConsumed;
	}
	void RingsConsumed(int totalRingsConsumed)
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
	public void ResetCombo()
	{
		Debug.Log("RESET COMBO");
		matchLastTurn = false;
		CurrentComboNumber = 0;
	}

	void Reset()
	{
		Debug.Log("RESETTING SCORE CONTROLLER");
		CurrentScore = 0;
		CurrentComboNumber = 0;
		matchLastTurn = false;
	}
	// Update is called once per frame

}
