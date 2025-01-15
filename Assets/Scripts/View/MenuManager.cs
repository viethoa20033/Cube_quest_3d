using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void Buttonlevel()
    {
        GameManager.Instance.UpdateGameState(GameState.Playing);
    }

    public void ButtonSetting()
    {
        GameManager.Instance.UpdateGameState(GameState.GameSetting);
    }

    public void ButtonExit()
    {
        GameManager.Instance.UpdateGameState(GameState.Playing);
    }

    public void ButtonChooseLevel()
    {
        GameManager.Instance.UpdateGameState(GameState.ChooseLevel);
    }
}

