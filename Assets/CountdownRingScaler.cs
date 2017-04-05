using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CountdownRingScaler : MonoBehaviour {

    public float minScale = 0.5f;
    public float maxScale = 4f;

    float currentLerp;
    public float lerpTime = 0.5f;
    private Vector3 velocity;
	// Use this for initialization
	void Start () {
        //transform.DOScale(Vector3.one * endScale, secondsToScaleDown);
        GameController.CountdownLerpEvent += UpdateTargetLerp;
    }
	
    void UpdateTargetLerp(float lerpVal)
    {
        currentLerp = lerpVal;
    }
	// Update is called once per frame
	void Update () {
        transform.localScale = Vector3.SmoothDamp(transform.localScale, Mathf.Lerp(minScale, maxScale, currentLerp) * Vector3.one, ref velocity, lerpTime);
    }
}
