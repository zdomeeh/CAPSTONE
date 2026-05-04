using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance;

    public bool hasReadLetter = false;
    public bool introPlayed = false;

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

    public void SetLetterRead()
    {
        hasReadLetter = true;
        Debug.Log("Lettera letta salvata in GameState");
    }
}