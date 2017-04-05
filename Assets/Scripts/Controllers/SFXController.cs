using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]

public class SFXController : MonoBehaviour {

    public GameObject prefabToSpawnOnRingConsumeEvent_FALLBACK;
    public GameObject prefabToSpawnAlongAxis_FALLBACK;
    public GameObject blueSFXRing, greenSFXRing, purpleSFXRing, redSFXRing, yellowSFXRing;
    public GameObject blueAxisBlast, greenAxisBlast, purpleAxisBlast, redAxisBlast, yellowAxisBlast;

    // Use this for initialization
    void Start () {
        MatchController.OnMatchEventData += ListenToMatches;
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
            GameObject sfxRing = Instantiate(GetSFXRingFromColorCode(data.colorMark));
            sfxRing.AddComponent<SFX_DestroyAfterSeconds>();
            sfxRing.transform.position = ring.transform.position; 
        }
        if (data.markedObjects.Count > 0)
        {
            GameObject sfxAxisEffect = Instantiate(GetAxisBlastFromColorCode(data.colorMark));
            sfxAxisEffect.AddComponent<SFX_DestroyAfterSeconds>();
            Vector3 middlePos = totalPosValue / data.markedObjects.Count;
            sfxAxisEffect.transform.position = middlePos;
            sfxAxisEffect.transform.rotation = Quaternion.LookRotation((data.markedObjects[0].transform.position - data.markedObjects[1].transform.position).normalized);
        }
    }
    GameObject GetAxisBlastFromColorCode(ColorIndex index)
    {
        switch (index)
        {
            case ColorIndex.Alpha:
                return redAxisBlast;
            case ColorIndex.Bravo:
                return blueAxisBlast;
            case ColorIndex.Charlie:
                return greenAxisBlast;
            case ColorIndex.Delta:
                return purpleAxisBlast;
            case ColorIndex.Echo:
                return yellowAxisBlast;
            case ColorIndex.Fanta:
                return purpleAxisBlast;
            case ColorIndex.Gamma:
                return purpleAxisBlast;
            case ColorIndex.Hotel:
                return blueAxisBlast;
            default:
                return prefabToSpawnAlongAxis_FALLBACK;
        }
    }
    GameObject GetSFXRingFromColorCode(ColorIndex index)
    {
        switch (index)
        {
            case ColorIndex.Alpha:
                return redSFXRing;
            case ColorIndex.Bravo:
                return blueSFXRing;
            case ColorIndex.Charlie:
                return greenSFXRing;
            case ColorIndex.Delta:
                return purpleSFXRing;
            case ColorIndex.Echo:
                return yellowSFXRing;
            case ColorIndex.Fanta:
                return purpleSFXRing;
            case ColorIndex.Gamma:
                return purpleSFXRing;
            case ColorIndex.Hotel:
                return blueSFXRing;
            default:
                return prefabToSpawnOnRingConsumeEvent_FALLBACK;
        }
    }
}
