using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFireParticleController : ParticleSystemVisualController {

	// Use this for initialization
	void Start () {
        ScoreController.ComboUpdateEvent += ScoreController_ComboUpdateEvent;
        StopAll();
	}

    private void ScoreController_ComboUpdateEvent(int valueToReturn)
    {
        if (valueToReturn >= 2)
        {
            PlayAll();
        }
        else
        {
            StopAll();
        }
    }
}
