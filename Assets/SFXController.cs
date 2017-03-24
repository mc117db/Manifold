using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]

public class SFXController : MonoBehaviour {

    public GameObject prefabToSpawnOnRingConsumeEvent;
    public GameObject prefabToSpawnAlongMatchAxis;

	// Use this for initialization
	void Start () {
        MatchController.instance.OnMatchEventData += ListenToMatches;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ListenToMatches(MatchData data)
    {

        Vector3 totalPosValue = new Vector3(0,0,0);
        foreach (RingBehaviour ring in data.markedObjects)
        {
            totalPosValue += ring.transform.position;
            GameObject sfxRing = Instantiate(prefabToSpawnOnRingConsumeEvent);
            sfxRing.transform.position = ring.transform.position; 
        }
        if (data.markedObjects.Count > 0)
        {
            GameObject sfxAxisEffect = Instantiate(prefabToSpawnAlongMatchAxis);
            Vector3 middlePos = totalPosValue / data.markedObjects.Count;
            sfxAxisEffect.transform.position = middlePos;
            sfxAxisEffect.transform.rotation = Quaternion.LookRotation((data.markedObjects[0].transform.position - data.markedObjects[1].transform.position).normalized);
        }
    }
}
