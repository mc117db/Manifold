using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RingPointManager : MonoBehaviour, IDropHandler {

    GameObject Ring;
    static float localRingSize = 1.5f;
	
    public bool ReceiveRing(ref RingDragBehaviour dRing)
    {
        //TODO get argument to accept a ring component reference rather than gameobject
        if (dRing)
        {
            // TODO Add conditional here that checks ring between current ring and ring to be accepted, return false if cannot fit.
            Ring = dRing.gameObject;
            Ring.transform.parent = transform;
            Ring.transform.localScale = Vector3.one * localRingSize;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("DROP");
        Ring = RingDragBehaviour.DraggedInstance;
        Ring.transform.parent = transform;
        Ring.transform.localScale = Vector3.one * localRingSize;
        Ring.transform.localPosition = Vector3.zero;
    }
}
