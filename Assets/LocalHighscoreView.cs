using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class LocalHighscoreView : MonoBehaviour {

    public Text localHighScoreTextComponent;
	// Use this for initialization
	void Start () {
		if (localHighScoreTextComponent)
        {
            if (!PlayerPrefs.HasKey("Local_HighScore"))
            {
                gameObject.SetActive(false);
                return;
            }
            localHighScoreTextComponent.text = PlayerPrefs.GetInt("Local_HighScore").ToString();
        }
	}
}
