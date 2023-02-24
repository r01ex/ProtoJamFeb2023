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
    [SerializeField] Image stageimage1;
    [SerializeField] Image stageimage2;
    [SerializeField] Image stageimage3;
    [SerializeField] MultiImageBtn st1Btn;
    [SerializeField] MultiImageBtn st2Btn;
    [SerializeField] MultiImageBtn st3Btn;

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
            st1Btn.gameObject.SetActive(false);
            st2Btn.gameObject.SetActive(false);
            st3Btn.gameObject.SetActive(false);
            try
            {
                stageimage1.sprite = StageReference.Instance.stageSpriteList[3*n];
                st1Btn.gameObject.SetActive(true);
                st1Btn.onClick.AddListener(delegate { selectStage(3 * n); });
                
                stageimage2.sprite = StageReference.Instance.stageSpriteList[(3*n)+1];
                st2Btn.gameObject.SetActive(true);
                st2Btn.onClick.AddListener(delegate { selectStage((3 * n) + 1); });
               
                stageimage3.sprite = StageReference.Instance.stageSpriteList[(3*n)+2];
                st3Btn.gameObject.SetActive(true);
                st3Btn.onClick.AddListener(delegate { selectStage((3 * n) + 2); });
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
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
