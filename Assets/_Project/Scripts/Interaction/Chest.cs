using UnityEngine;

public class Chest : Interactable
{
    [Header("Item richiesto")]
    public string requiredItem = "Crowbar";

    [Header("Dialoghi")]
    public TextAsset closedDialogue;
    public TextAsset hintDialogue;
    public TextAsset openDialogue;

    [Header("Oggetto nascosto")]
    public GameObject hiddenObject;

    [Header("Background swap")]
    public GameObject backgroundClosed;
    public GameObject backgroundOpen;

    [Header("Timing")]
    public float openDelay = 0.5f;

    private bool isOpened = false;

    public override void Interact()
    {
        // Se già aperta
        if (isOpened)
        {
            Debug.Log("Cassa già aperta");
            return;
        }

        bool hasItem = Inventory.Instance.HasItem(requiredItem);
        string selected = InventorySelection.Instance.selectedItem;

        // 1. non si ha il piede di porco
        if (!hasItem)
        {
            InkManager.Instance.StartStory(closedDialogue);
            return;
        }

        // 2. si ha il piede di porco ma non selezionato
        if (selected != requiredItem)
        {
            InkManager.Instance.StartStory(hintDialogue);
            return;
        }

        // 3. apertura cassa
        InkManager.Instance.StartStory(openDialogue);

        InventorySelection.Instance.ClearSelection();

        isOpened = true;

        // Delay per effetto visivo
        Invoke(nameof(OpenChestVisual), openDelay);
    }

    void OpenChestVisual()
    {
        // Cambio background
        if (backgroundClosed != null) backgroundClosed.SetActive(false);
        if (backgroundOpen != null) backgroundOpen.SetActive(true);

        // Attiva oggetto nascosto
        if (hiddenObject != null)
            hiddenObject.SetActive(true);

        Debug.Log("Cassa aperta!");
    }
}