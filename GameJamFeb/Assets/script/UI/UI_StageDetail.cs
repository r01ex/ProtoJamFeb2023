using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UI_StageDetail : MonoBehaviour
{
    [SerializeField] float _shiftTime = 1;
    [SerializeField] UI_FragileGrid _grid;
    Vector3 _initalPosition;
    bool _isMoving = false;
    StageSO SO;

    private void Awake()
    {
        _initalPosition = GetComponent<RectTransform>().localPosition;
    }

    public bool GoUp()
    {
        if (_isMoving == false)
        {
            _isMoving = true;

            transform.DOLocalMoveY(0, _shiftTime).From(_initalPosition).OnComplete(() =>
            {
                _isMoving = false;
            });

            return true;
        }
        else
        {
            return false;
        }
    }

    public void GoDown()
    {
        if (_isMoving == false)
        {
            _isMoving = true;

            transform.DOLocalMoveY(_initalPosition.y, _shiftTime).OnComplete(() =>
            {
                _isMoving = false;
            });
        }
    }

    public void SetDetail(int InIndex)
    {
        var Data = DataHandler.Instance.gameData;
        SO = StageReference.Instance.StageDatas[InIndex];

        if(Data.isCleared[InIndex])
        {
            //stageInfoText.text = "Time : " + Data.clearTime[n]
            _grid.SetCount(Data.userScore[InIndex], Data.userScore[InIndex], 0);
        }
        else
        {
            //stageInfoText.text = "Not Cleared";
            _grid.SetCount(SO.NeedFragiileCount, 0, 0);
        }
        //selectedstageImg.GetComponent<Image>().sprite = StageReference.Instance.StageDatas[n].Portrait;
    }

    public void GoToStage()
    {
        SceneManager.LoadScene(SO.SceneName);
    }
}
