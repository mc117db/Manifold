using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RingPointManager : MonoBehaviour, IDropHandler {

    RingBehaviour Ring;
    static float localRingSize = 1.5f;
    public delegate void onRingDrop();
    public static event onRingDrop RingDropEvent; // RingFactory depends on this

    #region OLD IMPLMENTATION
    /*
    public bool ReceiveRing(ref RingDragBehaviour dRing)
    {
        //TODO get argument to accept a ring component reference rather than gameobject (DONE)
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
    */
    #endregion

    // IDROPHANDLER IMPLEMENTATION
    public void OnDrop(PointerEventData eventData)
    { 
        if (!Ring)
        {
            // If reference point dont hold a ring, accept it straight away.
            AcceptRing();
        }
        else
        {
            //Debug.Log("SOMETHING INSIDE");
            RingBehaviour otherRing = RingDragBehaviour.DraggedInstance.GetComponent<RingBehaviour>();
            if (Ring.CombineRings(otherRing.CurrentRingData))
            {
                AcceptRing();
            }
            else
            {
                // CASE WHEN TIERS DONT MATCH UP
                return;
            }
        }
        
    }
    void AcceptRing()
    {
        Ring = RingDragBehaviour.DraggedInstance.GetComponent<RingBehaviour>();
        Ring.gameObject.GetComponent<RingDragBehaviour>().CanDrag = false;
        Ring.transform.parent = transform;
        Ring.transform.localScale = Vector3.one * localRingSize;
        Ring.transform.localPosition = Vector3.zero;

        RingDropEvent();
    }
}
