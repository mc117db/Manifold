using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Devtool_SceneSwitcher : MonoBehaviour {

	// Use this for initialization

    public void SwitchScene (int sceneNo)
    {
        SceneManager.LoadSceneAsync(sceneNo);
    }
}
