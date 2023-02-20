using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StageReference : MonoBehaviour
{
    public static StageReference Instance { get; private set; }
    public List<Sprite> stageSpriteList = new List<Sprite>();
    public List<string> stageNameList = new List<string>();
    public List<string> sceneNameList = new List<string>();
    public List<Sprite> selectedstageSpriteList = new List<Sprite>();
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }
}
