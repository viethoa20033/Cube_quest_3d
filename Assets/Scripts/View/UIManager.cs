using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject lockClick;
    public CanvasGroup blackPanel;
    
    [Header("Choose Level")]
    public GameObject chooseLevel;
    public CanvasGroup bg;
    public RectTransform head;
    public RectTransform levelButton;
    public Button[] levelButtons;


    [Header("Game Play")] 
    public RectTransform gameplay;
    public RectTransform gameSetting;


    [Header("Text")] 
    public Text coutLegText;
    public Text woodCubeText;
    
    
    [Header("Text Complete next level")] 
    public Text completeText;
    public string completeString;
    private string currentStringComplete;

    [Header("Change Music")] public bool isMusic;
    public Image musicImage;
    public Sprite[] musicIcons;

    private void Start()
    {
        GameManager.OnGameStateChanged += UpdateGameState;

        GameController.OnCoutLegChanged += UpdateLeg;
        GameController.OnCoutTargetChanged += UpdateWoodTarget;

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int index = i;
            levelButtons[i].onClick.AddListener(() => LevelButtonClick(index));
        }


        isMusic = PlayerPrefs.GetInt("isMusic", 1) == 1;
        if (isMusic)
        {
            AudioListener.volume = 1;
            musicImage.sprite = musicIcons[1];
        }
        else
        {
            AudioListener.volume = 0;
            musicImage.sprite = musicIcons[0];
        }
        
        head.anchoredPosition = new Vector2(0, Screen.height);
        head.DOAnchorPos(new Vector2(0, -125), 1f).SetEase(Ease.OutBack);

        StartCoroutine(LevelButtonAnimation());

    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= UpdateGameState;
        
        GameController.OnCoutLegChanged -= UpdateLeg;
        GameController.OnCoutTargetChanged -= UpdateWoodTarget;
    }
    void UpdateGameState(GameState state)
    {
        switch (state)
        {
            case GameState.ChooseLevel:
                HandleChooseLevel();
                break;
            
            case GameState.Playing:
                HandlePlaying();
                break;
            
            case GameState.GameSetting:
                HandleGameSetting();
                break;
            
            case GameState.GameSuccess:
                HandleGameSuccess();
                break;
        }
    }

    void HandleChooseLevel()
    {
        Time.timeScale = 1;
        
        lockClick.SetActive(true);
        
        chooseLevel.SetActive(true);
        bg.DOFade(1, 1f);
        
        gameplay.DOAnchorPos(new Vector2(0, Screen.height), 1f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            lockClick.SetActive(false);
            
            gameplay.gameObject.SetActive(false);
            gameSetting.gameObject.SetActive(false);
            
            
            head.anchoredPosition = new Vector2(0, Screen.height);
            head.DOAnchorPos(new Vector2(0, -125), 1f).SetEase(Ease.OutBack);
            
            levelButton.anchoredPosition = new Vector2(0, -Screen.height);
            levelButton.DOAnchorPos(new Vector2(0, -75), 1f).SetEase(Ease.OutBack);

            StartCoroutine(LevelButtonAnimation());

        });
    }

    void HandlePlaying()
    {
        Time.timeScale = 1;
        
        lockClick.SetActive(true);

        StartCoroutine(FalseLockButton());
        
        if (chooseLevel.activeInHierarchy)
        {
            bg.DOFade(0, 1f);
            head.DOAnchorPos(new Vector2(0, Screen.height), 1f).SetEase(Ease.InBack);
            levelButton.DOAnchorPos(new Vector2(0, -Screen.height), 1f).SetEase(Ease.InBack).OnComplete(() =>
            {
                lockClick.SetActive(false);
                
                chooseLevel.SetActive(false);

                gameplay.gameObject.SetActive(true);

                gameplay.anchoredPosition = new Vector2(0, Screen.height);
                gameplay.DOAnchorPos(Vector2.zero, 1f).SetEase(Ease.OutBack);
            });
        }

        if (gameSetting.gameObject.activeInHierarchy)
        {
            gameSetting.DOAnchorPos(new Vector2(0, Screen.height), .5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                lockClick.SetActive(false);

                gameSetting.gameObject.SetActive(false);
            });
        }
    }

    void HandleGameSetting()
    {
        if (GameManager.Instance.isPlaying)
        {
            Time.timeScale = 0;

            lockClick.SetActive(true);
            gameSetting.gameObject.SetActive(true);
            gameSetting.anchoredPosition = new Vector2(0, Screen.height);
            gameSetting.DOAnchorPos(Vector2.zero, .5f).SetEase(Ease.OutBack).SetUpdate(true).OnComplete(() =>
            {
                lockClick.SetActive(false);
            });
        }
    }

    void HandleGameSuccess()
    {
        lockClick.SetActive(true);
        
        completeText.gameObject.SetActive(true);
        StartCoroutine(RevealText());
    }
    
    void UpdateLeg(int coutLeg)
    {
        coutLegText.text = coutLeg.ToString();
    }

    void UpdateWoodTarget(int woodCout)
    {
        woodCubeText.text = woodCout + "/" + GameController.Instance.coutTarget;
    }

    void LevelButtonClick(int index)
    {
        LevelManager.Instance.SetLevel(index + 1);
    }

    public void ChangeMusic()
    {
        isMusic = !isMusic;

        if (isMusic)
        {
            AudioListener.volume = 1;
            musicImage.sprite = musicIcons[1];
        }
        else
        {
            AudioListener.volume = 0;
            musicImage.sprite = musicIcons[0];
        }
        
        PlayerPrefs.SetFloat("isMusic", isMusic ? 1 : 0);
        PlayerPrefs.Save();
    }
    IEnumerator LevelButtonAnimation()
    {
        foreach (var levelButton in levelButtons)
        {
            levelButton.transform.localScale = Vector3.zero;
        }

        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(.25f);
        }
    }

    IEnumerator FalseLockButton()
    {
        yield return new WaitForSeconds(1f);
        lockClick.SetActive(false);
    }
    
    IEnumerator RevealText()
    {
        completeText.text = "";
        for (int i = 0; i <= completeString.Length; i++)
        {
            currentStringComplete = completeString.Substring(0, i);
            completeText.text = currentStringComplete;
            yield return new WaitForSeconds(.05f);
        }

        yield return new WaitForSeconds(1f);
        
        //Black panel to next level
        
        //in black panel
        blackPanel.alpha = 0;
        blackPanel.DOFade(1, 1);
        
        //waiting 1.5f
        yield return new WaitForSeconds(1.5f);
        
        //out black panel
        completeText.gameObject.SetActive(false);
        blackPanel.DOFade(0, 1);
    }
}
