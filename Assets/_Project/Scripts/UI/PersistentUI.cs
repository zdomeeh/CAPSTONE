using UnityEngine;

public class PersistentUI : MonoBehaviour
{
    private static bool alreadyExists = false;

    private void Awake()
    {
        if (alreadyExists)
        {
            Destroy(gameObject);
            return;
        }

        alreadyExists = true;
        DontDestroyOnLoad(gameObject);
    }
}