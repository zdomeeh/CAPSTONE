using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Chest : Interactable
{
    [Header("Item richiesto")]
    public string requiredItem = "Crowbar";

    [Header("Dialoghi")]
    public TextAsset closedDialogue;
    public TextAsset hintDialogue;
    public TextAsset openDialogue;

    [Header("Oggetto nascosto")]
    public GameObject hiddenObject;

    [Header("Background swap")]
    public GameObject backgroundClosed;
    public GameObject backgroundOpen;

    [Header("Audio")]
    public AudioSource openSound;

    [Header("Black Screen")]
    public string blackScreenObjectName = "BlackScreenPanel";
    public float fadeDuration = 0.4f;
    public float openDuration = 2f;

    private bool isOpened = false;
    private bool isOpening = false;
    private Image blackScreen;

    public override void Interact()
    {
        // Se un dialogo e' gia' attivo, blocca l'interazione
        if (InkManager.Instance.IsDialogueActive())
            return;

        // Se la cassa e' gia' aperta o si sta aprendo, blocca l'interazione
        if (isOpened || isOpening)
            return;

        bool hasItem = Inventory.Instance.HasItem(requiredItem);
        string selected = InventorySelection.Instance.selectedItem;

        // Se il giocatore non ha l'oggetto richiesto, mostra il dialogo della cassa chiusa
        if (!hasItem)
        {
            InkManager.Instance.StartStory(closedDialogue);
            return;
        }

        // Se il giocatore ha l'oggetto ma non lo ha selezionato, mostra un suggerimento
        if (selected != requiredItem)
        {
            InkManager.Instance.StartStory(hintDialogue);
            return;
        }

        // Rimuove la selezione dell'oggetto
        InventorySelection.Instance.ClearSelection();

        // Cerca il pannello nero per la transizione
        FindBlackScreen();

        // Avvia la sequenza di apertura della cassa
        StartCoroutine(OpenChestSequence());
    }

    private IEnumerator OpenChestSequence()
    {
        // Indica che la cassa si sta aprendo
        isOpening = true;

        // Blocca gli input del giocatore
        InkManager.IsInputBlocked = true;

        // Fa comparire lo schermo nero
        yield return StartCoroutine(FadeBlack(1f));

        // Riproduce il suono di apertura
        if (openSound != null)
        {
            openSound.Stop();
            openSound.Play();
        }

        // Aspetta il tempo dell'apertura
        yield return new WaitForSecondsRealtime(openDuration);

        // Nasconde il background della cassa chiusa
        if (backgroundClosed != null)
            backgroundClosed.SetActive(false);

        // Mostra il background della cassa aperta
        if (backgroundOpen != null)
            backgroundOpen.SetActive(true);

        // Mostra l'oggetto nascosto
        if (hiddenObject != null)
            hiddenObject.SetActive(true);

        // Segna la cassa come aperta
        isOpened = true;

        // Fa scomparire lo schermo nero
        yield return StartCoroutine(FadeBlack(0f));

        // Sblocca gli input del giocatore
        InkManager.IsInputBlocked = false;

        // Indica che la cassa non si sta piu' aprendo
        isOpening = false;

        // Se esiste, avvia il dialogo dopo l'apertura
        if (openDialogue != null)
            InkManager.Instance.StartStory(openDialogue);

        // Messaggio di debug
        Debug.Log("Cassa aperta con transizione prima del dialogo.");
    }

    private void FindBlackScreen()
    {
        // Se il pannello nero e' gia' stato trovato, esce dal metodo
        if (blackScreen != null)
            return;

        Canvas canvas = FindObjectOfType<Canvas>();

        // Se non trova il Canvas, mostra un errore
        if (canvas == null)
        {
            Debug.LogError("Canvas persistente non trovato.");
            return;
        }

        Transform blackScreenTransform = canvas.transform.Find(blackScreenObjectName);

        // Se non trova il pannello nero, mostra un errore
        if (blackScreenTransform == null)
        {
            Debug.LogError("BlackScreenPanel non trovato nel Canvas persistente.");
            return;
        }

        blackScreen = blackScreenTransform.GetComponent<Image>();

        // Se il pannello non ha il componente Image, mostra un errore
        if (blackScreen == null)
        {
            Debug.LogError("BlackScreenPanel trovato, ma manca il componente Image.");
        }
    }

    private IEnumerator FadeBlack(float targetAlpha)
    {
        // Se il pannello nero non esiste, annulla la dissolvenza
        if (blackScreen == null)
        {
            Debug.LogError("BlackScreen e' null. Fade annullato.");
            yield break;
        }

        float startAlpha = blackScreen.color.a;
        float timer = 0f;

        // Cambia gradualmente la trasparenza del pannello nero
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / fadeDuration;

            Color color = blackScreen.color;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            blackScreen.color = color;

            yield return null;
        }

        // Imposta la trasparenza finale precisa
        Color finalColor = blackScreen.color;
        finalColor.a = targetAlpha;
        blackScreen.color = finalColor;
    }
}