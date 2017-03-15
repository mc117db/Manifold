using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct LevelColors
{
    public List<Color> colorsInLevel;
}

public class ColorManager : MonoBehaviour {
    public List<LevelColors> registeredLevelColors;
    public List<Color> activeColors = new List<Color>();
    public int currentLevel = 1;
    public static ColorManager instance;
	// Use this for initialization
    void Awake ()
    {
        instance = this;
    }
    public void UpdateLevel ()
    {
        Debug.Log("Updating Intial colors");
        activeColors.Clear();
        for (int i = 0; i < currentLevel; i++)
        {
            foreach (Color colr in registeredLevelColors[i].colorsInLevel)
            {
                activeColors.Add(colr);
            }
        }
    }
    public Color FetchColor()
    {
        return activeColors[Random.Range(0, activeColors.Count)];
    }
}
