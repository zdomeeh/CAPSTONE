using UnityEngine;

public class BedKeyInteractable : Interactable
{
    [Header("Dialoghi")]
    public TextAsset beforeLetterInk;
    public TextAsset BedKey;
    public TextAsset alreadyChecked;

    [Header("Oggetti")]
    public GameObject hiddenKey;

    [Header("Flags")]
    public string bedCheckedFlag = "bedroom_bed_checked";

    public override void Interact()
    {
        if (InkManager.Instance.IsDialogueActive())
            return;

        if (!GameState.Instance.hasReadLetter)
        {
            InkManager.Instance.StartStory(beforeLetterInk);
            return;
        }

        if (WorldState.Instance.HasFlag(bedCheckedFlag))
        {
            InkManager.Instance.StartStory(alreadyChecked);
            return;
        }

        InkManager.Instance.StartStory(BedKey);
    }

    public void RevealKey()
    {
        WorldState.Instance.SetFlag(bedCheckedFlag);

        if (hiddenKey != null)
            hiddenKey.SetActive(true);

        Debug.Log("Chiave rivelata sotto il letto");
    }
}