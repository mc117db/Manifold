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

    private Dictionary<ColorIndex,List<RingBehaviour>> pendingDataDictionary = new Dictionary<ColorIndex,List<RingBehaviour>>();
    //private List<MatchData> pendingMatchData = new List<MatchData>();
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
        if (!pendingDataDictionary.ContainsKey(matchData.colorMark))
        {
            pendingDataDictionary.Add(matchData.colorMark,matchData.markedObjects);
        }
        else
        {
            pendingDataDictionary[matchData.colorMark].AddRange(matchData.markedObjects);
        }
    }
    
	public void ClearPendingMatches()
    {
        Debug.Log("PENDING COLORS TO REMOVE: "+ pendingDataDictionary.Count+" COLORS!");
        int totalItemsRemoved= 0;
        foreach (ColorIndex colorKey in pendingDataDictionary.Keys)
        {
            Debug.Log("CLEARING COLORKEY: " + colorKey.ToString());
            for (int i = 0; i < pendingDataDictionary[colorKey].Count;i++)
            {
                totalItemsRemoved++;
                pendingDataDictionary[colorKey][i].RemoveColor(colorKey);
            }
            MatchData dataToBroadcast = new MatchData();
            dataToBroadcast.colorMark = colorKey;
            dataToBroadcast.markedObjects = pendingDataDictionary[colorKey];
            if (OnMatchEventData != null)
            {
                OnMatchEventData(dataToBroadcast);
            } 
        }
        if (totalItemsRemoved > 0)
        {
            if (OnMatchEventTotalItemsRemoved != null)
            {
                OnMatchEventTotalItemsRemoved(totalItemsRemoved);
            }
        }
        pendingDataDictionary.Clear();
    }
	

}
