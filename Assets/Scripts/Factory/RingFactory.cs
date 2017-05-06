using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class RingFactory : MonoBehaviour {

    public GameObject RingPrefab;
    public float horizontalOffset = 2f;
    public float middleUpperOffset = 3f;
    public delegate void onRefreshSet();
    public delegate void onStagingSetUpdate(List<RingData> listOfRingsInStagingSet);
    public static event onRefreshSet onRefreshSetEvent;
    public static event onStagingSetUpdate onStagingSetUpdateEvent;

    private void OnDestroy()
    {
        onRefreshSetEvent = null;
        onStagingSetUpdateEvent = null;
    }
    void Start () {
        SceneController.CleanUp += OnDestroy;
        RingPointManager.RingDropEvent += UpdateState;
    }

    public void UpdateState()
    {
        //Debug.Log("RingFactory Update State");
        //ringsInDock = transform.childCount;
        //Debug.Log("Rings currently in dock: " + transform.childCount);

        if (transform.childCount <= 0)
        {
            CreateNewSet();
             if (onRefreshSetEvent != null)
            {
                onRefreshSetEvent();
            }
        }
        List<RingData> ringDataInUpdatedSet = new List<RingData>();
        foreach (Transform child in transform)
        {
            ringDataInUpdatedSet.Add(child.GetComponent<RingBehaviour>().CurrentRingData);
        }
        if (ringDataInUpdatedSet.Count > 0)
        {
            if (onStagingSetUpdateEvent != null)
            {
                //Debug.Log("Updating Staging Set");
                onStagingSetUpdateEvent(ringDataInUpdatedSet);
            }
        }
    }
    // ----------------------- SPAWNING IMPLEMENTATION --------------------------------------- //
    public void SpawnRandomRingAtPoint(ReferencePointBehaviour point)
    {
        // Currently nothing calls this but its good to have
        GameObject ring = Instantiate(RingPrefab);
        RingBehaviour ringComponent = ring.GetComponent<RingBehaviour>();
        ringComponent.CurrentRingData = GenerateNewRingData();
        
        point.GetComponent<RingPointManager>().AcceptSpawnedRing(ringComponent);
    }
    public void SpawnRingDataAtPoint(RingData ringData, ReferencePointBehaviour point)
    {
        GameObject ring = Instantiate(RingPrefab);
        RingBehaviour ringComponent = ring.GetComponent<RingBehaviour>();
        ringComponent.CurrentRingData = ringData;

        point.GetComponent<RingPointManager>().AcceptSpawnedRing(ringComponent);
    }
    // ------------------------------------------------------------------------------------- //

    public void CreateNewSet()
    {
        //Debug.Log("REFRESH");
        //TODO Use pooling system to create the object, and destroying it.
        if (transform.childCount>0)
        {
            foreach(Transform child in gameObject.transform)
            {
                Destroy(child.gameObject);
            }
        }

        for (int i = -1; i < 2; i++)
        {
            GameObject ring = Instantiate(RingPrefab);   
            ring.transform.parent = transform;
            ring.transform.localPosition = new Vector3(i * horizontalOffset, i == 0 ? middleUpperOffset : 0, 0);
            ring.GetComponent<RingBehaviour>().CurrentRingData = GenerateNewRingData();
            ring.GetComponent<RingScaleAnimationController>().GrowIn();
        }
        UpdateState();
        //ringsInDock = transform.childCount;
    }
    public RingData GenerateNewRingData ()
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
