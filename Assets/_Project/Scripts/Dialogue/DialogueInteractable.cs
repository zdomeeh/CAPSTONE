using UnityEngine;

public class DialogueInteractable : Interactable
{
    public TextAsset inkJSON;

    public override void Interact()
    {
        // Controlla se il file esiste
        if (inkJSON != null)
        {
            // Avvia il dialogo tramite InkManager
            InkManager.Instance.StartStory(inkJSON);
        }
    }
}