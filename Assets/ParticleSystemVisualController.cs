using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParticleSystemVisualController : MonoBehaviour {

    public ParticleSystem[] systems;
	// Use this for initialization
	void Start () {
		
	}
    protected void PlayAll()
    {
        foreach(ParticleSystem system in systems)
        {
            system.Play();
        }
    }
    protected void StopAll()
    {
        foreach (ParticleSystem system in systems)
        {
            if (system.isPlaying)
            {
                system.Stop();
            }
        }
    }
}
