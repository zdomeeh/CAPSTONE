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
    public string message = "Qualcuno sapeva. E Tò non avrebbe dovuto scoprirlo.";

    [Header("Typing")]
    public float typingSpeed = 0.04f;
    public float delayBetweenTexts = 0.8f;

    [Header("Scene")]
    public string mainMenuScene = "MainMenu";

    private void Start()
    {
        titleText.text = "";
        messageText.text = "";
        backToMenuButton.gameObject.SetActive(false);

        StartCoroutine(EndSequence());
    }

    private IEnumerator EndSequence()
    {
        yield return StartCoroutine(TypeText(titleText, title));

        yield return new WaitForSeconds(delayBetweenTexts);

        yield return StartCoroutine(TypeText(messageText, message));

        yield return new WaitForSeconds(0.5f);

        backToMenuButton.gameObject.SetActive(true);
    }

    private IEnumerator TypeText(TextMeshProUGUI targetText, string textToWrite)
    {
        targetText.text = "";

        foreach (char letter in textToWrite)
        {
            targetText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }
}