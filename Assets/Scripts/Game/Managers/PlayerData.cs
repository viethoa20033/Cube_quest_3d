using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerData : MonoBehaviour
{
    public GameObject levelLock;
    public Text levelText;

    public Transform[] levelButtons;

    private void Start()
    {
        LoadData();

        GameManager.OnGameStateChanged += UpdateGameState;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= UpdateGameState;
    }

    void UpdateGameState(GameState state)
    {
        if (state == GameState.GameSuccess)
        {
            SaveData(LevelManager.Instance.level);
        }
    }

    public void LoadData()
    {
        int level = PlayerPrefs.GetInt("level", 0);

        foreach (Transform levelButton in levelButtons)
        {
            levelButton.GetComponent<Button>().interactable = false;
            foreach (Transform _levelButton in levelButton)
            {
                Destroy(_levelButton.gameObject);
            }
        }

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if(i <= level)
            {
                levelButtons[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                Instantiate(levelLock, levelButtons[i]);
            }
            
            Text _levelText = Instantiate(levelText, levelButtons[i]);
            _levelText.text = (i + 1).ToString();
        }
    }

    public void SaveData(int level)
    {
        int levelMax = PlayerPrefs.GetInt("level", 0);

        if (level > levelMax)
        {
            PlayerPrefs.SetInt("level", level);
            PlayerPrefs.Save();
            
            LoadData();
            
            Debug.Log("Save Level " + level);
        }
    }
}
