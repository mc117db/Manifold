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
    private int currentLevel = 1;
    public static ColorManager instance;

    public int CurrentLevel
    {
        get
        {
            return currentLevel;
        }

        set
        {
            currentLevel = value;
            Mathf.Clamp(currentLevel, 0, registeredLevelColors.Count);
            UpdateLevel();
        }
    }

    // Use this for initialization
    void Awake ()
    {
        instance = this;
        UpdateLevel();
    }
    public void UpdateLevel ()
    {
        Debug.Log("Updating Intial colors");
        activeColors.Clear();
        Debug.Log("NO OF REGISTERED LEVELS: " + registeredLevelColors.Count);
        for (int i = 0; i < CurrentLevel; i++)
        {
            foreach (Color colr in registeredLevelColors[i].colorsInLevel)
            {
                Debug.Log("ADD");
                activeColors.Add(colr);
            }
        }
    }
    public Color FetchColor()
    {
        return activeColors[Random.Range(0, activeColors.Count)];
    }
}
