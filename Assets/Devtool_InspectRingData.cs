using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Devtool_InspectRingData : MonoBehaviour {

    public RingBehaviour ringToInspect;
    public void PrintRingDataInConsole()
    {
        if (ringToInspect)
        {
            Debug.Log("Current Ring Data: OUTER:[" + ringToInspect.CurrentRingData.ringColors[0].ToString() 
                +"] MIDDLE: ["+ ringToInspect.CurrentRingData.ringColors[1].ToString() 
                +"] INNER: ["+ ringToInspect.CurrentRingData.ringColors[2].ToString()+"]");
        }
        else
        {
            Debug.Log("Please Assign Ring in ringToInspect");
        }
    }

}
