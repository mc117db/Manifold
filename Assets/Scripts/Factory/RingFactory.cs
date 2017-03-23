using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingFactory : MonoBehaviour {

    public GameObject RingPrefab;
    public float horizontalOffset = 2f;
    public float middleUpperOffset = 3f;
    public delegate void onRefreshSet();
    public static event onRefreshSet onRefreshSetEvent;
    // Use this for initialization
    [HideInInspector]
    public int ringsInDock = 3;
	void Start () {
        RingPointManager.RingDropEvent += UpdateState;
        CreateNewSet(); //TODO This will be called by the GameManager later
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateState()
    {
        ringsInDock = transform.childCount;
        if (ringsInDock <= 0)
        {
            if (onRefreshSetEvent != null)
            {
                onRefreshSetEvent();
            }
            CreateNewSet();
        }
    }
    void SpawnRingAtParent(Transform parentTransform)
    {
        GameObject ring = Instantiate(RingPrefab);
        ring.transform.parent = parentTransform;
    }
    void CreateNewSet()
    {
        Debug.Log("REFRESH");
        //TODO Use pooling system to create the object
        for (int i = -1; i < 2; i++)
        {
            GameObject ring = Instantiate(RingPrefab);   
            ring.transform.parent = transform;
            ring.transform.localPosition = new Vector3(i * horizontalOffset, i == 0 ? middleUpperOffset : 0, 0);
            ring.GetComponent<RingBehaviour>().CurrentRingData = GenerateNewRingData();
        }
        ringsInDock = 3;
    }
    RingData GenerateNewRingData ()
    {
        RingData data = new RingData();
        data.Inner = Random.value < 0.5 ? true : false;
        data.Middle = Random.value < 0.5 ? true : false;
        data.Outer = Random.value < 0.5 ? true : false;
        #region EDGE CASES
        if (!(data.Inner && data.Middle && data.Outer))
        {
            float chance = Random.value;
            if (chance < 0.33)
            {
                data.Inner = true;
            }
            else if (chance < 0.66)
            {
                data.Middle = true;
            }
            else
            {
                data.Outer = true;
            }
        }
        if (data.Inner && data.Middle && data.Outer)
        {
            float chance = Random.value;
            if (chance < 0.33f)
            {
                data.Inner = false;
            }
            else if (chance < 0.66f)
            {
                data.Middle = false;
            }
            else
            {
                data.Outer = false;
            }
        } 
        #endregion
        //TODO Address case when adjacent tiers have the same color (not important but good to have)
        data.ringColors = new List<ColorIndex>();
        for (int i = 0; i < 3; i++)
        {
            ColorIndex clr = ColorManager.instance.FetchColorIndex();
            //Debug.Log(clr);
            data.ringColors.Add(clr);
        }
        if (!data.Outer)
        {
            data.ringColors[0] = ColorIndex.NONE;
        }
        if (!data.Middle)
        {
            data.ringColors[1] = ColorIndex.NONE;
        }
        if (!data.Inner)
        {
            data.ringColors[2] = ColorIndex.NONE;
        }
        return data;
    }
}
