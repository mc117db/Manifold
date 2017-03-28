using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class ScoreTextUpdater : MonoBehaviour {

    public Text textComponent;
	// Use this for initialization
	void Start () {
        ScoreController.instance.ScoreValueUpdateEvent += UpdateText;
	}
	
	// Update is called once per frame
	void UpdateText (int valueToPush) {
        textComponent.text = valueToPush.ToString();
	}
}
