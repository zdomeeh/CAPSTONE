using UnityEngine;

public class PaintingLetterInteractable : Interactable
{
    public TextAsset paintingInk;
    public TextAsset alreadyCheckedInk;
    public GameObject hiddenLetter;

    public string paintingCheckedFlag = "mainroom_painting_checked";

    public override void Interact()
    {
        // Se un dialogo e' gia' attivo, blocca l'interazione
        if (InkManager.Instance.IsDialogueActive())
            return;

        // Se il quadro e' gia' stato controllato, mostra un dialogo diverso
        if (WorldState.Instance.HasFlag(paintingCheckedFlag))
        {
            InkManager.Instance.StartStory(alreadyCheckedInk);
            return;
        }

        // Avvia il dialogo principale del quadro
        InkManager.Instance.StartStory(paintingInk);
    }

    public void RevealLetter()
    {
        // Segna che il quadro e' stato controllato
        WorldState.Instance.SetFlag(paintingCheckedFlag);

        // Mostra la lettera nascosta
        if (hiddenLetter != null)
            hiddenLetter.SetActive(true);

        // Messaggio di debug
        Debug.Log("Lettera rivelata dietro il quadro");
    }
}