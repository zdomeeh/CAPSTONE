using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance;

    public bool hasReadLetter = false;
    public bool introPlayed = false;

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

    public void SetLetterRead()
    {
        // Segna che la lettera e' stata letta
        hasReadLetter = true;

        // Messaggio di debug
        Debug.Log("Lettera letta salvata in GameState");
    }

    public void ResetGameState()
    {
        // Resetta lo stato della lettera
        hasReadLetter = false;

        // Resetta lo stato dell'introduzione
        introPlayed = false;
    }
}