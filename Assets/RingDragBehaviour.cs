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

    [SerializeField]

    #region Interface Implementations

    void Start ()
    {

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (DraggedInstance == null)
        {
            DraggedInstance = gameObject;
            _startPosition = transform.position;
            _zDistanceToCamera = Mathf.Abs(_startPosition.z - Camera.main.transform.position.z);

            _offsetToMouse = _startPosition - Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, _zDistanceToCamera)
            );
            dragging = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.touchCount > 1)
            return;

        transform.position = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, _zDistanceToCamera)
            ) + _offsetToMouse;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        DraggedInstance = null;
        _offsetToMouse = Vector3.zero;
    }

    #endregion
}
