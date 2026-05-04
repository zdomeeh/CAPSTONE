using UnityEngine;

public class InventorySelection : MonoBehaviour
{
    public static InventorySelection Instance;

    public string selectedItem;

    void Awake()
    {
        // Se non esiste un'istanza, la imposta
        if (Instance == null)
        {
            Instance = this;

            // Mantiene questo oggetto tra le scene
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Evita duplicati
            Destroy(gameObject);
        }
    }

    public void SelectItem(string itemName)
    {
        // Imposta l'oggetto selezionato
        selectedItem = itemName;

        // Messaggio di debug
        Debug.Log("Selezionato: " + itemName);
    }

    public void ClearSelection()
    {
        // Rimuove la selezione corrente
        selectedItem = null;
    }

    public void ResetSelection()
    {
        // Resetta completamente la selezione
        selectedItem = null;
    }
}