using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextFields : MonoBehaviour
{   
    [Header("Gameplay Text Elements")]
    public Text xScoreText;         //reference to in-game Text UIElement
    public Text xHighScoreText;     //reference to in-game Text UIElement
    public Text oScoreText;         //reference to in-game Text UIElement
    public Text oHighScoreText;     //reference to in-game Text UIElement
    //public Text timePassed;
    public bool flag = true;
    private GameObject GameController;

    private void Start()
    {
        GameController = GameObject.Find("GameController");
        StartCoroutine(UpdateState());
    }

    /// <summary>
    /// using coroutine as Update() Method which reduces the memory overhead
    /// </summary>
    IEnumerator UpdateState()
    {
        while (true)//never ending loop
        {
            xScoreText.text = "X Score: " + GameController.GetComponent<GameController_PvAI>().xScore;                      //display current X Score on game screen
            xHighScoreText.text = "X High Score: " + GameController.GetComponent<GlobalSettingsControl>().xHighScoreCount;  //display X High Score on game screen

            oScoreText.text = "O Score: " + GameController.GetComponent<GameController_PvAI>().oScore;                      //display current O Score on game screen
            oHighScoreText.text = "O High Score: " + GameController.GetComponent<GlobalSettingsControl>().oHighScoreCount;  //display O High Score on game screen

            //timePassed.text = "Time (s): " + Time.timeSinceLevelLoad.ToString("0");

            yield return new WaitForEndOfFrame();//end of coroutine 
        }
    }
}
