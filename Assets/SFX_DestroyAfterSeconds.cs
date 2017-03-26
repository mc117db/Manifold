using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_DestroyAfterSeconds : MonoBehaviour {

	public float TimeBeforeDestroy = 3f;
	// Use this for initialization
	void Start () {
		//TODO Add Pooling behaviour over at SFX prefabs
		Destroy(gameObject,TimeBeforeDestroy);
	}

}
