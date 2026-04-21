using UnityEngine;

public class InventorySelection : MonoBehaviour
{
    public static InventorySelection Instance;

    public string selectedItem;

    private void Awake()
    {
        Instance = this;
    }

    public void SelectItem(string itemName)
    {
        selectedItem = itemName;
        Debug.Log("Selezionato: " + itemName);
    }

    public void ClearSelection()
    {
        selectedItem = null;
    }
}
