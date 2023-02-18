using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool[] isCleared;
    public float[] clearTime;
    public int[] userScore;
    public int[] maxScore;
    public GameData()
    {
        int numberOfStages = 6;
        isCleared = new bool[numberOfStages];
        clearTime = new float[numberOfStages];
        userScore = new int[numberOfStages];
        maxScore = new int[numberOfStages]; 
    }
}
