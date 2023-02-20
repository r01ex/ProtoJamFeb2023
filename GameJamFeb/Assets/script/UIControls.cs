using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class UIControls : MonoBehaviour
{
    int TOTALSTAGEPAGECOUNT = 2;

    bool isinStageSelect = false;
    [SerializeField] GameObject stageimage1;
    [SerializeField] GameObject stageimage2;
    [SerializeField] GameObject stageimage3;

    [SerializeField] GameObject stageCanvas;
    [SerializeField] GameObject homeCanvas;
    [SerializeField] GameObject nextBtn;
    [SerializeField] GameObject prevBtn;

    [SerializeField] GameObject backBtn;
    [SerializeField] GameObject playBtn;
    [SerializeField] GameObject stageSelectPanel;
    [SerializeField] GameObject selectedstageImg;
    int currentStagePage = 0;
    bool isForStageSelect = false;
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        if(mode == LoadSceneMode.Additive)
        {
            Debug.Log(SceneManager.GetSceneAt(0).name);
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));
            isForStageSelect = true;
        }
    }
    public void Start()
    {
        if(isForStageSelect)
        {
            gotoStageSelection();
        }
    }
    public void gotoStageSelection()
    {
        isinStageSelect = true;
        homeCanvas.SetActive(false);
        stageCanvas.SetActive(true);
        changeSelectedStage(0);

    }
    public void changeSelectedStage(int n)
    {
        if (n >= 0 && n < TOTALSTAGEPAGECOUNT)
        {
            Debug.Log("changing to stage " + n);
            currentStagePage = n;
            stageimage1.SetActive(false);
            stageimage2.SetActive(false);
            stageimage3.SetActive(false);
            try
            {
                stageimage1.GetComponent<Image>().sprite = StageReference.Instance.stageSpriteList[3*n];
                stageimage1.SetActive(true);
                stageimage1.GetComponent<Button>().onClick.AddListener(delegate { selectStage(3 * n); });
                stageimage2.GetComponent<Image>().sprite = StageReference.Instance.stageSpriteList[(3*n)+1];
                stageimage2.SetActive(true);
                stageimage2.GetComponent<Button>().onClick.AddListener(delegate { selectStage((3 * n) + 1); });
                stageimage3.GetComponent<Image>().sprite = StageReference.Instance.stageSpriteList[(3*n)+2];
                stageimage3.SetActive(true);
                stageimage3.GetComponent<Button>().onClick.AddListener(delegate { selectStage((3 * n) + 2); });
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
            //playBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            //playBtn.GetComponent<Button>().onClick.AddListener(delegate { gotoStage(n); });
        }
        else
        {
            Debug.Log("no stage avail");
        }
        if(n==0)
        {
            prevBtn.GetComponent<Button>().interactable = false;
        }
        else
        {
            prevBtn.GetComponent<Button>().interactable = true;
        }
        if(n==(TOTALSTAGEPAGECOUNT - 1))
        {
            nextBtn.GetComponent<Button>().interactable = false;
        }
        else
        {
            nextBtn.GetComponent<Button>().interactable = true;
        }
    }
    public void nextStageSelect()
    {
        changeSelectedStage(currentStagePage + 1);
    }
    public void prevStageSelect()
    {
        changeSelectedStage(currentStagePage - 1);
    }
    public void goBacktoHome()
    {
        isinStageSelect = false;
        homeCanvas.SetActive(true);
        stageCanvas.SetActive(false);
    }
    public void selectStage(int n)
    {
        stageSelectPanel.SetActive(true);
        selectedstageImg.GetComponent<Image>().sprite = StageReference.Instance.selectedstageSpriteList[n];
        playBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        playBtn.GetComponent<Button>().onClick.AddListener(delegate { gotoStage(n); });
        backBtn.GetComponent<Button>().onClick.AddListener(delegate { stageSelectPanel.SetActive(false); });
    }
    public void gotoStage(int stagenum)
    {
        SceneManager.LoadScene(StageReference.Instance.sceneNameList[stagenum]);
    }
}
