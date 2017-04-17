using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum ColorIndex
{
    Alpha,Bravo,Charlie,Delta,Echo,Fanta,Gamma,Hotel,NONE
}
[System.Serializable]
public struct ColorRegistrationData
{
    public ColorIndex indentifier;
    public Color colorToRegister;
}
[System.Serializable]
public struct LevelColors
{
    public List<ColorIndex> colorsInLevel;
}

public class ColorManager : MonoBehaviour {
    public ColorRegistrationData[] colorRegistrationData = new ColorRegistrationData[8];
    public List<LevelColors> registeredLevelColors;
    public List<ColorIndex> activeColors = new List<ColorIndex>();
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
    }
    public Color FetchColorInformation (ColorIndex index)
    {
        foreach (ColorRegistrationData data in colorRegistrationData)
        {
            if (data.indentifier == index)
            {
                return data.colorToRegister;
            }
        }
        Debug.Log("The ColorIndex you are trying to fetch doesn't exist in colorRegistrationData");
        return new Color();
    }
    public void UpdateLevel ()
    {
        if (CurrentLevel > registeredLevelColors.Count)
        {
            Debug.Log("Game tried to advance but max level reached already");
            return;
        }
        Debug.Log("Updating colors to level "+CurrentLevel);
        activeColors.Clear();
        //Debug.Log("NO OF REGISTERED LEVELS: " + registeredLevelColors.Count);
        for (int i = 0; i < CurrentLevel; i++)
        {
            foreach (ColorIndex colr in registeredLevelColors[i].colorsInLevel)
            {
                //Debug.Log("ADD");
                activeColors.Add(colr);
            }
        }
    }
    public ColorIndex FetchColorIndex()
    {
        return activeColors[Random.Range(0, activeColors.Count)];
    }
}
