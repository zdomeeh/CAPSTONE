using UnityEngine;

public class BedKeyInteractable : Interactable
{
    public TextAsset BedKey;              // dialogo principale (con scelta)
    public TextAsset alreadyChecked;   
    public GameObject hiddenKey;

    private bool keyRevealed = false;

    public override void Interact()
    {
        if (InkManager.Instance.IsDialogueActive())
            return;

        //  SE GIÀ TROVATA LA CHIAVE
        if (keyRevealed)
        {
            InkManager.Instance.StartStory(alreadyChecked);
            return;
        }

        //  dialogo normale
        InkManager.Instance.StartStory(BedKey);
    }

    public void RevealKey()
    {
        if (!keyRevealed)
        {
            hiddenKey.SetActive(true);
            keyRevealed = true;

            Debug.Log("Chiave rivelata sotto il letto");
        }
    }
}