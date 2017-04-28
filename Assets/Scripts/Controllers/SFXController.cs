using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]

public class SFXController : MonoBehaviour {

    [Header("Prefabs")]
    public GameObject prefabToSpawnOnRingConsumeEvent_FALLBACK;
    public GameObject prefabToSpawnAlongAxis_FALLBACK;
    public GameObject prefabToSpawnPoint_FALLBACK;
    public GameObject blueSFXRing, greenSFXRing, purpleSFXRing, redSFXRing, yellowSFXRing;
    public GameObject blueAxisBlast, greenAxisBlast, purpleAxisBlast, redAxisBlast, yellowAxisBlast;
    public GameObject bluePoint, greenPoint, purplePoint, redPoint, yellowPoint;
    public GameObject blueStackBlast, greenStackBlast, purpleStackBlast, redStackBlast, yellowStackBlast;
    [Space(10)]
    [Header("Point SFX Settings")]
    public Vector3 scoreTextPos;
    public float intVel, spd, deltaMin, deltaMax;
    [Space(10)]
    public GameObject gridEntrySFX;

    // Use this for initialization
    void Start () {
        MatchController.OnMatchEventData += ListenToMatches;
        GameController.RemoveColorTiersLocationEvent += SpawnPointsFromColorRemoval;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void SpawnPointsFromColorRemoval(List<Vector3>positions,ColorIndex colr)
    {
        SpawnStackBlast(colr);
        for (int i = 0; i < positions.Count;i++)
        {
            GameObject sfxRing = Instantiate(GetSFXRingFromColorCode(colr));
            sfxRing.AddComponent<SFX_DestroyAfterSeconds>();
            sfxRing.transform.position = positions[i];

            GameObject pt = Instantiate(GetSFXPoint(colr));
            pt.transform.position = positions[i];
            pt.AddComponent<SFX_PointParticleBehaviour>().Initialize(intVel,spd,deltaMin,deltaMax,scoreTextPos);
        }
    }
    void ListenToMatches(MatchData data)
    {
        Vector3 totalPosValue = new Vector3(0,0,0);
        foreach (RingBehaviour ring in data.markedObjects)
        {
            // Spawn Rings
            totalPosValue += ring.transform.position;
            GameObject sfxRing = Instantiate(GetSFXRingFromColorCode(data.colorMark));
            sfxRing.AddComponent<SFX_DestroyAfterSeconds>();
            sfxRing.transform.position = ring.transform.position;
            // Spawn Points
            GameObject sfxPoint = Instantiate(GetSFXPoint(data.colorMark));
            sfxPoint.transform.position = ring.transform.position;
            sfxPoint.AddComponent<SFX_PointParticleBehaviour>().Initialize(intVel,spd,deltaMin,deltaMax,scoreTextPos);
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
    GameObject GetSFXPoint(ColorIndex index)
    {
        switch (index)
        {
            case ColorIndex.Alpha:
                return redPoint;
            case ColorIndex.Bravo:
                return bluePoint;
            case ColorIndex.Charlie:
                return greenPoint;
            case ColorIndex.Delta:
                return purplePoint;
            case ColorIndex.Echo:
                return yellowPoint;
            case ColorIndex.Fanta:
                return purplePoint;
            case ColorIndex.Gamma:
                return purplePoint;
            case ColorIndex.Hotel:
                return bluePoint;
            default:
                return prefabToSpawnPoint_FALLBACK;
        }
    }
    GameObject GetStackBlastFromColorCode(ColorIndex index)
    {
        switch (index)
        {
            case ColorIndex.Alpha:
                return redStackBlast;
            case ColorIndex.Bravo:
                return blueStackBlast;
            case ColorIndex.Charlie:
                return greenStackBlast;
            case ColorIndex.Delta:
                return purpleStackBlast;
            case ColorIndex.Echo:
                return yellowStackBlast;
            case ColorIndex.Fanta:
                return purpleStackBlast;
            case ColorIndex.Gamma:
                return purpleStackBlast;
            case ColorIndex.Hotel:
                return blueStackBlast;
            default:
                return blueStackBlast;
        }
    }

    public void SpawnGridEntry (Vector3 pos)
    {
        if (gridEntrySFX)
        {
            GameObject gridEntrySFXGO = Instantiate(gridEntrySFX, pos, Quaternion.identity);
            gridEntrySFXGO.AddComponent<SFX_DestroyAfterSeconds>();
        }
    }
    public void SpawnStackBlast(ColorIndex index)
    {
        GameObject stackBlast = Instantiate(GetStackBlastFromColorCode(index));
        stackBlast.transform.position = Vector3.zero+new Vector3(0,0,-3);
        stackBlast.AddComponent<SFX_DestroyAfterSeconds>();
    }
}
