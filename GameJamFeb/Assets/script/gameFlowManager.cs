using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;
public class gameFlowManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverCanvasPrefab;
    [SerializeField] int stageNum;
    int TOTALSTAGECOUNT = 6;
    public gameFlowManager Instance { get; private set; }
    [SerializeField] int FragileCount;
    float gametime = 0;
    [SerializeField] int maxTime;
    public int successCount = 0;
    public int fragileCount
    {
        get
        {
            return FragileCount;
        }
        set
        {
            FragileCount = value;
            if(FragileCount==0)
            {
                gameOver();
            }
        }
    }
    
    public void gameOver()
    {
        DataHandler.Instance.changeStageData(stageNum, true, gametime, successCount);
        DataHandler.Instance.Save();
        GameOverCanvas gameovercanvas = Instantiate(gameOverCanvasPrefab).GetComponent<GameOverCanvas>();
        gameovercanvas.timeText.text = ((Mathf.Round(gametime*100))/100).ToString();
        gameovercanvas.scoreText.text = successCount.ToString() + "/" + fragileCount.ToString();
        if (stageNum >= TOTALSTAGECOUNT - 1)
        {
            gameovercanvas.nextBtn.interactable = false;
        }
        gameovercanvas.nextBtn.onClick.AddListener(nextStage);
        gameovercanvas.retryBtn.onClick.AddListener(retryStage);
        gameovercanvas.stageBtn.onClick.AddListener(stageSelect);
    }
    void gameTimeOver()
    {
        
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

    private void Awake()
    {
        if(Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }
    private void Start()
    {
        gametime = 0;
    }
    private void Update()
    {
        gametime += Time.deltaTime;
        if(gametime == maxTime)
        {
            gameTimeOver();
        }
    }
   
}
