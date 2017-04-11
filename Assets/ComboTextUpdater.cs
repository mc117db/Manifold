using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboTextUpdater : MonoBehaviour {

    public Text textComponent;
    public string appendText = "COMBO";
	// Use this for initialization
	void Start () {
        ScoreController.ComboUpdateEvent += UpdateText;
        textComponent.enabled = false;
	}
	
	// Update is called once per frame
	void UpdateText (int curCombo) {
        textComponent.enabled = curCombo > 0 ? true : false;
        textComponent.text = curCombo.ToString() + " " + appendText;
	}
}
