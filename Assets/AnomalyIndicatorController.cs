using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnomalyIndicatorController : MonoBehaviour {

	public bool active;
	public ReferencePointBehaviour target;
    public float lerpSpeed = 20f;
    public Vector3 inActivePos = new Vector3(0.5f, 6f, 5f);
    public SpriteRenderer firstColor, secondColor, thirdColor;
    public Color inactiveIndicatorColor;
	
	// Use this for initialization
	void Start () {
        GameController.TargetReferencePointToSpawnWhenStagingSetUpdateChange += ChangeTargetPoint;
        GameController.RingDataToSpawnWhenStagingSetUpdateChange += ChangeIndicatorColors;
	}
	
    void ChangeTargetPoint(ReferencePointBehaviour point)
    {
        target = point;
        if (target)
        {
            active = true;
        }
        else
        {
            active = false;
        }
    }
    void ChangeIndicatorColors(RingData data)
    {
        if (data.Outer)
        {
            firstColor.color = ColorManager.instance.FetchColorInformation(data.ringColors[0]);
        }
        else
        {
            firstColor.color = inactiveIndicatorColor;
        }
        if (data.Middle)
        {
            secondColor.color = ColorManager.instance.FetchColorInformation(data.ringColors[1]);
        }
        else
        {
            secondColor.color = inactiveIndicatorColor;
        }
        if (data.Inner)
        {
            thirdColor.color = ColorManager.instance.FetchColorInformation(data.ringColors[2]);
        }
        else
        {
            thirdColor.color = inactiveIndicatorColor;
        }
    } 
	// Update is called once per frame
	void Update () {
		if (active)
        {
            if (target)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x, target.transform.position.y,transform.position.z), lerpSpeed * Time.deltaTime);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, inActivePos, lerpSpeed * Time.deltaTime);
        }
	}
}
