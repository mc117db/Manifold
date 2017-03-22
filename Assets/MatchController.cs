using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MatchController : MonoBehaviour {

	// Use this for initialization
    public static MatchController instance;
    public delegate void OnMatch(int totalNoOfItemsRemoved);
    public delegate void OnMatchData(MatchData matchDataRemoved); 
    public event OnMatch OnMatchEventTotalItemsRemoved;
    public event OnMatchData OnMatchEventData;


    private List<MatchData> pendingMatchData = new List<MatchData>();
    public void Awake()
    {
        instance = this;
    }
    void Start () {
		if (instance != this)
        {
            Destroy(this);
        }
	}
    public void StoreMatch (MatchData matchData)
    {
        if (pendingMatchData.Count == 0)
        {
            pendingMatchData.Add(matchData);
        }
        else
        {
            for (int i = 0; i < pendingMatchData.Count; i++)
            {
                if (pendingMatchData[i].colorMark == matchData.colorMark)
                {
                    pendingMatchData[i].markedObjects.AddRange(matchData.markedObjects);
                }
            }
        }
    }
	public void ClearPendingMatches()
    {
        if (pendingMatchData.Count == 0)
        {
            return;
        }
        int totalItemsRemoved= 0;
        for (int i = 0; i < pendingMatchData.Count; i++)
        {
            if (OnMatchEventData != null)
            {
                OnMatchEventData(pendingMatchData[i]);
            }
            for (int j = 0; j < pendingMatchData[i].markedObjects.Count; j++)
            {
                totalItemsRemoved++;
                // Do something on the ring
                pendingMatchData[i].markedObjects[j].RemoveColor(pendingMatchData[i].colorMark);
            }
        }
        if (totalItemsRemoved > 0)
        {
            if (OnMatchEventTotalItemsRemoved != null)
            {
                OnMatchEventTotalItemsRemoved(totalItemsRemoved);
            }
        }
        pendingMatchData.Clear();
    }
	

}
