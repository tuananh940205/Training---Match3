using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameUIController : MonoBehaviour
{
    private static GameUIController instance;
    private int sceneIndex;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = GetComponent<GameUIController>();
        DontDestroyOnLoad(this.gameObject);
    }

    public void OnSceneChange()
    {
        // SceneManager.LoadScene(targetSceneIndex);
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 0)
        {
            SceneManager.LoadScene(1);
            
        }
            
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}