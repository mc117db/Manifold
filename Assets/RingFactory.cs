using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingFactory : MonoBehaviour {

    public GameObject RingPrefab;
    public float horizontalOffset = 2f;
    public float middleUpperOffset = 3f;
    // Use this for initialization
    [SerializeField]
    int ringsInDock = 3;
	void Start () {
        RingPointManager.RingDropEvent += UpdateState;
        CreateNewSet(); //TODO This will be called by the GameManager later
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateState()
    {
        ringsInDock--;
        if (ringsInDock <= 0)
        {    
            CreateNewSet();
        }
    }
    void CreateNewSet()
    {
        Debug.Log("REFRESH");
        //TODO use pooling system to create the object
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
        if (!(data.Inner&&data.Middle&&data.Outer))
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
        data.ringColors = new List<Color>();
        for (int i = 0; i < 3; i++)
        {
            Color clr = ColorManager.instance.FetchColor();
            //Debug.Log(clr);
            data.ringColors.Add(clr);
        }
        return data;
    }
}
