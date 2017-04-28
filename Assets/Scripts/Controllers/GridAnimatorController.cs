using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAnimatorController : MonoBehaviour {

    public SFXController sfxControllerRef;
    public Animator animatorController;
	// Use this for initialization
	void Start () {
        GameController.StartEvent += Entry;
        GameController.LoseEvent += Exit;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void Entry()
    {
        if (animatorController)
        {
            animatorController.enabled = true;
            animatorController.SetTrigger("Entry");
        }
    }
    void Exit()
    {
        if (animatorController)
        {
            animatorController.enabled = true;
            animatorController.SetTrigger("Exit1");
        }
    }
    public void EntryExplodeSFX()
    {
        sfxControllerRef.SpawnGridEntry(transform.position);
    }
    public void ExitImplodeSFX()
    {
        sfxControllerRef.SpawnGridEntry(transform.position);
    }
    public void TurnOffAnimator()
    {
        if (animatorController)
        {
            animatorController.enabled = false;
        }
    }
}
