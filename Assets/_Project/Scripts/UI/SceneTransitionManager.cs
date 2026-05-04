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
        // Evita piu' istanze
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Imposta questa come istanza principale
        Instance = this;

        // Mantiene questo oggetto tra le scene
        DontDestroyOnLoad(gameObject);
    }

    public void LoadSceneWithTransition(string sceneName, AudioSource audio = null, float holdDuration = 1.5f)
    {
        // Se una transizione e' gia' in corso, blocca
        if (isTransitioning)
            return;

        // Avvia la sequenza di transizione
        StartCoroutine(Transition(sceneName, audio, holdDuration));
    }

    private IEnumerator Transition(string sceneName, AudioSource audio, float holdDuration)
    {
        // Segna che la transizione e' in corso
        isTransitioning = true;

        // Blocca input
        InkManager.IsInputBlocked = true;

        // Nasconde l'inventario
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        // Fade verso nero
        yield return StartCoroutine(FadeBlack(1f));

        // Riproduce audio se presente
        if (audio != null)
        {
            audio.Stop();
            audio.Play();
        }

        // Attende prima di cambiare scena
        yield return new WaitForSecondsRealtime(holdDuration);

        // Carica la nuova scena
        SceneManager.LoadScene(sceneName);

        // Attende un frame per sicurezza
        yield return null;

        // Fade dal nero alla scena
        yield return StartCoroutine(FadeBlack(0f));

        // Mostra di nuovo l'inventario
        if (inventoryPanel != null)
            inventoryPanel.SetActive(true);

        // Sblocca input
        InkManager.IsInputBlocked = false;

        // Segna fine transizione
        isTransitioning = false;
    }

    private IEnumerator FadeBlack(float targetAlpha)
    {
        // Se manca lo schermo nero, errore
        if (blackScreen == null)
        {
            Debug.LogError("BlackScreen non assegnato nel SceneTransitionManager.");
            yield break;
        }

        float startAlpha = blackScreen.color.a;
        float timer = 0f;

        // Cambia gradualmente la trasparenza
        while (timer < defaultFadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / defaultFadeDuration;

            Color color = blackScreen.color;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            blackScreen.color = color;

            yield return null;
        }

        // Imposta il valore finale
        Color finalColor = blackScreen.color;
        finalColor.a = targetAlpha;
        blackScreen.color = finalColor;
    }
}