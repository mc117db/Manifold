using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RingScaleAnimationController : MonoBehaviour {

	
	// Use this for initialization
	void Start () {
		//TODO Subscribe Shake to the event of ring state change event
		gameObject.GetComponent<RingBehaviour>().stateChangeEvent += Shake;
		//TODO Refactor for pooling later.
		GrowIn();
	}
	public void Shake()
	{
		transform.DOShakeScale(1f,new Vector3(0.5f,0.5f,0),20,30f);
	}
	public void GrowIn()
	{
		transform.localScale = Vector3.zero;
		transform.DOScale(Vector3.one,0.2f).SetEase(Ease.InOutSine);
	}
}
