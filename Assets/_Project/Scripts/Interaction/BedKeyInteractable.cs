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
        // Se un dialogo e' gia' attivo, non permette una nuova interazione
        if (InkManager.Instance.IsDialogueActive())
            return;

        // Se il giocatore non ha ancora letto la lettera, mostra il dialogo iniziale
        if (!GameState.Instance.hasReadLetter)
        {
            InkManager.Instance.StartStory(beforeLetterInk);
            return;
        }

        // Se il letto e' gia' stato controllato, mostra un dialogo diverso
        if (WorldState.Instance.HasFlag(bedCheckedFlag))
        {
            InkManager.Instance.StartStory(alreadyChecked);
            return;
        }

        // Altrimenti avvia il dialogo in cui si trova la chiave
        InkManager.Instance.StartStory(BedKey);
    }

    public void RevealKey()
    {
        // Segna che il letto e' stato controllato
        WorldState.Instance.SetFlag(bedCheckedFlag);

        // Mostra la chiave nascosta
        if (hiddenKey != null)
            hiddenKey.SetActive(true);

        // Messaggio di debug
        Debug.Log("Chiave rivelata sotto il letto");
    }
}