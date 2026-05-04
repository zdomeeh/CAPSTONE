using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject inventoryPanel;

    [Header("Dialogue")]
    public TextAsset introInk;

    [Header("Audio")]
    public AudioSource fallAudio;

    [Header("Black Screen")]
    public Image blackScreen;
    public float blackHoldTime = 0.6f;
    public float fadeDuration = 1f;

    private void Start()
    {
        if (GameState.Instance != null && GameState.Instance.introPlayed)
        {
            if (blackScreen != null)
                SetBlackAlpha(0f);

            if (inventoryPanel != null)
                inventoryPanel.SetActive(true);

            return;
        }

        StartCoroutine(IntroSequence());
    }

    private IEnumerator IntroSequence()
    {
        if (GameState.Instance != null)
            GameState.Instance.introPlayed = true;

        InkManager.IsInputBlocked = true;

        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        if (blackScreen == null)
        {
            Debug.LogError("BlackScreen non assegnato nell'IntroManager.");
            yield break;
        }

        SetBlackAlpha(1f);

        if (fallAudio != null)
            fallAudio.Play();

        yield return new WaitForSeconds(blackHoldTime);

        yield return StartCoroutine(FadeBlack(0f));

        if (introInk != null)
        {
            InkManager.Instance.OnStoryEnd += ShowInventoryAfterIntro;
            InkManager.Instance.StartStory(introInk);
        }
    }

    private void ShowInventoryAfterIntro()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(true);
    }

    private IEnumerator FadeBlack(float targetAlpha)
    {
        if (blackScreen == null)
            yield break;

        float startAlpha = blackScreen.color.a;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            if (blackScreen == null)
                yield break;

            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            Color color = blackScreen.color;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            blackScreen.color = color;

            yield return null;
        }

        SetBlackAlpha(targetAlpha);
    }

    private void SetBlackAlpha(float alpha)
    {
        if (blackScreen == null)
            return;

        Color color = blackScreen.color;
        color.a = alpha;
        blackScreen.color = color;
    }
}