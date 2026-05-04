using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanelManager : MonoBehaviour
{
    public static OptionPanelManager Instance;

    [Header("UI")]
    public CanvasGroup optionsCanvasGroup;
    public GameObject inventoryPanel;

    [Header("Options")]
    public Slider volumeSlider;
    public Toggle fullscreenToggle;

    [Header("Settings")]
    public float fadeDuration = 0.25f;
    public bool allowEscapeKey = true;
    public bool pauseGameWhenOpen = true;

    private bool isOpen = false;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        // Imposta l'istanza
        Instance = this;
    }

    private void Start()
    {
        // Imposta il pannello chiuso all'avvio
        SetPanelInstant(false);

        // Carica il volume salvato
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        savedVolume = Mathf.Clamp01(savedVolume);

        // Carica il fullscreen salvato
        bool savedFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        // Imposta lo slider del volume
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.wholeNumbers = false;
            volumeSlider.value = savedVolume;
        }

        // Imposta il toggle fullscreen
        if (fullscreenToggle != null)
            fullscreenToggle.isOn = savedFullscreen;

        // Applica le impostazioni
        AudioListener.volume = savedVolume;
        Screen.fullScreen = savedFullscreen;

        // Messaggio di debug
        Debug.Log("Volume caricato: " + AudioListener.volume);
    }

    private void Update()
    {
        // Se il tasto ESC non e' permesso, esce
        if (!allowEscapeKey)
            return;

        // Controlla la pressione di ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Se un dialogo e' attivo, blocca apertura opzioni
            if (InkManager.Instance != null && InkManager.Instance.IsDialogueActive())
                return;

            // Apre o chiude il pannello opzioni
            ToggleOptions();
        }
    }

    public void ToggleOptions()
    {
        // Cambia stato del pannello
        if (isOpen)
            CloseOptions();
        else
            OpenOptions();
    }

    public void OpenOptions()
    {
        // Messaggio di debug
        Debug.Log("OPEN OPTIONS CHIAMATO");

        // Se e' gia' aperto, esce
        if (isOpen)
            return;

        // Segna come aperto
        isOpen = true;

        // Mette in pausa il gioco
        if (pauseGameWhenOpen)
            Time.timeScale = 0f;

        // Blocca gli input
        InkManager.IsInputBlocked = true;

        // Nasconde l'inventario
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        // Avvia la dissolvenza in apertura
        StartFade(1f, true);
    }

    public void CloseOptions()
    {
        // Se e' gia' chiuso, esce
        if (!isOpen)
            return;

        // Segna come chiuso
        isOpen = false;

        // Avvia la dissolvenza in chiusura
        StartFade(0f, false);

        // Ripristina il tempo
        if (pauseGameWhenOpen)
            Time.timeScale = 1f;

        // Sblocca gli input
        InkManager.IsInputBlocked = false;

        // Mostra l'inventario
        if (inventoryPanel != null)
            inventoryPanel.SetActive(true);
    }

    public void SetVolume(float value)
    {
        // Imposta il volume
        AudioListener.volume = value;

        // Salva il valore
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool value)
    {
        // Imposta il fullscreen
        Screen.fullScreen = value;

        // Salva il valore
        PlayerPrefs.SetInt("Fullscreen", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void StartFade(float targetAlpha, bool enableInteraction)
    {
        // Ferma eventuale fade attivo
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        // Avvia una nuova dissolvenza
        fadeCoroutine = StartCoroutine(FadeOptions(targetAlpha, enableInteraction));
    }

    private IEnumerator FadeOptions(float targetAlpha, bool enableInteractionAtEnd)
    {
        // Se il pannello non esiste, esce
        if (optionsCanvasGroup == null)
            yield break;

        // Se sta aprendo, abilita i raycast ma non ancora l'interazione
        if (targetAlpha > 0f)
        {
            optionsCanvasGroup.blocksRaycasts = true;
            optionsCanvasGroup.interactable = false;
        }

        float startAlpha = optionsCanvasGroup.alpha;
        float timer = 0f;

        // Cambia gradualmente la trasparenza
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / fadeDuration;

            optionsCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            yield return null;
        }

        // Imposta il valore finale
        optionsCanvasGroup.alpha = targetAlpha;
        optionsCanvasGroup.interactable = enableInteractionAtEnd;
        optionsCanvasGroup.blocksRaycasts = enableInteractionAtEnd;
    }

    private void SetPanelInstant(bool open)
    {
        // Imposta lo stato
        isOpen = open;

        // Se il pannello non esiste, esce
        if (optionsCanvasGroup == null)
            return;

        // Imposta subito visibilita' e interazione
        optionsCanvasGroup.alpha = open ? 1f : 0f;
        optionsCanvasGroup.interactable = open;
        optionsCanvasGroup.blocksRaycasts = open;
    }
}