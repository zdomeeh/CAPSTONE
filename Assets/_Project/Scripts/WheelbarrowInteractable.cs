using UnityEngine;
using System.Collections;

public class WheelbarrowInteractable : Interactable
{
    [Header("Dialoghi")]
    public TextAsset firstDialogue;
    public TextAsset moveChoiceDialogue;

    [Header("Oggetti")]
    public GameObject crowbar;
    public string crowbarItemId = "crowbar";

    [Header("Audio")]
    public AudioSource moveSound;
    public float revealDelay = 1f;

    [Header("Flags")]
    public string enteredCabinFlag = "entered_cabin_once";
    public string movedFlag = "wheelbarrow_moved";

    private void Start()
    {
        if (WorldState.Instance != null && WorldState.Instance.HasFlag(movedFlag))
        {
            gameObject.SetActive(false);

            if (crowbar != null && !WorldState.Instance.IsItemCollected(crowbarItemId))
                crowbar.SetActive(true);
        }
    }

    public override void Interact()
    {
        if (InkManager.Instance.IsDialogueActive())
            return;

        if (!WorldState.Instance.HasFlag(enteredCabinFlag))
        {
            InkManager.Instance.StartStory(firstDialogue);
            return;
        }

        InkManager.Instance.StartStory(moveChoiceDialogue);
    }

    public void MoveWheelbarrow()
    {
        if (WorldState.Instance.HasFlag(movedFlag))
            return;

        StartCoroutine(MoveWheelbarrowSequence());
    }

    private IEnumerator MoveWheelbarrowSequence()
    {
        WorldState.Instance.SetFlag(movedFlag);

        if (moveSound != null)
            moveSound.Play();

        yield return new WaitForSeconds(revealDelay);

        if (crowbar != null)
            crowbar.SetActive(true);

        gameObject.SetActive(false);

        Debug.Log("Carriola spostata, piede di porco rivelato");
    }
}