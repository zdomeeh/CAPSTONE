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
        // Mostra il pannello del menu principale
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);

        // Assicura che il tempo sia normale
        Time.timeScale = 1f;
    }

    public void PlayGame()
    {
        // Resetta lo stato del gioco prima di iniziare
        if (GameResetManager.Instance != null)
            GameResetManager.Instance.ResetGame();

        // Carica la prima scena di gioco
        SceneManager.LoadScene(firstGameScene);
    }

    public void ExitGame()
    {
        // Chiude l'applicazione
        Application.Quit();

        // Messaggio di debug (utile solo in editor)
        Debug.Log("Exit Game");
    }
}