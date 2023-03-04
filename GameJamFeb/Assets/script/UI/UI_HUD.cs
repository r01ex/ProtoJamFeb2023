using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HUD : MonoBehaviour
{
    [SerializeField] StageSO stageSO;
    [SerializeField] Image timer;
    [SerializeField] UI_FragileGrid grid;



    public void UpdateHUD(float currentTime, int totalFragile, int savedFragile, int currentSafeFragile, int aliveFragile)
    {
        grid.SetCount(totalFragile, currentSafeFragile, totalFragile - aliveFragile);
        grid.FragileSprite = stageSO.FragileImage;
        grid.BrokenSprite = stageSO.BrokenFragileImage;
        timer.fillAmount = 1 - currentTime / stageSO.TimeLimit;
        timer.color = new Color(0.5f + (1 - timer.fillAmount) * 0.5f, 0.5f + timer.fillAmount * 0.5f, 0.5f);
    }
}
