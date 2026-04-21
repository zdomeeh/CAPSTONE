using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Interactable
{
    public string requiredItem = "Key";

    public override void Interact()
    {
        string selected = InventorySelection.Instance.selectedItem;

        if (selected == requiredItem)
        {
            Debug.Log("Hai usato la chiave!");
            OpenDoor();

            InventorySelection.Instance.ClearSelection();
        }
        else
        {
            Debug.Log("Non funziona...");
        }
    }

    void OpenDoor()
    {
        Debug.Log("Porta aperta!");

        SceneManager.LoadScene("Lvl1");
    }
}
