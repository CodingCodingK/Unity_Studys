using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Newtonsoft.Json.Bson;
using UnityEngine.UI;

public class View : MonoBehaviour
{
    private RectTransform logoName;
    private RectTransform menuUI;
    private RectTransform gameUI;
    private GameObject restartButton;

    private Text score;
    private Text highScore;

    // Start is called before the first frame update
    void Awake()
    {
        logoName = transform.Find("Canvas/LogoName") as RectTransform;
        menuUI = transform.Find("Canvas/MenuUI") as RectTransform;
        gameUI = transform.Find("Canvas/GameUI") as RectTransform;
        restartButton = transform.Find("Canvas/MenuUI/RestartButton").gameObject;
        score = transform.Find("Canvas/GameUI/ScoreLabel/Text").gameObject.GetComponent<Text>();
        highScore = transform.Find("Canvas/GameUI/HighScoreLabel/Text").gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMenu()
    {
        logoName.gameObject.SetActive(true);
        logoName.DOAnchorPosY(-72.17438f,0.8f);
        menuUI.gameObject.SetActive(true);
        menuUI.DOAnchorPosY(36.67181f,0.8f);
    }
    
    public void HideMenu()
    {
        logoName.DOAnchorPosY(75f,0.5f).OnComplete(()=> { logoName.gameObject.SetActive(false); });
        menuUI.DOAnchorPosY(-36.67181f,0.5f).OnComplete(()=>{menuUI.gameObject.SetActive(false); });
    }

    public void UpdateGameUI(int score, int highScore)
    {
        // 更新GameUI面板下的Score。
        this.score.text = score.ToString();
        this.highScore.text = highScore.ToString();
    }
    
    public void ShowGameUI(int score, int highScore)
    {
        // 更新GameUI面板下的Score。
        UpdateGameUI(score, highScore);
        
        gameUI.gameObject.SetActive(true);
        gameUI.DOAnchorPosY(-61.587f,0.8f);
    }
    
    public void HideGameUI()
    {
        gameUI.gameObject.SetActive(false);
        gameUI.DOAnchorPosY(61.587f,0.8f);
    }

    public void ShowRestartButton()
    {
        if (restartButton.activeSelf) return;
        
        restartButton.SetActive(true);
    }
    
    public void HideRestartButton()
    {
        restartButton.SetActive(false);
    }
}
