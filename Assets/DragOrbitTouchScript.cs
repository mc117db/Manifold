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
	[SerializeField]
	bool touching;

	public delegate void OnDeltaChange(Vector2 delta);
	public static event OnDeltaChange deltaChange;
	public static event OnDeltaChange deltaRatioChange;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
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
			delta = currentInputPos - startInputPos;
			deltaScreenRatio = new Vector2 ((float)System.Math.Round(delta.x/Screen.width,2) , 
											(float)System.Math.Round(delta.y/Screen.height,2));
			if (deltaChange != null)
			{
				deltaChange(delta);
			}
			if (deltaRatioChange != null)
			{
				deltaRatioChange(deltaScreenRatio);
			}
			currentInputPos = FetchCurrentInputPosition();
		}
	}
	Vector2 FetchCurrentInputPosition()
	{
		return new Vector2(Input.mousePosition.x,Input.mousePosition.y);
	}
	void ResetValues()
	{
		startInputPos = Vector2.zero;
		currentInputPos = Vector2.zero;
		delta = Vector2.zero;
		deltaScreenRatio = Vector2.zero;
	}
}
