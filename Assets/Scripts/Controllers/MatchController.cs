using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MatchController : MonoBehaviour {

    // Use this for initialization
    public bool DEV_PREVENTMATCHES;
    public static MatchController instance;
    public delegate void OnEvent();
    public delegate void OnMatchEvent();
    public delegate void OnMatch(int totalNoOfItemsRemoved);
    public delegate void OnMatchData(MatchData matchDataRemoved);
    public delegate void OnSpawnEventType(SpawnType eventtype);
    public static event OnMatchEvent OnMatchEventHappen; 
    public static event OnMatch OnMatchEventTotalItemsRemoved;
    public static event OnMatchData OnMatchEventData;
    public static event OnMatchEvent OnAnomalyMatchEvent;
    public static event OnMatchEvent OnForceDropMatchEvent;
    public static event OnEvent PendingMatchClearedEvent;
    public static event OnSpawnEventType PendingMatchClearedEventType;
    private void OnDestroy()
    {
        OnMatchEventHappen = null;
        OnMatchEventTotalItemsRemoved = null;
        OnMatchEventData = null;
        OnAnomalyMatchEvent = null;
        OnForceDropMatchEvent = null;
        PendingMatchClearedEvent = null;
        PendingMatchClearedEventType = null;
    }
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
        bool anomalyMatchDetected = false;
        bool forceMatchDetected = false;
        //Debug.Log("PENDING COLORS TO REMOVE: "+ pendingDataDictionary.Count+" COLORS!");
        int totalItemsRemoved= 0;
        foreach (ColorIndex colorKey in pendingDataDictionary.Keys)
        {
            //Debug.Log("CLEARING COLORKEY: " + colorKey.ToString());
            for (int i = 0; i < pendingDataDictionary[colorKey].Count;i++)
            {
                if (!anomalyMatchDetected && pendingDataDictionary[colorKey][i].CurrentRingData.spawnType == SpawnType.Anomaly)
                {
                    anomalyMatchDetected = true;
                }
                if (!forceMatchDetected && pendingDataDictionary[colorKey][i].CurrentRingData.spawnType == SpawnType.ForceDrop)
                {
                    forceMatchDetected = true;
                }
                totalItemsRemoved += pendingDataDictionary[colorKey][i].ReturnNumberOfTiersOfColor(colorKey);
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
            if (anomalyMatchDetected)
            {
                Debug.Log("ANOMALY MATCH DETECTED!");
                if (OnAnomalyMatchEvent != null)
                {
                    OnAnomalyMatchEvent();
                }
                if (PendingMatchClearedEventType != null)
                {
                    PendingMatchClearedEventType(SpawnType.Anomaly);
                }
            }
            else if (forceMatchDetected)
            {
                Debug.Log("FORCE MATCH DETECTED!");
                if (OnForceDropMatchEvent != null)
                {
                    OnForceDropMatchEvent();
                }
                if (PendingMatchClearedEventType != null)
                {
                    PendingMatchClearedEventType(SpawnType.ForceDrop);
                }
            }
            else
            {
                if (PendingMatchClearedEventType != null)
                {
                    PendingMatchClearedEventType(SpawnType.Normal);
                }
            }
            if (OnMatchEventTotalItemsRemoved != null)
            {
                OnMatchEventTotalItemsRemoved(totalItemsRemoved); // ScoreController depends on this
            }
            if (OnMatchEventHappen != null)
            {
                OnMatchEventHappen();
            }
        }
        pendingDataDictionary.Clear();
    }
	

}
