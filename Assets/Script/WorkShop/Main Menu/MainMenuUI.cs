using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Fianl Pee 3")]
    [SerializeField] private string gameSceneName = "Fianl Pee 3";

    public void OnClickStartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(1);
    }

    public void OnClickQuit()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
