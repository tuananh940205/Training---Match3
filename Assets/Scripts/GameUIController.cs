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
    [SerializeField] private GameController gameControllerObject;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OnSceneChange()
    {
        // SceneManager.LoadScene(targetSceneIndex);
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 0)
            SceneManager.LoadScene(1);
        else if (sceneIndex == 1)
            SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }
}