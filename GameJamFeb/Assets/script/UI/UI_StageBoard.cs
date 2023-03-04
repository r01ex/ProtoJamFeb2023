using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StageBoard : MonoBehaviour
{
    [SerializeField] RectTransform[] _sockets;
    [SerializeField] UI_StageDetail _detail;

    [SerializeField]
    private GameObject _stageButtonPrefab;

    private Dictionary<StageSO, UI_StageButton> _stageButtons = new Dictionary<StageSO, UI_StageButton>();
    
    public void CreateStageButton(StageSO InData)
    {
        if(_sockets.Length > _stageButtons.Count)
        {
            if (_stageButtons.ContainsKey(InData) == false)
            {
                UI_StageButton stageButton = GameObject.Instantiate(_stageButtonPrefab).GetComponent<UI_StageButton>();

                stageButton.gameObject.transform.SetParent(_sockets[_stageButtons.Count]);

                stageButton.transform.localPosition = Vector3.zero;
                stageButton.transform.localRotation = Quaternion.identity;
                stageButton.transform.localScale = Vector3.one;


                stageButton.SetStage(InData, (int i)=>{
                    SelectStage(i);
                });
                _stageButtons.Add(InData, stageButton);
            }
        }
        else
        {
            Debug.Log("no more sockets to add button!");
        }
    }

    public void SelectStage(int n)
    {
        if (_detail.GoUp())
        {
            _detail.SetDetail(n);
        }
    }
}
