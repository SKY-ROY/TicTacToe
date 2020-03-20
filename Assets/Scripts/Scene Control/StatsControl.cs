using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsControl : MonoBehaviour
{
    public Text xHighScore;//UIElement Text reference for X High Score on stats panel
    public Text oHighScore;//UIElement Text reference for O High Score on stats panel 

    private GameObject settings;

    // Start is called before the first frame update
    void Start()
    {
        settings = GameObject.Find("StartSceneControl");//for future refernce in the script
        StartCoroutine(UpdateState());
    }

    /// <summary>
    /// using co-routine instead of Update() to display Loaded HighScore values from binary file, and starting this coroutine in Start method so values are displayed recurringly
    /// </summary>
    IEnumerator UpdateState()
    {
        while(true)//infinite loop
        { 
            xHighScore.text = "X High Score: " + settings.GetComponent<GlobalSettingsControl>().xHighScoreCount;//O HighScore display on start screen
            oHighScore.text = "O High Score: " + settings.GetComponent<GlobalSettingsControl>().oHighScoreCount;//X HighScore display on start screen

            yield return new WaitForEndOfFrame();//end of coroutine
        }
    }
}
