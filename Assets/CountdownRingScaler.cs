using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CountdownRingScaler : MonoBehaviour {

    public float startScale = 5f;
    public float endScale = 0.2f;
    public float secondsToScaleDown = 10f;
    private float targetScale;
    public float animationLerpSpeed = 2f;
	// Use this for initialization
	void Start () {
        transform.DOScale(Vector3.one * endScale, secondsToScaleDown); 
    }
	
	// Update is called once per frame
	void Update () {

    }
}
