using System.Collections.Generic;
using UnityEngine;

public class WorldState : MonoBehaviour
{
    public static WorldState Instance;

    private HashSet<string> collectedItems = new HashSet<string>();
    private HashSet<string> worldFlags = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void MarkItemCollected(string itemId)
    {
        if (!string.IsNullOrEmpty(itemId))
            collectedItems.Add(itemId);
    }

    public bool IsItemCollected(string itemId)
    {
        return !string.IsNullOrEmpty(itemId) && collectedItems.Contains(itemId);
    }

    public void SetFlag(string flagId)
    {
        if (!string.IsNullOrEmpty(flagId))
            worldFlags.Add(flagId);
    }

    public bool HasFlag(string flagId)
    {
        return !string.IsNullOrEmpty(flagId) && worldFlags.Contains(flagId);
    }

    public void ResetWorldState()
    {
        collectedItems.Clear();
        worldFlags.Clear();
    }
}