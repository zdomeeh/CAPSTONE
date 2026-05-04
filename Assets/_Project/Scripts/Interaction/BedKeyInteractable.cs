using UnityEngine;

public class BedKeyInteractable : Interactable
{
    public TextAsset BedKey;
    public TextAsset alreadyChecked;
    public GameObject hiddenKey;

    public string bedCheckedFlag = "bedroom_bed_checked";

    public override void Interact()
    {
        if (InkManager.Instance.IsDialogueActive())
            return;

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