using UnityEngine;

public class PersistentUI : MonoBehaviour
{
    private static bool alreadyExists = false;

    private void Awake()
    {
        // Se esiste gia' un'istanza, distrugge questo oggetto
        if (alreadyExists)
        {
            Destroy(gameObject);
            return;
        }

        // Segna che esiste gia' un'istanza
        alreadyExists = true;

        // Mantiene questo oggetto tra le scene
        DontDestroyOnLoad(gameObject);
    }
}