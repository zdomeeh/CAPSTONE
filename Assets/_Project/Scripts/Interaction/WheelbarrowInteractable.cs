using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WheelbarrowInteractable : Interactable
{
    [Header("Dialoghi")]
    public TextAsset firstDialogue;
    public TextAsset moveChoiceDialogue;

    [Header("Oggetti")]
    public GameObject crowbar;
    public string crowbarItemId = "outside_crowbar";

    [Header("Audio")]
    public AudioSource moveSound;

    [Header("Black Screen")]
    public string blackScreenObjectName = "BlackScreenPanel";
    public float fadeDuration = 0.4f;
    public float moveDuration = 3.5f;

    [Header("Flags")]
    public string enteredCabinFlag = "entered_cabin_once";
    public string movedFlag = "wheelbarrow_moved";

    private Image blackScreen;
    private bool isMoving = false;

    private void Start()
    {
        // Se la carriola e' gia' stata spostata in precedenza
        if (WorldState.Instance != null && WorldState.Instance.HasFlag(movedFlag))
        {
            // Mostra il piede di porco se non e' stato raccolto
            if (crowbar != null && !WorldState.Instance.IsItemCollected(crowbarItemId))
                crowbar.SetActive(true);

            // Nasconde la carriola
            gameObject.SetActive(false);
        }
    }

    public override void Interact()
    {
        // Se un dialogo e' attivo, blocca l'interazione
        if (InkManager.Instance.IsDialogueActive())
            return;

        // Se si sta gia' muovendo, blocca l'interazione
        if (isMoving)
            return;

        // Se il giocatore non e' ancora entrato nella capanna, mostra il primo dialogo
        if (!WorldState.Instance.HasFlag(enteredCabinFlag))
        {
            InkManager.Instance.StartStory(firstDialogue);
            return;
        }

        // Mostra il dialogo con la scelta di spostare la carriola
        InkManager.Instance.StartStory(moveChoiceDialogue);
    }

    public void MoveWheelbarrow()
    {
        // Evita avvii multipli
        if (isMoving)
            return;

        // Se e' gia' stata spostata, non fa nulla
        if (WorldState.Instance.HasFlag(movedFlag))
            return;

        // Cerca il pannello nero per la transizione
        FindBlackScreen();

        // Avvia la sequenza di movimento
        StartCoroutine(MoveWheelbarrowSequence());
    }

    private void FindBlackScreen()
    {
        // Se e' gia' stato trovato, esce
        if (blackScreen != null)
            return;

        Canvas canvas = FindObjectOfType<Canvas>();

        // Se non trova il Canvas, errore
        if (canvas == null)
        {
            Debug.LogError("Canvas persistente non trovato.");
            return;
        }

        Transform blackScreenTransform = canvas.transform.Find(blackScreenObjectName);

        // Se non trova il pannello, errore
        if (blackScreenTransform == null)
        {
            Debug.LogError("BlackScreenPanel non trovato nel Canvas persistente. Controlla il nome.");
            return;
        }

        blackScreen = blackScreenTransform.GetComponent<Image>();

        // Se manca il componente Image, errore
        if (blackScreen == null)
        {
            Debug.LogError("BlackScreenPanel trovato, ma manca il componente Image.");
        }
    }

    private IEnumerator MoveWheelbarrowSequence()
    {
        // Segna che la carriola si sta muovendo
        isMoving = true;

        // Blocca gli input
        InkManager.IsInputBlocked = true;

        // Schermo nero in entrata
        yield return StartCoroutine(FadeBlack(1f));

        // Riproduce il suono di movimento
        if (moveSound != null)
        {
            moveSound.Stop();
            moveSound.Play();
        }

        // Attende la durata del movimento
        yield return new WaitForSeconds(moveDuration);

        // Salva che la carriola e' stata spostata
        WorldState.Instance.SetFlag(movedFlag);

        // Mostra il piede di porco
        if (crowbar != null)
            crowbar.SetActive(true);

        // Schermo nero in uscita
        yield return StartCoroutine(FadeBlack(0f));

        // Sblocca gli input
        InkManager.IsInputBlocked = false;

        // Riprende il dialogo messo in pausa
        InkManager.Instance.ResumeAfterExternalAction();

        // Nasconde la carriola
        gameObject.SetActive(false);
    }

    private IEnumerator FadeBlack(float targetAlpha)
    {
        // Se il pannello nero non esiste, annulla
        if (blackScreen == null)
        {
            Debug.LogError("BlackScreen e' null. Fade annullato.");
            yield break;
        }

        float startAlpha = blackScreen.color.a;
        float timer = 0f;

        // Cambia gradualmente la trasparenza
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

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