using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBehaviour : MonoBehaviour {

	RingData currentRingData;
    [SerializeField]
    GameObject outer, middle, inner;
    public bool isInPlace;

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
        }
    }
    void PaintRings()
    {
        if (!currentRingData.Outer)
        {
            outer.SetActive(false);
        }
        else
        {
            outer.GetComponent<SpriteRenderer>().color = currentRingData.ringColors[0];
        }
        if (!currentRingData.Middle)
        {
            middle.SetActive(false);
        }
        else
        {
            middle.GetComponent<SpriteRenderer>().color = currentRingData.ringColors[1];
        }
        if (!currentRingData.Inner)
        {
            inner.SetActive(false);
        }
        else
        {
            inner.GetComponent<SpriteRenderer>().color = currentRingData.ringColors[2];
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
            return true;
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
