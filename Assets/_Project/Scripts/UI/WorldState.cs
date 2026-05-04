using System.Collections.Generic;
using UnityEngine;

public class WorldState : MonoBehaviour
{
    public static WorldState Instance;

    private HashSet<string> collectedItems = new HashSet<string>();
    private HashSet<string> worldFlags = new HashSet<string>();

    private void Awake()
    {
        // Evita piu' istanze
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

    public void MarkItemCollected(string itemId)
    {
        // Salva l'oggetto come raccolto
        if (!string.IsNullOrEmpty(itemId))
            collectedItems.Add(itemId);
    }

    public bool IsItemCollected(string itemId)
    {
        // Controlla se l'oggetto e' stato raccolto
        return !string.IsNullOrEmpty(itemId) && collectedItems.Contains(itemId);
    }

    public void SetFlag(string flagId)
    {
        // Imposta una flag nel mondo
        if (!string.IsNullOrEmpty(flagId))
            worldFlags.Add(flagId);
    }

    public bool HasFlag(string flagId)
    {
        // Controlla se una flag e' attiva
        return !string.IsNullOrEmpty(flagId) && worldFlags.Contains(flagId);
    }

    public void ResetWorldState()
    {
        // Pulisce tutti gli oggetti raccolti
        collectedItems.Clear();

        // Pulisce tutte le flag del mondo
        worldFlags.Clear();
    }
}