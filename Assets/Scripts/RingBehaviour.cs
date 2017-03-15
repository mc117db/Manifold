using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBehaviour : MonoBehaviour {

	RingData currentRingData;
    [SerializeField]
    GameObject outer, middle, inner;

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
        return false;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
