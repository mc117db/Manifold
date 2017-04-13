using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    public Animator CanvasController;
	// Use this for initialization
	void Start () {
        GameController.GameStateChange += ListenToGameStateChange;
        CanvasController.enabled = false;
	}
	
	// Update is called once per frame
    void ListenToGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.Running:
                CanvasController.enabled = false;
                break;
            case GameState.Lose:
                Lose();
                break;
            case GameState.Paused:
                Pause();
                break;
        }
    }
    void Lose()
    {
        CanvasController.enabled = true;
        CanvasController.SetTrigger("Lose");
    }
    void Pause()
    {

    }
}
