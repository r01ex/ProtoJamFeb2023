using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIControls : MonoBehaviour
{
    int MAXSTAGE = 2;

    bool isinStageSelect = false;
    [SerializeField] GameObject stageimage;

    [SerializeField] GameObject stageCanvas;
    [SerializeField] GameObject homeCanvas;
    [SerializeField] GameObject playBtn;
    [SerializeField] GameObject nextBtn;
    [SerializeField] GameObject prevBtn;
    int currentStage = 0;
    public void gotoStageSelection()
    {
        isinStageSelect = true;
        homeCanvas.SetActive(false);
        stageCanvas.SetActive(true);
        changeSelectedStage(0);

    }
    public void changeSelectedStage(int n)
    {
        if (n >= 0 && n < MAXSTAGE)
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
        if(n==(MAXSTAGE-1))
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
