using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.IO;

public class GlobalSettingsControl : MonoBehaviour
{
    [Header("Setting Variables")]
    public bool musicBool = true;                   //is music on or not
    public bool sfxBool = true;                     //is sound on or not
    public int qualityLevelIndex;                   //quality level "0:Low, 1:medium, 2:High"
    public float volumeVal;                         //volume value
    public int xHighScoreCount;                     //X High Score
    public int oHighScoreCount;                     //O High Score

    [Header("Scene Object Reference")]
    public Slider volumeSlider;                     //reference to UIElement volume slider
    public static int size;
    public Toggle[] toggleArray = new Toggle[size]; //reference to toggle in oreder: Music, SFX.
    public Dropdown graphicsVal;                    //reference to drop down list

    [Header("Global Object Reference")]
    public AudioMixer audioMixer;                   //Audio Mixer Refence to control all audio sources

    private float previousVol;

    private void Awake()
    {
        IsFirstTime();
        LoadState();
    }
    private void Start()
    {
        if (musicBool && sfxBool)
        {
            GetComponent<AudioSource>().Play();
        }
    }
    private void Update()
    {
        graphicsVal.value = qualityLevelIndex;
        toggleArray[0].isOn = musicBool;
        toggleArray[1].isOn = sfxBool;
        volumeSlider.value = volumeVal;
    }
    /// <summary>
    /// getter method for xHighScoreCount
    /// </summary>
    public int GetXHS()
    {
        return xHighScoreCount;
    }
    /// <summary>
    /// getter method for xHighScoreCount
    /// </summary>
    public int GetOHS()
    {
        return oHighScoreCount;
    }
    /// <summary>
    /// method to update and save oHighScoreCount
    /// </summary>
    /// <param name="scoreValue"></param>
    public void OHighScoreUpdate(int scoreValue)
    {
        if (gameObject.GetComponent<GameController_PvAI>())
        {
            oHighScoreCount = scoreValue;
            SaveState();
            Debug.Log("O HS: " + oHighScoreCount);
        }
    }
    /// <summary>
    /// method to update and save xHighScoreCount
    /// </summary>
    /// <param name="scoreValue"></param>
    public void XHighScoreUpdate(int scoreValue)
    {
        if(gameObject.GetComponent<GameController_PvAI>())
        {
            xHighScoreCount = scoreValue;
            SaveState();
            Debug.Log("X HS: " + xHighScoreCount);
        }
    }
    /// <summary>
    /// listener method for event handler to update and save quality index value
    /// </summary>
    /// <param name="qualityIndex"></param>
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        qualityLevelIndex = qualityIndex;
        SaveState();
    }
    /// <summary>
    /// listener method for event handler to update and save music boolean value
    /// </summary>
    /// <param name="argVal"></param>
    public void SetMusic(bool argVal)
    {
        musicBool = argVal;
        if(musicBool)
        {
            GetComponent<AudioSource>().Play();
        }
        else if(!musicBool)
        {
            GetComponent<AudioSource>().Stop();
        }
        SaveState();
    }
    /// <summary>
    /// listener method for event handler to update and save SFX boolean value
    /// </summary>
    /// <param name="argVal"></param>
    public void SetSFX(bool argVal)
    {
        sfxBool = argVal;
        if(!sfxBool)
        {
            audioMixer.GetFloat("Volume", out previousVol);
            audioMixer.SetFloat("Volume", -80);
            GetComponent<AudioSource>().Stop();
        }
        else if(sfxBool)
        {
            audioMixer.SetFloat("Volume", previousVol);
            GetComponent<AudioSource>().Play();
        }
        SaveState();
    }
    /// <summary>
    /// listener method for event handler to update and save sound volume value 
    /// </summary>
    /// <param name="argvolume"></param>
    public void SetVolume(float argvolume)
    {
        audioMixer.SetFloat("Volume", argvolume);
        volumeVal = argvolume;
        SaveState();
    }
    /// <summary>
    /// method to save all the variable values controlling game state into the binary file
    /// </summary>
    public void SaveState()
    {
        SaveSystem.SaveState(this);
        
    }
    /// <summary>
    /// method to load values of game state altering variables from binary file which store values from previous game session
    /// </summary>
    public void LoadState()
    {
        StateData data = SaveSystem.LoadState();

        sfxBool = data.sfx;
        musicBool = data.music;
        qualityLevelIndex = data.qualityLevel;
        volumeVal = data.volume;
        xHighScoreCount = data.xHighScore;
        oHighScoreCount = data.oHighScore;
        Debug.Log("Loaded values-> XHS:" + xHighScoreCount + " OHS:" + oHighScoreCount);
    }
    /// <summary>
    /// method to check whether the application is run for very first time on respective system or not, if yes a new binary file is created by SaveState() method
    /// </summary>
    public void IsFirstTime()
    {
        string path1 = Application.persistentDataPath + "/this.Sett";
        
        if (!File.Exists(path1))
        {
            Debug.Log("First Time Launch");
            SaveState();
        }
        else
            Debug.Log("Not First Time");
    }
}
