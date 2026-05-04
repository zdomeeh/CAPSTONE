using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Interactable
{
    [Header("Scene")]
    public string sceneToLoad;

    [Header("Dialogue")]
    public TextAsset dialogueBeforeLoad;

    [Header("Timing")]
    public float delayBeforeLoad = 1.2f;

    [Header("World State")]
    public bool setWorldFlagOnLoad = false;
    public string worldFlagToSet;

    private bool isLoading = false;

    public override void Interact()
    {
        if (isLoading)
            return;

        if (InkManager.Instance != null && InkManager.Instance.IsDialogueActive())
            return;

        isLoading = true;

        if (dialogueBeforeLoad != null)
        {
            InkManager.Instance.StartStory(dialogueBeforeLoad);
        }

        Invoke(nameof(LoadScene), delayBeforeLoad);
    }

    private void LoadScene()
    {
        if (setWorldFlagOnLoad && WorldState.Instance != null)
        {
            WorldState.Instance.SetFlag(worldFlagToSet);
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}