using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MatchController : MonoBehaviour {

    // Use this for initialization
    public bool DEV_PREVENTMATCHES;
    public static MatchController instance;
    public delegate void OnMatchEvent();
    public delegate void OnMatch(int totalNoOfItemsRemoved);
    public delegate void OnMatchData(MatchData matchDataRemoved);
    public static event OnMatchEvent OnMatchEventHappen; 
    public static event OnMatch OnMatchEventTotalItemsRemoved;
    public static event OnMatchData OnMatchEventData;

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
        if (DEV_PREVENTMATCHES)
        {
            Debug.LogWarning("DEV: MATCHCONTROLLER IS PREVENTING MATCHES");
        }
        GameController.StartEvent += delegate { pendingDataDictionary.Clear(); };
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
        if (DEV_PREVENTMATCHES)
        {
            return;
        }
        //Debug.Log("PENDING COLORS TO REMOVE: "+ pendingDataDictionary.Count+" COLORS!");
        int totalItemsRemoved= 0;
        foreach (ColorIndex colorKey in pendingDataDictionary.Keys)
        {
            //Debug.Log("CLEARING COLORKEY: " + colorKey.ToString());
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
            if (OnMatchEventHappen != null)
            {
                OnMatchEventHappen();
            }
        }
        pendingDataDictionary.Clear();
    }
	

}
