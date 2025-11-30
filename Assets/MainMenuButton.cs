using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu";

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // กลับเวลาปกติ
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
