using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageButton : MonoBehaviour
{
    [SerializeField] Image _portrait;

    public delegate void Callback(int i);
    StageSO SO;
    Callback callback;

    public void SetStage(StageSO InData, Callback InFunc)
    {
        gameObject.SetActive(true);
        callback = InFunc;
        SO = InData;

        _portrait.sprite = InData.Portrait;
    }

    public void Execute()
    {
        callback?.Invoke(SO.StageIndex);
    }
}
