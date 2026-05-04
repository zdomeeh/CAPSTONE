using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;

    [Header("Scenes")]
    public string firstGameScene = "Bedroom";

    private void Start()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);

        Time.timeScale = 1f;
    }

    public void PlayGame()
    {
        if (GameResetManager.Instance != null)
            GameResetManager.Instance.ResetGame();

        SceneManager.LoadScene(firstGameScene);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }
}