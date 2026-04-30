using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Interactable
{
    [Header("Scene")]
    public string sceneToLoad;

    [Header("Dialogue")]
    public TextAsset dialogueBeforeLoad;

    [Header("Settings")]
    public float delayBeforeLoad = 1.2f;

    private bool isLoading = false;

    public override void Interact()
    {
        // Evita doppio click
        if (isLoading)
            return;

        // Se c'è un dialogo attivo NON fa nulla
        if (InkManager.Instance != null && InkManager.Instance.IsDialogueActive())
            return;

        isLoading = true;

        // Mostra dialogo se esiste
        if (dialogueBeforeLoad != null)
        {
            InkManager.Instance.StartStory(dialogueBeforeLoad);
        }

        // Carica scena dopo delay
        Invoke(nameof(LoadScene), delayBeforeLoad);
    }

    void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}