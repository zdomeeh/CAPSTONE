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
        // Se l'introduzione e' gia' stata vista
        if (GameState.Instance != null && GameState.Instance.introPlayed)
        {
            // Assicura che lo schermo nero sia invisibile
            if (blackScreen != null)
                SetBlackAlpha(0f);

            // Mostra l'inventario
            if (inventoryPanel != null)
                inventoryPanel.SetActive(true);

            return;
        }

        // Avvia la sequenza introduttiva
        StartCoroutine(IntroSequence());
    }

    private IEnumerator IntroSequence()
    {
        // Segna che l'introduzione e' stata riprodotta
        if (GameState.Instance != null)
            GameState.Instance.introPlayed = true;

        // Blocca gli input del giocatore
        InkManager.IsInputBlocked = true;

        // Nasconde l'inventario
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        // Controlla che lo schermo nero sia assegnato
        if (blackScreen == null)
        {
            Debug.LogError("BlackScreen non assegnato nell'IntroManager.");
            yield break;
        }

        // Imposta lo schermo completamente nero
        SetBlackAlpha(1f);

        // Riproduce il suono di caduta
        if (fallAudio != null)
            fallAudio.Play();

        // Attende un breve tempo
        yield return new WaitForSeconds(blackHoldTime);

        // Dissolvenza da nero a trasparente
        yield return StartCoroutine(FadeBlack(0f));

        // Avvia il dialogo iniziale
        if (introInk != null)
        {
            InkManager.Instance.OnStoryEnd += ShowInventoryAfterIntro;
            InkManager.Instance.StartStory(introInk);
        }
    }

    private void ShowInventoryAfterIntro()
    {
        // Mostra l'inventario dopo il dialogo iniziale
        if (inventoryPanel != null)
            inventoryPanel.SetActive(true);
    }

    private IEnumerator FadeBlack(float targetAlpha)
    {
        // Se lo schermo non esiste, esce
        if (blackScreen == null)
            yield break;

        float startAlpha = blackScreen.color.a;
        float timer = 0f;

        // Cambia gradualmente la trasparenza
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

        // Imposta il valore finale
        SetBlackAlpha(targetAlpha);
    }

    private void SetBlackAlpha(float alpha)
    {
        // Se lo schermo non esiste, esce
        if (blackScreen == null)
            return;

        // Imposta direttamente la trasparenza
        Color color = blackScreen.color;
        color.a = alpha;
        blackScreen.color = color;
    }
}