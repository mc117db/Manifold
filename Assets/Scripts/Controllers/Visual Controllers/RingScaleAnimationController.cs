using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RingScaleAnimationController : MonoBehaviour {

    Vector3 initScale = Vector3.one;
	// Use this for initialization
	void Start () {
		gameObject.GetComponent<RingBehaviour>().stateChangeEvent += Shake;
        gameObject.GetComponent<RingDragBehaviour>().failDragEvent += GrowIn;
		//TODO Refactor for pooling later.
	}
	public void Shake()
	{
		transform.DOShakeScale(1f,new Vector3(0.5f,0.5f,0),20,30f);
	}
	public void GrowIn()
	{
        initScale = transform.localScale;
        transform.localScale = Vector3.zero;
		transform.DOScale(initScale, 0.2f).SetEase(Ease.InOutSine);
	}
}
