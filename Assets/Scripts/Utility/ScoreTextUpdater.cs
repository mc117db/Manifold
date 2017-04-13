using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
[RequireComponent(typeof(Text))]
public class ScoreTextUpdater : MonoBehaviour {

    public Text textComponent;
    private Vector3 initPos;
    private Vector3 initScale;
	// Use this for initialization
	void Start () {
        ScoreController.ScoreValueUpdateEvent += UpdateText;
        SFX_PointParticleBehaviour.OnFinishLerpEvent += PunchText;
        initPos = transform.position;
        initScale = transform.localScale;
	}
	
	// Update is called once per frame
	void UpdateText (int valueToPush) {
        textComponent.text = valueToPush.ToString();
        ShakeText();
    }
    void PunchText()
    {
        if (transform.localScale.magnitude > (Vector3.one * 0.5f).magnitude)
        {
            transform.DOPunchScale(Vector3.one * -1 * 0.1f, 0.1f).OnComplete(delegate { transform.DOScale(initScale, 0.1f); }).OnStart( delegate { transform.localScale = initScale; } );
        }
    }
    void ShakeText()
    {
        DOTween.Shake(() => transform.position, x => transform.position = x, 0.1f, 10, 10, 25, false).OnComplete(delegate { transform.position = initPos; });
    }
}
