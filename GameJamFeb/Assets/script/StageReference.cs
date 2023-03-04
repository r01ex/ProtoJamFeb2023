using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StageReference : MonoBehaviour
{
    public static StageReference Instance { get; private set; }

    [SerializeField] StageSO[] _stageDatas;
    public StageSO[] StageDatas { get => _stageDatas; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject); 
        }
    }
}
