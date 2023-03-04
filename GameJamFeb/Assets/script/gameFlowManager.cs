using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
public class gameFlowManager : MonoBehaviour
{
    public UI_HUD HUD;
    [SerializeField] GameObject gameOverCanvasPrefab;
    [SerializeField] int stageNum;
    int TOTALSTAGECOUNT = 6;
    public static gameFlowManager Instance { get; private set; }
    int FragileCount;
    int TotalFragileCount;
    public void RegistFragile() { TotalFragileCount++; }
    float gametime = 0;
    [SerializeField] float maxTime;
    public int successCount = 0;
    public int currentSuccessCount = 0;
    bool isgameOver = false;
    public UnityEvent stageEndEvent;
    public int fragileCount
    {
        get
        {
            return FragileCount;
        }
        set
        {
            FragileCount = value;
            if(FragileCount==0 || FragileCount == currentSuccessCount)
            {
                Debug.Log("GAMEOVER CALLED");
                gameOver();
            }
        }
    }
   
    public void gameOver()
    {
        Debug.Log("game over");
        isgameOver = true;
        DataHandler.Instance.changeStageData(stageNum, true, gametime, successCount);
        DataHandler.Instance.Save();
        GameOverCanvas gameovercanvas = Instantiate(gameOverCanvasPrefab).GetComponent<GameOverCanvas>();
        gameovercanvas.timeText.text = "time: "+((Mathf.Round(gametime*100))/100).ToString();
        gameovercanvas.scoreText.text = "successCount: "+ currentSuccessCount.ToString() + "/" + TotalFragileCount.ToString();
        if (stageNum >= TOTALSTAGECOUNT - 1)
        {
            gameovercanvas.nextBtn.interactable = false;
        }
        gameovercanvas.nextBtn.onClick.AddListener(nextStage);
        gameovercanvas.retryBtn.onClick.AddListener(retryStage);
        gameovercanvas.stageBtn.onClick.AddListener(stageSelect);
        stageEndEvent.Invoke();
    }
    void gameTimeOver()
    {
        isgameOver = true;
        GameOverCanvas gameovercanvas = Instantiate(gameOverCanvasPrefab).GetComponent<GameOverCanvas>();
        gameovercanvas.timeText.text = "timeOVER";
        gameovercanvas.scoreText.text = "successCount: " + currentSuccessCount.ToString() + "/" + TotalFragileCount.ToString();
        gameovercanvas.nextBtn.interactable = false;  
        gameovercanvas.retryBtn.onClick.AddListener(retryStage);
        gameovercanvas.stageBtn.onClick.AddListener(stageSelect);
        stageEndEvent.Invoke();
    }

    public void nextStage()
    {
        SceneManager.LoadScene(this.gameObject.scene.buildIndex + 1);
    }
    public void retryStage()
    {
        SceneManager.LoadScene(this.gameObject.scene.name);
    }
    public void stageSelect()
    {
        SceneManager.LoadScene("HomeAndStageselect",LoadSceneMode.Additive);   
    }

    public void changeCurrentSuccessCount(int addamount)
    {
        currentSuccessCount += addamount;
        if(successCount<currentSuccessCount)
        {
            successCount = currentSuccessCount;
        }
        Debug.Log("on change count cur is : " + currentSuccessCount + "  fragile is: " + fragileCount);
        if(fragileCount==currentSuccessCount)
        {
            if (isgameOver == false)
            {
                gameOver();
            }
        }
    }
    public void fragileBreak()
    {
        fragileCount--;
    }
    private void Awake()
    {
        if(Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }
    private void Start()
    {
        FragileCount = TotalFragileCount;
        gametime = 0;
    }
    private void Update()
    {
        if (isgameOver == false)
        {
            gametime += Time.deltaTime;
            if (gametime >= maxTime)
            {
                gameTimeOver();
            }

            HUD?.UpdateHUD(gametime, TotalFragileCount, successCount, currentSuccessCount, fragileCount);
        }
    }
   
}
