using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonManager : MonoBehaviour
{

    public GameObject settingsMenu;
    public void PlayGame()
    {
        SceneManager.LoadScene("1_IntroCutscene");
        Time.timeScale = 1;
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }
    public void Exit()
    {
        Debug.Log("Game exited");
        Application.Quit();
    }


    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartScreen()
    {
        SceneManager.LoadScene("0_StartScreen");
        Time.timeScale = 1;
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }
}
