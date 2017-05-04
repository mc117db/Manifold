using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBehaviour : MonoBehaviour {
	RingData currentRingData;
    [SerializeField]
    GameObject outer, middle, inner;
    bool isInPlace;
    public delegate void stateChange();
    public event stateChange stateChangeEvent;

    public RingData CurrentRingData
    {
        get
        {
            return currentRingData;
        }

        set
        {
            currentRingData = value;
            PaintRings();
            if (stateChangeEvent != null)
            {
                //stateChangeEvent();
            } 
        }
    }
    public bool IsInPlace{
        get{
            return isInPlace;
        }
        set
        {
            isInPlace = value;
            if(isInPlace && stateChangeEvent != null)
            {
                stateChangeEvent();
            }
        }
    }
    void PaintRings()
    {
        if (!currentRingData.Outer && !currentRingData.Middle && !currentRingData.Inner)
        {
            // TODO Add Pooling implementation later
            //HACK
            transform.parent.GetComponent<RingPointManager>().ToggleReferencePointRenderer(true);
            Destroy(gameObject);
            return;
        }
        if (ColorManager.instance == null)
        {
            Debug.Log("NO COLORMANAGER TO FETCH COLOR DATA FROM!");
        }
        if (!currentRingData.Outer)
        {
            outer.SetActive(false);
        }
        else
        {
            outer.GetComponent<SpriteRenderer>().color = ColorManager.instance.FetchColorInformation(
            currentRingData.ringColors[0]);
            outer.SetActive(true);
        }
        if (!currentRingData.Middle)
        {
            middle.SetActive(false);
        }
        else
        {
            middle.GetComponent<SpriteRenderer>().color = ColorManager.instance.FetchColorInformation(
            currentRingData.ringColors[1]);
            middle.SetActive(true);
        }
        if (!currentRingData.Inner)
        {
            inner.SetActive(false);
        }
        else
        {
            inner.GetComponent<SpriteRenderer>().color = ColorManager.instance.FetchColorInformation(
                currentRingData.ringColors[2]);
            inner.SetActive(true);
        }
      
    }
    public bool CombineRings (RingData other)
	{
        // Return true if can combine, else return false.
        // This function is called on the Ring that is attached to the reference point.
        // not on the currently selected one.
        bool conflict = false;
        RingData newState = currentRingData;
        #region TIER CHECKS
        // OUTER
        if (!currentRingData.Outer)
        {
            newState.Outer = other.Outer;
            newState.ringColors[0] = other.ringColors[0];
        }
        else if (other.Outer)
        {
            conflict = true;
        }
        // MIDDLE
        if (!currentRingData.Middle)
        {
            newState.Middle = other.Middle;
            newState.ringColors[1] = other.ringColors[1];
        }
        else if (other.Middle)
        {
            conflict = true;
        }
        // INNER
        if (!currentRingData.Inner)
        {
            newState.Inner = other.Inner;
            newState.ringColors[2] = other.ringColors[2];
        }
        else if (other.Inner)
        {
            conflict = true;
        }
        #endregion
        if (conflict)
        {
            //Debug.Log("CANNOT COMBINE :<");
            return false;
        }
        else
        {
            //Debug.Log("CAN COMBINE!");
            currentRingData = newState;
            PaintRings();
            stateChangeEvent();
            return true;
        }
    }
    public bool CheckIfCanCombine (RingData other)
    {
        bool conflict = false;
        #region TIER CHECKS
        // OUTER
        if (currentRingData.Outer)
        {
            if(other.Outer)
            {
                conflict = true;
            }
        }
        // MIDDLE
        if (currentRingData.Middle)
        {
            if (other.Middle)
            {
                conflict = true;
            }
        }
     
        // INNER
        if (currentRingData.Inner)
        {
            if (other.Inner)
            {
                conflict = true;
            }
        }
        #endregion
        return conflict;
    }
    public void RemoveColor (ColorIndex index)
    {
        RingData newState = currentRingData;
        for (int i = 0; i < newState.ringColors.Count; i++)
        {
            if (newState.ringColors[i] == index)
            {
                //TODO Add implementation to count color remove over here
                newState.ringColors[i] = ColorIndex.NONE;
            }
        }
        if (newState.ringColors[0]== ColorIndex.NONE)
        {
            newState.Outer = false;
        }
        if (newState.ringColors[1] == ColorIndex.NONE)
        {
            newState.Middle = false;
        }
        if (newState.ringColors[2] == ColorIndex.NONE)
        {
            newState.Inner = false;
        }
        CurrentRingData = newState;
    }
    public void RemoveRing()
    {
        RingData newState = new RingData();
        newState.ringColors = new List<ColorIndex>();
        for (int i = 0; i < 2; i++)
        {
            newState.ringColors.Add(ColorIndex.NONE);
        }
        CurrentRingData = newState;
    }
    public int ReturnNumberOfTiersOfColor(ColorIndex index)
    {
        int noOfTiers = 0;
        foreach (ColorIndex colr in CurrentRingData.ringColors)
        {
            if (colr == index)
            {
                noOfTiers++;
            }
        }
        return noOfTiers;
    }
    void ClearSpawnModifiers (){
        if (currentRingData.spawnType != SpawnType.Normal)
        {
            currentRingData.spawnType = SpawnType.Normal;
        }
    }
	// Use this for initialization
	void Start () {
        MatchController.PendingMatchClearedEvent += ClearSpawnModifiers;
        GameController.NoMatchEvent += ClearSpawnModifiers;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnDestroy()
    {
        stateChangeEvent = null;
    }
}
