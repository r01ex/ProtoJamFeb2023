using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadingSceneScript : MonoBehaviour
{
    public static LoadingSceneScript Instance { get; private set; }
    public int scenetoLoad;
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        DontDestroyOnLoad(this.gameObject);
    }
    public void loadscene(int scenenumber)
    {
        SceneManager.LoadScene("LoadingScene");
        scenetoLoad = scenenumber;
        //���� �ε��� ��ũ��Ʈ���� loadingSceneScript.instance.scenetoload�� �ε�
    }
}
