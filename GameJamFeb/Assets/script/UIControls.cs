using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;
public class UIControls : MonoBehaviour
{
    const int TOTAL_STAGE_PAGE_COUNT = 2;
    const int STAGES_PER_PAGE = 3;

    [SerializeField] Image stageimage1;
    [SerializeField] Image stageimage2;
    [SerializeField] Image stageimage3;
    [SerializeField] MultiImageBtn st1Btn;
    [SerializeField] MultiImageBtn st2Btn;
    [SerializeField] MultiImageBtn st3Btn;

    [SerializeField] UI_StageBoard _board;
    [SerializeField] UI_Title _title;
    [SerializeField] GameObject nextBtn;
    [SerializeField] GameObject prevBtn;

    [SerializeField] GameObject backBtn;
    [SerializeField] GameObject playBtn;
    [SerializeField] UI_StageDetail _detail;
    [SerializeField] GameObject selectedstageImg;
    [SerializeField] TextMeshProUGUI stageInfoText;
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
        if (mode == LoadSceneMode.Additive)
        {
            Debug.Log(SceneManager.GetSceneAt(0).name);
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));
            isForStageSelect = true;
        }
    }
    public void Start()
    {
        if (isForStageSelect)
        {
            gotoStageSelection();
        }
    }
    public void gotoStageSelection()
    {
        if (_title.GoUp())
        {
            _board.gameObject.SetActive(true);
            //changeSelectedStage(0);
            InitStages();
        }
    }
    public void goBacktoHome()
    {
        _title.GoDown(delegate { _board.gameObject.SetActive(false); });
    }

    public void InitStages()
    {
        var DataHandler = global::DataHandler.Instance;
        DataHandler.Load();
        var Datas = DataHandler.gameData;

        for (int i = 0; i < Datas.isCleared.Length; ++i)
        {
            bool isLastStage = !Datas.isCleared[i];

            _board.CreateStageButton(StageReference.Instance.StageDatas[i]);

            if (isLastStage)
                break;
        }
    }



    public void changeSelectedStage(int n)
    {
        DataHandler.Instance.Load();
        if (n >= 0 && n < TOTAL_STAGE_PAGE_COUNT)
        {
            Debug.Log("changing to stage " + n);
            currentStagePage = n;
            st1Btn.gameObject.SetActive(false);
            st2Btn.gameObject.SetActive(false);
            st3Btn.gameObject.SetActive(false);
            try
            {
                stageimage1.sprite = StageReference.Instance.StageDatas[STAGES_PER_PAGE * n].Portrait;
                st1Btn.gameObject.SetActive(true);
                st1Btn.onClick.AddListener(delegate { selectStage(STAGES_PER_PAGE * n); });

                stageimage2.sprite = StageReference.Instance.StageDatas[(STAGES_PER_PAGE * n) + 1].Portrait;
                st2Btn.gameObject.SetActive(true);
                st2Btn.onClick.AddListener(delegate { selectStage((STAGES_PER_PAGE * n) + 1); });

                stageimage3.sprite = StageReference.Instance.StageDatas[(STAGES_PER_PAGE * n) + 2].Portrait;
                st3Btn.gameObject.SetActive(true);
                st3Btn.onClick.AddListener(delegate { selectStage((STAGES_PER_PAGE * n) + 2); });
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        else
        {
            Debug.Log("no stage avail");
        }

        prevBtn.GetComponent<Button>().interactable = (n != 0);

        nextBtn.GetComponent<Button>().interactable = (n != (TOTAL_STAGE_PAGE_COUNT - 1));
    }
    public void nextStageSelect()
    {
        changeSelectedStage(currentStagePage + 1);
    }
    public void prevStageSelect()
    {
        changeSelectedStage(currentStagePage - 1);
    }

    public void selectStage(int n)
    {
        if(_detail.GoUp())
        {
            _detail.SetDetail(n);
        }
    }
}
