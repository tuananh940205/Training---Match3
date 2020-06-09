using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartUIController : MonoBehaviour
{
    public void OnGamePlaySceneEnter()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
