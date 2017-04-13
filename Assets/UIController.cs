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
    public void TurnOffAnimator()
    {
        CanvasController.enabled = false;
    }
    public void Restart()
    {
        CanvasController.enabled = true;
        CanvasController.SetTrigger("Restart");
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
