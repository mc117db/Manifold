using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RingDragBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public static GameObject DraggedInstance;
    Vector3 _startPosition;
    Vector3 _offsetToMouse;
    float _zDistanceToCamera;
    public static bool dragging;
    private bool canDrag;
    public delegate void failDrag();
    public event failDrag failDragEvent;

    public bool CanDrag
    {
        get
        {
            return canDrag;
        }

        set
        {
            canDrag = value;
        }
    }
    private void OnDestroy()
    {
        // HACK Refactor to get rid of this...
        RingDragBehaviour.dragging = false;
    }
    #region Interface Implementations
    void Start()
    {
        CanDrag = true;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!CanDrag || GameController.instance.GAMESTATE != GameState.Running)
        {
            return;
        }
            DraggedInstance = gameObject;
            _startPosition = transform.position;
            _zDistanceToCamera = Mathf.Abs(_startPosition.z - Camera.main.transform.position.z);

            _offsetToMouse = _startPosition - Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, _zDistanceToCamera)
            );
            dragging = true;
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!CanDrag || GameController.instance.GAMESTATE != GameState.Running)
        {
            return;
        }
        if (Input.touchCount > 1)
        {
            return;
        }

        transform.position = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, _zDistanceToCamera)
            ) + _offsetToMouse;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragging)
        {
            dragging = false;
            if (!CanDrag)
            {
                // This means its attached to something
                return;
            }
            if (failDragEvent != null)
            {
                failDragEvent();
            }
            transform.position = _startPosition;
            DraggedInstance = null;
            _offsetToMouse = Vector3.zero;
        }
    }
    public void OnDragOverAndCombine()
    {
        dragging = false;
        DraggedInstance = null;
        _offsetToMouse = Vector3.zero;
        //TODO Refactor for pooling later
        //Debug.Log("DESTROYING RING TO BE COMBINED");
        transform.parent = null;
        Destroy(gameObject);
    }

    #endregion
}
