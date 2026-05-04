using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [Header("UI")]
    public Image blackScreen;
    public GameObject inventoryPanel;

    [Header("Settings")]
    public float defaultFadeDuration = 0.4f;

    private bool isTransitioning = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadSceneWithTransition(string sceneName, AudioSource audio = null, float holdDuration = 1.5f)
    {
        if (isTransitioning)
            return;

        StartCoroutine(Transition(sceneName, audio, holdDuration));
    }

    private IEnumerator Transition(string sceneName, AudioSource audio, float holdDuration)
    {
        isTransitioning = true;
        InkManager.IsInputBlocked = true;

        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        // Fade verso nero
        yield return StartCoroutine(FadeBlack(1f));

        if (audio != null)
        {
            audio.Stop();
            audio.Play();
        }

        yield return new WaitForSecondsRealtime(holdDuration);

        SceneManager.LoadScene(sceneName);

        // aspetta 1 frame per far caricare la scena
        yield return null;

        // Fade dal nero alla scena
        yield return StartCoroutine(FadeBlack(0f));

        if (inventoryPanel != null)
            inventoryPanel.SetActive(true);

        InkManager.IsInputBlocked = false;
        isTransitioning = false;
    }

    private IEnumerator FadeBlack(float targetAlpha)
    {
        if (blackScreen == null)
        {
            Debug.LogError("BlackScreen non assegnato nel SceneTransitionManager.");
            yield break;
        }

        float startAlpha = blackScreen.color.a;
        float timer = 0f;

        while (timer < defaultFadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / defaultFadeDuration;

            Color color = blackScreen.color;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            blackScreen.color = color;

            yield return null;
        }

        Color finalColor = blackScreen.color;
        finalColor.a = targetAlpha;
        blackScreen.color = finalColor;
    }
}