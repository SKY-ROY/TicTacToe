using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    public Button button;
    public Text buttonText;
    
    private GameController_PvAI gameController;

    public void SetSpace()
    {
        if (gameController.playerMove)                          //for player's turn
        {
            buttonText.text = gameController.GetPlayerSide();   //assigning a particular symbol from 'O' or 'X'
            button.interactable = false;                        //cell interaction disabled after marking with any side
            gameController.EndTurn();                           //check win conditions after every player turn
        }
    }
    public void SetGameControllerReference(GameController_PvAI controller)
    {
        gameController = controller;
    }
}
