using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Interactable
{
    public TextAsset noLetterDialogue;
    public TextAsset lockedDialogue;
    public TextAsset openDialogue;

    public string requiredItem = "Key";

    public override void Interact()
    {
        Debug.Log("HasReadLetter: " + GameState.Instance.hasReadLetter);

        // 1. lettera non letta
        if (!GameState.Instance.hasReadLetter)
        {
            InkManager.Instance.StartStory(noLetterDialogue);
            return;
        }

        // 2. Controlla item selezionato
        string selected = InventorySelection.Instance.selectedItem;

        if (selected != requiredItem)
        {
            InkManager.Instance.StartStory(lockedDialogue);
            return;
        }

        // 3. Porta si apre
        InkManager.Instance.StartStory(openDialogue);

        InventorySelection.Instance.ClearSelection();

        Invoke(nameof(OpenDoor), 1.5f);
    }

    void OpenDoor()
    {
        SceneManager.LoadScene("Lvl1");
    }
}
