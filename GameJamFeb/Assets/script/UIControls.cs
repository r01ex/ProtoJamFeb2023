using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIControls : MonoBehaviour
{
    int TOTALSTAGECOUNT = 2;

    bool isinStageSelect = false;
    [SerializeField] GameObject stageimage;

    [SerializeField] GameObject stageCanvas;
    [SerializeField] GameObject homeCanvas;
    [SerializeField] GameObject playBtn;
    [SerializeField] GameObject nextBtn;
    [SerializeField] GameObject prevBtn;
    int currentStage = 0;
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
        if (n >= 0 && n < TOTALSTAGECOUNT)
        {
            Debug.Log("changing to stage " + n);
            currentStage = n;
            stageimage.GetComponent<Image>().sprite = StageReference.Instance.stageSpriteList[n];
            playBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            playBtn.GetComponent<Button>().onClick.AddListener(delegate { gotoStage(n); });
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
        if(n==(TOTALSTAGECOUNT - 1))
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
        changeSelectedStage(currentStage + 1);
    }
    public void prevStageSelect()
    {
        changeSelectedStage(currentStage - 1);
    }
    public void goBacktoHome()
    {
        isinStageSelect = false;
        homeCanvas.SetActive(true);
        stageCanvas.SetActive(false);
    }
    
    public void gotoStage(int stagenum)
    {
        SceneManager.LoadScene(StageReference.Instance.sceneNameList[stagenum]);
    }
}
