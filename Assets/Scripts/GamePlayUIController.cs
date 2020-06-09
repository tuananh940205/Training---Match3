using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayUIController : MonoBehaviour
{
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }
}
