using UnityEngine;

public class GameResetManager : MonoBehaviour
{
    public static GameResetManager Instance;

    private void Awake()
    {
        // Evita la presenza di piu' istanze
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Imposta questa come istanza principale
        Instance = this;

        // Mantiene questo oggetto tra le scene
        DontDestroyOnLoad(gameObject);
    }

    public void ResetGame()
    {
        // Resetta lo stato generale del gioco
        if (GameState.Instance != null)
            GameState.Instance.ResetGameState();

        // Resetta lo stato del mondo
        if (WorldState.Instance != null)
            WorldState.Instance.ResetWorldState();

        // Svuota l'inventario
        if (Inventory.Instance != null)
            Inventory.Instance.ResetInventory();

        // Resetta la selezione degli oggetti
        if (InventorySelection.Instance != null)
            InventorySelection.Instance.ResetSelection();

        // Sblocca eventuali input bloccati
        InkManager.IsInputBlocked = false;

        // Ripristina il tempo normale
        Time.timeScale = 1f;
    }
}