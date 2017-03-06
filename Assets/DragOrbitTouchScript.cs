using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragOrbitTouchScript : MonoBehaviour {

	[SerializeField]
	Vector2 startInputPos;
	[SerializeField]
	Vector2 currentInputPos;
	[SerializeField]
	bool touching;

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
				startInputPos = Vector2.zero;
				currentInputPos = Vector2.zero;
			}
			touching = !touching;
		}
		if (touching)
		{
			currentInputPos = FetchCurrentInputPosition();
		}	
	}
	Vector2 FetchCurrentInputPosition()
	{
		return new Vector2(Input.mousePosition.x,Input.mousePosition.y);
	}
}
