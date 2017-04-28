using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

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
    public delegate void onDragDrop(bool isDragged);
    public static event onRingDrop RingDropEvent; // RingFactory depends on this
    public static Action<ColorIndex> onTierMatchUp; // Learnt something new about Actions - https://unity3d.college/2016/10/05/unity-events-actions-delegates/
    public event onRingDrop stateChange; // This happens when something changes to current ring
    public event onDragDrop stateChangeDragDrop;
    private void OnDestroy()
    {
        RingDropEvent = null;
    }
    void Start()
    {
        stateChangeDragDrop += CheckNeighboursForSimiliarColors;
    } 
    // IDROPHANDLER IMPLEMENTATION
    private void CheckNeighboursForSimiliarColors(bool isDragDrop)
    {
        if (!Ring)
        {
            return;
        }
        GetComponent<ReferencePointBehaviour>().CheckNeighboursForColor(Ring.CurrentRingData.ringColors,isDragDrop);
    }
    public bool CheckPointIfCanAccept(RingData other)
    {
        if (ring)
        {
            return ring.CheckIfCanCombine(other);
        }
        else
        {
            return true;
        }
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
            if (Ring)
            {
                RingBehaviour otherRing = RingDragBehaviour.DraggedInstance.GetComponent<RingBehaviour>();
                if (Ring.CombineRings(otherRing.CurrentRingData))
                {
                    // The dragged ring will be destroyed with its data transferred to the residing ring in ringpointmanager.
                    //AcceptDragRing(); 
                    otherRing.GetComponent<RingDragBehaviour>().OnDragOverAndCombine();
                    RingDropEvent();
                    if (stateChange != null)
                    {
                        stateChange();
                    }
                    // Check if ring has 3 of the same colors using LINQ expression
                    if (!(Ring.CurrentRingData.ringColors.Any(o => o != Ring.CurrentRingData.ringColors[0])) && Ring.CurrentRingData.ringColors[0] != ColorIndex.NONE)
                    {
                        onTierMatchUp(Ring.CurrentRingData.ringColors[0]);
                    }
                    else if (stateChangeDragDrop != null)
                    {
                        stateChangeDragDrop(true);
                    }
                  
                }
                else
                {
                    // CASE WHEN TIERS DONT MATCH UP
                    return;
                }
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
    public void RemoveRing()
    {
        if (ring)
        {
            ring.RemoveRing();
        }
    }
    void AcceptDragRing()
    {
        if (RingDragBehaviour.DraggedInstance.GetComponent<RingBehaviour>() != null)
        {
            Ring = RingDragBehaviour.DraggedInstance.GetComponent<RingBehaviour>();
            IntializeRing(true);
        }
    }
    public void AcceptSpawnedRing(RingBehaviour ringToAccept)
    {
        Ring = ringToAccept;
        IntializeRing(false);
        Ring.GetComponent<RingScaleAnimationController>().GrowIn();
    }
    void IntializeRing(bool isDragDrop)
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
        if (stateChangeDragDrop != null)
        {
            stateChangeDragDrop(isDragDrop);
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
