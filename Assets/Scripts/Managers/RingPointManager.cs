using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RingPointManager : MonoBehaviour, IDropHandler {

    // Guidelines for this manager
    // This should not know anything about the overall game state
    // This should not know its own neighbours

    RingBehaviour ring;
    public RingBehaviour Ring
    {
        get{  
            return ring;
        }
        set
        {
            ring = value;
            if (ring)
            {
            ToggleReferencePointRenderer(false);
            }
            else
            {
            ToggleReferencePointRenderer(true);
            }
        }
    }
    static float localRingSize = 1.5f;
    public delegate void onRingDrop();
    public static event onRingDrop RingDropEvent; // RingFactory depends on this
    public event onRingDrop stateChange; // This happens when something changes to current ring
    void Start()
    {
        stateChange += CheckNeighboursForSimiliarColors;
    } 
    // IDROPHANDLER IMPLEMENTATION
    private void CheckNeighboursForSimiliarColors()
    {
        if (!Ring)
        {
            return;
        }
        GetComponent<ReferencePointBehaviour>().CheckNeighboursForColor(Ring.CurrentRingData.ringColors);
    }
    public void OnDrop(PointerEventData eventData)
    { 
        if (!Ring)
        {
            // If reference point dont hold a ring, accept it straight away.
            AcceptDragRing();
        }
        else
        {
            //Debug.Log("SOMETHING INSIDE");
            RingBehaviour otherRing = RingDragBehaviour.DraggedInstance.GetComponent<RingBehaviour>();
            if (Ring.CombineRings(otherRing.CurrentRingData))
            {
                //AcceptDragRing();
                RingDropEvent();
                stateChange();
                GetComponent<RingDragBehaviour>().OnDragOverAndCombine();
            }
            else
            {
                // CASE WHEN TIERS DONT MATCH UP
                return;
            }
        }
        
    }
    public bool HaveColor(ColorIndex index)
    {
        if (!Ring || index == ColorIndex.NONE)
        {
            return false;
        }
        else
        {
            return Ring.CurrentRingData.ringColors.Contains(index);
        }
    }
    void AcceptDragRing()
    {
        Ring = RingDragBehaviour.DraggedInstance.GetComponent<RingBehaviour>();
        IntializeRing();
    }
    public void AcceptSpawnedRing(RingBehaviour ringToAccept)
    {
        Ring = ringToAccept;
        IntializeRing();
        Ring.GetComponent<RingScaleAnimationController>().GrowIn();
    }
    void IntializeRing()
    {
        Ring.gameObject.GetComponent<RingDragBehaviour>().CanDrag = false;
        Ring.transform.parent = transform;
        Ring.transform.localScale = Vector3.one * localRingSize;
        Ring.transform.localPosition = Vector3.zero;
        Ring.IsInPlace = true;
        if (stateChange != null)
        {
            stateChange();
        }
        if (RingDropEvent != null)
        {
            RingDropEvent();
        }
    }
    public void ToggleReferencePointRenderer (bool val)
    {
        //HACK
        GetComponent<SpriteRenderer>().enabled = val;
        GetComponent<ReferencePointBehaviour>().ToggleColor(false);
    }
}
