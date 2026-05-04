using UnityEngine;

public class Door : Interactable
{
    [Header("Dialoghi")]
    public TextAsset noLetterDialogue;
    public TextAsset lockedDialogue;
    public TextAsset openDialogue;

    [Header("Item richiesto")]
    public string requiredItem = "Key";

    [Header("Scena")]
    public string sceneToLoad = "Lvl0.5";

    [Header("Transizione")]
    public AudioSource transitionAudio;
    public float blackHoldDuration = 3.5f;

    private bool isLoading = false;

    public override void Interact()
    {
        // Se la scena si sta gia' caricando, blocca l'interazione
        if (isLoading)
            return;

        // Se un dialogo e' gia' attivo, blocca l'interazione
        if (InkManager.Instance != null && InkManager.Instance.IsDialogueActive())
            return;

        // Se il giocatore non ha ancora letto la lettera, mostra il dialogo dedicato
        if (!GameState.Instance.hasReadLetter)
        {
            InkManager.Instance.StartStory(noLetterDialogue);
            return;
        }

        string selected = InventorySelection.Instance.selectedItem;

        // Se l'oggetto selezionato non e' quello richiesto, mostra il dialogo della porta chiusa
        if (selected != requiredItem)
        {
            InkManager.Instance.StartStory(lockedDialogue);
            return;
        }

        // Rimuove la selezione dell'oggetto
        InventorySelection.Instance.ClearSelection();

        // Segna che sta iniziando il caricamento
        isLoading = true;

        // Quando il dialogo finisce, avvia la transizione della porta
        InkManager.Instance.OnStoryEnd += StartDoorTransition;

        // Avvia il dialogo della porta aperta
        InkManager.Instance.StartStory(openDialogue);
    }

    private void StartDoorTransition()
    {
        // Carica la nuova scena con transizione
        SceneTransitionManager.Instance.LoadSceneWithTransition(
            sceneToLoad,
            transitionAudio,
            blackHoldDuration
        );
    }
}