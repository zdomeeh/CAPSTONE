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
        Instance = this;
    }

    private void Start()
    {
        SetPanelInstant(false);

        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        savedVolume = Mathf.Clamp01(savedVolume);

        bool savedFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.wholeNumbers = false;
            volumeSlider.value = savedVolume;
        }

        if (fullscreenToggle != null)
            fullscreenToggle.isOn = savedFullscreen;

        AudioListener.volume = savedVolume;
        Screen.fullScreen = savedFullscreen;

        Debug.Log("Volume caricato: " + AudioListener.volume);
    }

    private void Update()
    {
        if (!allowEscapeKey)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (InkManager.Instance != null && InkManager.Instance.IsDialogueActive())
                return;

            ToggleOptions();
        }
    }

    public void ToggleOptions()
    {
        if (isOpen)
            CloseOptions();
        else
            OpenOptions();
    }

    public void OpenOptions()
    {
        Debug.Log("OPEN OPTIONS CHIAMATO");

        if (isOpen)
            return;

        isOpen = true;

        if (pauseGameWhenOpen)
            Time.timeScale = 0f;

        InkManager.IsInputBlocked = true;

        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        StartFade(1f, true);
    }

    public void CloseOptions()
    {
        if (!isOpen)
            return;

        isOpen = false;

        StartFade(0f, false);

        if (pauseGameWhenOpen)
            Time.timeScale = 1f;

        InkManager.IsInputBlocked = false;

        if (inventoryPanel != null)
            inventoryPanel.SetActive(true);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool value)
    {
        Screen.fullScreen = value;
        PlayerPrefs.SetInt("Fullscreen", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void StartFade(float targetAlpha, bool enableInteraction)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOptions(targetAlpha, enableInteraction));
    }

    private IEnumerator FadeOptions(float targetAlpha, bool enableInteractionAtEnd)
    {
        if (optionsCanvasGroup == null)
            yield break;

        if (targetAlpha > 0f)
        {
            optionsCanvasGroup.blocksRaycasts = true;
            optionsCanvasGroup.interactable = false;
        }

        float startAlpha = optionsCanvasGroup.alpha;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / fadeDuration;

            optionsCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            yield return null;
        }

        optionsCanvasGroup.alpha = targetAlpha;
        optionsCanvasGroup.interactable = enableInteractionAtEnd;
        optionsCanvasGroup.blocksRaycasts = enableInteractionAtEnd;
    }

    private void SetPanelInstant(bool open)
    {
        isOpen = open;

        if (optionsCanvasGroup == null)
            return;

        optionsCanvasGroup.alpha = open ? 1f : 0f;
        optionsCanvasGroup.interactable = open;
        optionsCanvasGroup.blocksRaycasts = open;
    }
}