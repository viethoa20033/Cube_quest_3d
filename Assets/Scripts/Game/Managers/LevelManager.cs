using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : SingletonBase<LevelManager>
{
    public static UnityAction<int> OnLevelChanged;
    
    public int level;

    public GameObject[] mapLevels;

  

    public void SetLevel(int _level)
    {
        level = _level;
        OnLevelChanged?.Invoke(level);

        LoadMapLevels();
    }
    public void NextLevel()
    {
        if (level < 15)
        {
            level++;
            LoadMapLevels();
            OnLevelChanged?.Invoke(level);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void LoadMapLevels()
    {
        GameObject[] maps = GameObject.FindGameObjectsWithTag("map");
        foreach (var map in maps)
        {
            Destroy(map.gameObject);
        }
        Instantiate(mapLevels[level - 1]);
        
        GameController.Instance.ResetCoutLeg();
    }

}
