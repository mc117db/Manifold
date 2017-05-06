using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    public delegate void OnEvent();
    public static event OnEvent CleanUp;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void LoadScene(int levelIndex)
    {
        if (CleanUp != null)
        {
            CleanUp();
            CleanUp = null;
        }
        SceneManager.LoadScene(levelIndex);
        
    }
}
