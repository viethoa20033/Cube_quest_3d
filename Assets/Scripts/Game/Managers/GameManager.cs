using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : SingletonBase<GameManager>
{
    public static UnityAction<GameState> OnGameStateChanged;

    public GameState gameState;
    public bool isPlaying;

    [Header("Particle")]
    public GameObject[] particles;
    public Transform fxPointPlayer;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }

    public void UpdateGameState(GameState newState)
    {
        gameState = newState;

        switch (newState)
        {
            case GameState.ChooseLevel:
                break;
            
            case GameState.Playing:
                HandlePlaying();
                break;
            
            case GameState.GameSetting:
                break;
            
            case GameState.GameSuccess:
                HandleGameSuccess();
                break;
        }
        OnGameStateChanged?.Invoke(gameState);
    }

    void HandlePlaying()
    {
        if (!isPlaying)
        {
            StartCoroutine(DelayPlay());
        }
    }

    void HandleGameSuccess()
    {
        StopAllCoroutines();
        StartCoroutine(NextLevel());
    }
    
    IEnumerator DelayPlay()
    {
        yield return new WaitForSeconds(.25f);
        isPlaying = true;
    }

    IEnumerator NextLevel()
    {
        
        foreach (var par in particles)
        {
            GameObject _par = Instantiate(par, fxPointPlayer.position, Quaternion.identity);
            Destroy(_par, 3f);
        }
        
        isPlaying = false;
        yield return new WaitForSeconds(3f);
        
        
        LevelManager.Instance.NextLevel();
        UpdateGameState(GameState.Playing);
    }
}

public enum GameState
{
    ChooseLevel,
    Playing,
    GameSetting,
    GameSuccess
}
