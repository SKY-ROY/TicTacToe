using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateData
{
    /// <summary>
    /// data types to store various variable to handle GameState 
    /// </summary>
    public bool sfx;
    public bool music;
    public int qualityLevel;
    public int xHighScore, oHighScore;
    public int slotA_val, slotB_val, slotC_val;
    public float volume;

    public StateData(GlobalSettingsControl settings)
    {
        qualityLevel = settings.qualityLevelIndex;
        music = settings.musicBool;
        sfx = settings.sfxBool;
        volume = settings.volumeVal;
        xHighScore = settings.xHighScoreCount;
        oHighScore = settings.oHighScoreCount;

    }
}
