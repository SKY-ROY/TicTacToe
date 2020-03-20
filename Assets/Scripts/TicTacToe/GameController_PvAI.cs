using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player
{
    public Image panel;
    public Text text;
    public Button button;
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

public class GameController_PvAI : MonoBehaviour
{
    public int xScore, oScore, xHighScore, oHighScore;
    public Text[] buttonList;
    public GameObject gameOverPanel;
    public Text gameOverText;
    public GameObject restartButton;
    public GameObject startInfo;
    public AudioSource winSound;
    public AudioSource drawSound;
    public int boardSize = 9;

    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;

    public bool playerMove;//AI
    public float delay = 10;//AI
    
    private string playerSide;
    private string computerSide;//AI
    private int value;//AI
    private bool gameOver;
    private int moveCount;

    private void Awake()
    {
        gameOverPanel.SetActive(false);
        SetGameControllerReferenceOnButtons();
        moveCount = 0;
        xScore = 0;
        oScore = 0;
        restartButton.SetActive(false);
        playerMove = true;//AI
        gameOver = false;
    }
    private void Start()
    {
        xHighScore = gameObject.GetComponent<GlobalSettingsControl>().GetXHS();//Loading X High score from save-file
        oHighScore = gameObject.GetComponent<GlobalSettingsControl>().GetOHS();//Loading O High score from save-file
    }
    /// <summary>
    ///Update function Handling AI behaviour through out the match
    /// </summary>
    private void Update()
    {
        //AI Behaviour
        if (!playerMove)//AI
        {
            delay += delay * Time.deltaTime;//AI
            if (delay >= 50)//AI
            {
                value = Random.Range(0, buttonList.Length - 1);//AI
                if (buttonList[value].GetComponentInParent<Button>().interactable == true)//AI
                {
                    buttonList[value].text = GetComputerSide();//AI
                    buttonList[value].GetComponentInParent<Button>().interactable = false;//AI
                    EndTurn();//AI
                }
            }
        }   
    }
    
    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);//interaction with particular cells of the board
        }
    }
    /// <summary>
    /// Setting player side to 'X' or 'O' based on button clicked on top of each borad
    /// </summary>
    public void SetStartingSide(string startingSide)
    {
        playerSide = startingSide;
        if (playerSide == "X")
        {
            computerSide = "O";//AI
            SetPlayerColors(playerX, playerO);//interchanging colors for alternating sides 
        }
        else
        {
            computerSide = "X";//AI
            SetPlayerColors(playerO, playerX);//interchanging colors for alternating sides
        }

        StartGame();
    }
    /// <summary>
    /// to get player side symbol
    /// </summary>
    public string GetPlayerSide()
    {
        return playerSide;
    }
    /// <summary>
    /// to get computer side symbol
    /// </summary>
    public string GetComputerSide()
    {
        return computerSide;
    }
    /// <summary>
    /// method to check winning condition and the end of each move played by computer and player
    /// </summary>
    public void EndTurn()
    {
        moveCount++;
        //different methods corresponding to different board size
        if(boardSize == 9)//3x3 board
        {
            Board3Check();
        }
        else if(boardSize == 16)//4x4 board
        {
            Board4Check();
        }
        else if(boardSize == 25)//5x5 board
        {
            Board5Check();
        }
        
        if(!gameOver)//gameover-flag to prevent switching sides when one side wins
        {
            ChangeSides();//changing X to O and vice versa
            delay = 10;//AI
        }
    }
    /// <summary>
    /// default state at the beginning of each game-session
    /// </summary>
    void StartGame()
    {
        SetBoardInteractable(true);
        SetPlayerButtons(false);
        startInfo.SetActive(false);
    }
    /// <summary>
    /// external reference through restart button
    /// </summary>
    public void RestartGame()
    {
        moveCount = 0;
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        SetPlayerButtons(true);
        SetPlayerColorsInactive();
        startInfo.SetActive(true);
        playerMove = true;//AI
        delay = 10;//AI
        gameOver = false;

        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].text = "";
        }
    }
    /// <summary>
    /// Set panel colors for side selection panels
    /// </summary>
    void SetPlayerColors(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }
    /// <summary>
    /// Game-Over state
    /// </summary>
    void GameOver(string winningPlayer)
    {
        SetBoardInteractable(false);
        
        if(winningPlayer == "draw")
        {
            SetGameOverText("It's a Draw!");
            drawSound.Play();
            SetPlayerColorsInactive();
        }
        else
        {
            SetGameOverText(winningPlayer + " Wins!");
            winSound.Play();
            if(winningPlayer == "X")
            {
                xScore++;
                if (xScore > xHighScore)
                {
                    xHighScore = xScore;
                    gameObject.GetComponent<GlobalSettingsControl>().XHighScoreUpdate(xHighScore);
                }
            }
            else if(winningPlayer == "O")
            {
                oScore++;
                if (oScore > oHighScore)
                {
                    oHighScore = oScore;
                    gameObject.GetComponent<GlobalSettingsControl>().OHighScoreUpdate(oHighScore);
                } 
            }
        }
        gameOver = true;
        restartButton.SetActive(true);
    }
    /// <summary>
    /// Changing Sides from O to X and vice versa during change state
    /// </summary>
    void ChangeSides()
    {
        //playerSide = (playerSide == "X") ? "O" : "X";//PvP
        playerMove = (playerMove == true) ? false : true;//AI
        //if(playerSide == "X")//PvP
        if(playerMove == true)//AI
        {
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            SetPlayerColors(playerO, playerX);
        }
    }
    /// <summary>
    /// Result State to show winning side
    /// </summary>
    void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
    }
    /// <summary>
    /// switch from non-interactable state to gameplay state
    /// </summary>
    void SetBoardInteractable(bool toggle)
    {
        for(int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }
    /// <summary>
    /// non-interactable state
    /// </summary>
    void SetPlayerButtons(bool toggle)
    {
        playerX.button.interactable = toggle;
        playerO.button.interactable = toggle;
    }
    /// <summary>
    /// inactive state for seide selection
    /// </summary>
    void SetPlayerColorsInactive()
    {
        playerX.panel.color = inactivePlayerColor.panelColor;
        playerX.text.color = inactivePlayerColor.textColor;
        playerO.panel.color = inactivePlayerColor.panelColor;
        playerO.text.color = inactivePlayerColor.textColor;
    }
    /// <summary>
    /// Win-check condition for 3x3 board 
    /// </summary>
    void Board3Check()
    {
        //win condition check
        int j;
        //check rows 
        j = 0;
        for (int i = 0; i < 3; i++)
        {
            if (buttonList[j].text == playerSide && buttonList[j + 1].text == playerSide && buttonList[j + 2].text == playerSide)
            {
                Debug.Log("Player check");
                GameOver(playerSide);
            }
            else if (buttonList[j].text == computerSide && buttonList[j + 1].text == computerSide && buttonList[j + 2].text == computerSide)
            { 
                ///AI
                Debug.Log("AI check");
                GameOver(computerSide);
                ///AI
            }
            j += 3;
        }

        //check columns
        j = 0;
        for (int i = 0; i < 3; i++)
        {
            if (buttonList[j].text == playerSide && buttonList[j + 3].text == playerSide && buttonList[j + 6].text == playerSide)
            {
                Debug.Log("Player check");
                GameOver(playerSide);   
            }
            else if (buttonList[j].text == computerSide && buttonList[j + 3].text == computerSide && buttonList[j + 6].text == computerSide)
            {
                ///AI
                Debug.Log("AI check");
                GameOver(computerSide);
                ///AI
            }
            j += 1;
        }

        //first diognal check
        if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide)
        {
            Debug.Log("Player check");
            GameOver(playerSide);
        }
        else if (buttonList[0].text == computerSide && buttonList[4].text == computerSide && buttonList[8].text == computerSide)
        {
            ///AI
            Debug.Log("AI check");
            GameOver(computerSide);
            ///AI
        }

        //second diognal check
        if (buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide)
        {
            Debug.Log("Player check");
            GameOver(playerSide);
        }
        else if (buttonList[2].text == computerSide && buttonList[4].text == computerSide && buttonList[6].text == computerSide)
        {
            ///AI
            Debug.Log("AI check");
            GameOver(computerSide);       
            ///AI
        }

        //draw condition
        if (moveCount == 9)
        {
            GameOver("draw");
        }
    }
    /// <summary>
    /// Win-check condition for 4x4 board
    /// </summary>
    void Board4Check()
    {
        //win condition check
        int j;
        //check rows 
        j = 0;
        for (int i = 0; i < 4; i++)
        {
            if (buttonList[j].text == playerSide && buttonList[j+1].text == playerSide && buttonList[j+2].text == playerSide && buttonList[j+3].text == playerSide)//player side check
            {
                GameOver(playerSide);
            }
            else if (buttonList[j].text == computerSide && buttonList[j + 1].text == computerSide && buttonList[j + 2].text == computerSide && buttonList[j + 3].text == computerSide)//computer side check
            {
                GameOver(computerSide);
            }
            j += 4;
        }
        //check columns
        j = 0;
        for (int i = 0; i < 4; i++)
        {
            if (buttonList[j].text == playerSide && buttonList[j+4].text == playerSide && buttonList[j+8].text == playerSide && buttonList[j+12].text == playerSide)//player side check
            {
                GameOver(playerSide);
            }
            else if (buttonList[j].text == computerSide && buttonList[j + 4].text == computerSide && buttonList[j + 8].text == computerSide && buttonList[j + 12].text == computerSide)//computer side check
            {
                ///AI
                GameOver(playerSide);
                ///AI
            }
            j += 1;
        }
        //first diognal check
        if (buttonList[0].text == playerSide && buttonList[5].text == playerSide && buttonList[10].text == playerSide && buttonList[15].text == playerSide)//player side check
        {
            GameOver(playerSide);
        }
        else if (buttonList[0].text == computerSide && buttonList[5].text == computerSide && buttonList[10].text == computerSide && buttonList[15].text == computerSide)//computer side check
        {
            ///AI
            GameOver(computerSide);
            ///AI
        }
        //second diognal check
        if (buttonList[3].text == playerSide && buttonList[6].text == playerSide && buttonList[9].text == playerSide && buttonList[12].text == playerSide)//player side check
        {
            GameOver(playerSide);
        }
        else if (buttonList[3].text == computerSide && buttonList[6].text == computerSide && buttonList[9].text == computerSide && buttonList[12].text == computerSide)//computer side check
        {
            ///AI
            GameOver(computerSide);
            ///AI
        }

        //draw condition
        if (moveCount == 16)
        {
            GameOver("draw");
        }
    }
    /// <summary>
    /// Win-check condition for 5x5 board
    /// </summary>
    void Board5Check()
    {
        //win condition check
        int j;
        //check rows 
        j = 0;
        for (int i = 0; i < 5; i++)
        {
            if (buttonList[j].text == playerSide && buttonList[j + 1].text == playerSide && buttonList[j + 2].text == playerSide && buttonList[j + 3].text == playerSide && buttonList[j + 4].text == playerSide)//player side check
            {
                GameOver(playerSide);
            }
            else if (buttonList[j].text == computerSide && buttonList[j + 1].text == computerSide && buttonList[j + 2].text == computerSide && buttonList[j + 3].text == computerSide && buttonList[j + 4].text == computerSide)//computer side check
            {
                ///AI
                GameOver(computerSide);
                ///AI
            }
            j += 5;
        }
        //check columns
        j = 0;
        for (int i = 0; i < 5; i++)
        {
            if (buttonList[j].text == playerSide && buttonList[j + 5].text == playerSide && buttonList[j + 10].text == playerSide && buttonList[j + 15].text == playerSide && buttonList[j + 20].text == playerSide)//player side check
            {
                GameOver(playerSide);
            }
            else if (buttonList[j].text == computerSide && buttonList[j + 5].text == computerSide && buttonList[j + 10].text == computerSide && buttonList[j + 15].text == computerSide && buttonList[j + 20].text == computerSide)//computer side check
            {
                ///AI
                GameOver(computerSide);
                ///AI
            }
            j += 1;
        }
        //diognal check
        if (buttonList[0].text == playerSide && buttonList[6].text == playerSide && buttonList[12].text == playerSide && buttonList[18].text == playerSide && buttonList[24].text == playerSide)//player side check
        {
            GameOver(playerSide);
        }
        else if (buttonList[0].text == computerSide && buttonList[6].text == computerSide && buttonList[12].text == computerSide && buttonList[18].text == computerSide && buttonList[24].text == computerSide)//computer side check
        {
            ///AI
            GameOver(computerSide);
            ///AI
        }
        if (buttonList[4].text == playerSide && buttonList[8].text == playerSide && buttonList[12].text == playerSide && buttonList[16].text == playerSide && buttonList[20].text == playerSide)//player side check
        {
            GameOver(playerSide);
        }
        else if (buttonList[4].text == computerSide && buttonList[8].text == computerSide && buttonList[12].text == computerSide && buttonList[16].text == computerSide && buttonList[20].text == computerSide)//computer side check
        {
            ///AI
            GameOver(computerSide);
            ///AI
        }

        //draw condition
        if (moveCount == 25)
        {
            GameOver("draw");
        }
    }
}