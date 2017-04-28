using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragOrbitTouchScript : MonoBehaviour {

	[SerializeField]
	Vector2 startInputPos;
	[SerializeField]
	Vector2 currentInputPos;
	[SerializeField]
	Vector2 delta;
	Vector2 deltaScreenRatio; // Ratio in ratio to the screen size
	Vector2 mouseMovementDelta;
	Vector2 lastFrameInputPosition;
	[SerializeField]
	bool touching;
    bool active = true;

	public delegate void OnDeltaChange(Vector2 delta);
	public static event OnDeltaChange deltaChange;
	public static event OnDeltaChange deltaRatioChange;
	public static event OnDeltaChange mouseMovementDeltaChange;

    private void OnDestroy()
    {
        deltaChange = null;
        deltaRatioChange = null;
        mouseMovementDeltaChange = null;
    }
    // Use this for initialization
    void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
        active = !RingDragBehaviour.dragging;
		// Mouse Input
		if ((Input.GetMouseButtonDown(0)&&!touching) || (Input.GetMouseButtonUp(0))&&touching)
		{
			if (!touching)
			{
				//Store Start Position of Drag
				startInputPos = FetchCurrentInputPosition();
			}
			else
			{
				ResetValues();
			}
			touching = !touching;
		}

		if (touching)
		{
            if (active)
            {
                currentInputPos = FetchCurrentInputPosition();
                delta = currentInputPos - startInputPos;
                deltaScreenRatio = new Vector2((float)System.Math.Round(delta.x / Screen.width, 2),
                                                (float)System.Math.Round(delta.y / Screen.height, 2));
                mouseMovementDelta = currentInputPos - lastFrameInputPosition;
                lastFrameInputPosition = currentInputPos;
                NotifyListeners();
            }
			else
			{
				ResetValues();
			}
		}
	}
	// ---------------------- Helper Functions --------------------- //
	Vector2 FetchCurrentInputPosition()
	{
		return new Vector2(Input.mousePosition.x,Input.mousePosition.y);
	}
	void NotifyListeners()
	{
			if (deltaChange != null)
			{
				deltaChange(delta);
			}
			if (deltaRatioChange != null)
			{
				deltaRatioChange(deltaScreenRatio);
			}
			if (mouseMovementDeltaChange != null)
			{
				mouseMovementDeltaChange(mouseMovementDelta);
			}
	}	
	void ResetValues()
	{
		startInputPos = Vector2.zero;
		currentInputPos = Vector2.zero;
		delta = Vector2.zero;
		deltaScreenRatio = Vector2.zero;
		mouseMovementDelta = Vector2.zero;
		lastFrameInputPosition = Vector2.zero;
	}
}
