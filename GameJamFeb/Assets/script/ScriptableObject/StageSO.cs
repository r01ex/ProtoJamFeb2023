using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[CreateAssetMenu(fileName = "Stage Data", menuName = "ScriptableObjects/Stage", order = 1)]
public class StageSO : ScriptableObject
{
    [SerializeField] Sprite portrait;
    public Sprite Portrait { get => portrait; }

    [SerializeField] float timeLimit;
    public float TimeLimit { get => timeLimit; }

    [SerializeField] Sprite fragileImage;
    public Sprite FragileImage { get => fragileImage; }

    [SerializeField] Sprite brokenFragileImage;
    public Sprite BrokenFragileImage { get => brokenFragileImage; }

    [SerializeField] int needFragileCount;
    public int NeedFragiileCount { get => needFragileCount; }

    [SerializeField] int stageIndex;
    public int StageIndex { get => stageIndex; }

    [SerializeField] string sceneName;
    public string SceneName { get => sceneName; }
}
