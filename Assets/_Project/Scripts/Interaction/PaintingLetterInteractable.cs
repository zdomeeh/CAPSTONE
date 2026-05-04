using UnityEngine;

public class PaintingLetterInteractable : Interactable
{
    public TextAsset paintingInk;
    public TextAsset alreadyCheckedInk;
    public GameObject hiddenLetter;

    public string paintingCheckedFlag = "mainroom_painting_checked";

    public override void Interact()
    {
        if (InkManager.Instance.IsDialogueActive())
            return;

        if (WorldState.Instance.HasFlag(paintingCheckedFlag))
        {
            InkManager.Instance.StartStory(alreadyCheckedInk);
            return;
        }

        InkManager.Instance.StartStory(paintingInk);
    }

    public void RevealLetter()
    {
        WorldState.Instance.SetFlag(paintingCheckedFlag);

        if (hiddenLetter != null)
            hiddenLetter.SetActive(true);

        Debug.Log("Lettera rivelata dietro il quadro");
    }
}