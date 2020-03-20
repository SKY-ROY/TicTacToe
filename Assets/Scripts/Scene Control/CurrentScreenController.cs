using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentScreenController : MonoBehaviour
{
    public void SceneSwitch(string Title)
    {
        //SceneManager.LoadScene(Title);
        gameObject.GetComponent<LevelLoader>().LoadLevel(Title);
    }
    public void ClosePanel(GameObject ClosingPanel)
    {
        ClosingPanel.SetActive(false);
    }
    public void OpenPanel(GameObject OpeningPanel)
    {
        OpeningPanel.SetActive(true);
    }
}
