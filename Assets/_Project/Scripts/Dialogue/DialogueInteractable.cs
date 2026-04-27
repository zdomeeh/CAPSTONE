using UnityEngine;

public class DialogueInteractable : Interactable
{
    public TextAsset inkJSON;

    public override void Interact()
    {
        if (inkJSON != null)
        {
            InkManager.Instance.StartStory(inkJSON);
        }
    }
}