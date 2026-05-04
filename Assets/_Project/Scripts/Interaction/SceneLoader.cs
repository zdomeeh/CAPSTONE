using UnityEngine;

public class SceneLoader : Interactable
{
    [Header("Scene")]
    public string sceneToLoad;

    [Header("Dialogue")]
    public TextAsset dialogueBeforeLoad;

    [Header("Transition")]
    public AudioSource transitionAudio;
    public float blackHoldDuration = 1.5f;

    [Header("World State")]
    public bool setWorldFlagOnLoad = false;
    public string worldFlagToSet;

    private bool isLoading = false;

    public override void Interact()
    {
        // Se sta gia' caricando una scena, blocca l'interazione
        if (isLoading)
            return;

        // Se un dialogo e' attivo, blocca l'interazione
        if (InkManager.Instance != null && InkManager.Instance.IsDialogueActive())
            return;

        // Segna che sta iniziando il caricamento
        isLoading = true;

        // Se esiste un dialogo prima del cambio scena
        if (dialogueBeforeLoad != null)
        {
            // Aspetta la fine del dialogo prima di cambiare scena
            InkManager.Instance.OnStoryEnd += StartSceneTransition;
            InkManager.Instance.StartStory(dialogueBeforeLoad);
        }
        else
        {
            // Se non c'e' dialogo, cambia scena subito
            StartSceneTransition();
        }
    }

    private void StartSceneTransition()
    {
        // Se richiesto, imposta una flag nel mondo di gioco
        if (setWorldFlagOnLoad && WorldState.Instance != null)
        {
            WorldState.Instance.SetFlag(worldFlagToSet);
        }

        // Carica la scena con transizione
        SceneTransitionManager.Instance.LoadSceneWithTransition(
            sceneToLoad,
            transitionAudio,
            blackHoldDuration
        );
    }
}