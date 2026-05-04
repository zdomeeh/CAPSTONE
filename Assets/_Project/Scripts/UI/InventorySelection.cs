using UnityEngine;

public class InventorySelection : MonoBehaviour
{
    public static InventorySelection Instance;

    public string selectedItem;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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

    public void ResetSelection()
    {
        selectedItem = null;
    }
}
