using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class DataHandler : MonoBehaviour
{
    string dir;
    string filename;
    public static DataHandler Instance { get; private set; }
    private GameData gameData;
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        DontDestroyOnLoad(this.gameObject);
        Newdata();
        dir = Application.persistentDataPath;
        filename = "testData.json";
    }

    public void Save()
    {
        string fullpath = Path.Combine(dir, filename);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullpath));
            string dataToStore = JsonUtility.ToJson(gameData,true);
            using(FileStream stream = new FileStream(fullpath,FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("error saving " + e);
        }
    }
    public void Load()
    {
        string fullpath = Path.Combine(dir, filename);
        if(File.Exists(fullpath))
        {
            try
            {
                string dataToLoad;
                using (FileStream stream = new FileStream(fullpath,FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                Debug.Log("loaded data : " + dataToLoad);
                gameData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.Log("error loading " + e);
            }
        }
        if (this.gameData==null)
        {
            Debug.Log("making new data");
            Newdata();
        }
    }
    public void Newdata()
    {
        this.gameData = new GameData();
    }
    public void changeStageData(int stagenum, bool iscleared, float cleartime, int score)
    {
        if (iscleared == true)
        {
            if (gameData.isCleared[stagenum] == true)
            {
                if(cleartime<gameData.clearTime[stagenum])
                {
                    gameData.clearTime[stagenum] = cleartime;
                }
                if(score>gameData.userScore[stagenum])
                {
                    gameData.userScore[stagenum] = score;
                }
            }
            else
            {
                gameData.isCleared[stagenum] = true;
                gameData.clearTime[stagenum] = cleartime;
                gameData.userScore[stagenum] = score;
            }
        }
       
    }


}
