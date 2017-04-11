using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
[RequireComponent(typeof(Text))]
public class ScoreTextUpdater : MonoBehaviour {

    public Text textComponent;
    private Vector3 initPos;
	// Use this for initialization
	void Start () {
        ScoreController.ScoreValueUpdateEvent += UpdateText;
        SFX_PointParticleBehaviour.OnFinishLerpEvent += PunchText;
        initPos = transform.position;
	}
	
	// Update is called once per frame
	void UpdateText (int valueToPush) {
        textComponent.text = valueToPush.ToString();
        ShakeText();
    }
    void PunchText()
    {
        DOTween.Punch(() => transform.position, x => transform.position = x,Vector3.up*6f, 0.7f, 10, 0.7f).OnComplete(delegate { transform.position = initPos; });
    }
    void ShakeText()
    {
        DOTween.Shake(() => transform.position, x => transform.position = x, 0.1f, 10, 10, 25, false).OnComplete(delegate { transform.position = initPos; });
    }
}
