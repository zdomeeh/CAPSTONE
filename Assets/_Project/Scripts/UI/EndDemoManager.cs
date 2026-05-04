using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndDemoManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI messageText;
    public Button backToMenuButton;

    [Header("Text")]
    public string title = "FINE DEMO";
    public string message = "QUALCUNO SAPEVA. E TO' NON AVREBBE DOVUTO SCOPRIRLO.";

    [Header("Typing")]
    public float typingSpeed = 0.04f;
    public float delayBetweenTexts = 0.8f;

    [Header("Scene")]
    public string mainMenuScene = "MainMenu";

    private void Start()
    {
        // Svuota i testi iniziali
        titleText.text = "";
        messageText.text = "";

        // Nasconde il bottone per tornare al menu
        backToMenuButton.gameObject.SetActive(false);

        // Avvia la sequenza finale
        StartCoroutine(EndSequence());
    }

    private IEnumerator EndSequence()
    {
        // Scrive il titolo gradualmente
        yield return StartCoroutine(TypeText(titleText, title));

        // Aspetta prima di scrivere il messaggio
        yield return new WaitForSeconds(delayBetweenTexts);

        // Scrive il messaggio gradualmente
        yield return StartCoroutine(TypeText(messageText, message));

        // Aspetta prima di mostrare il bottone
        yield return new WaitForSeconds(0.5f);

        // Mostra il bottone per tornare al menu
        backToMenuButton.gameObject.SetActive(true);
    }

    private IEnumerator TypeText(TextMeshProUGUI targetText, string textToWrite)
    {
        // Svuota il testo prima di scrivere
        targetText.text = "";

        // Scrive una lettera alla volta
        foreach (char letter in textToWrite)
        {
            targetText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void BackToMenu()
    {
        // Resetta il gioco se il manager esiste
        if (GameResetManager.Instance != null)
            GameResetManager.Instance.ResetGame();

        // Ripristina la velocita' normale del tempo
        Time.timeScale = 1f;

        // Carica la scena del menu principale
        SceneManager.LoadScene(mainMenuScene);
    }
}