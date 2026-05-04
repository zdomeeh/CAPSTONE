using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource audioSource;

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

        // Mantiene la musica tra le scene
        DontDestroyOnLoad(gameObject);

        // Prende il componente AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Se esiste un AudioSource e non sta gia' suonando
        if (audioSource != null && !audioSource.isPlaying)
        {
            // Attiva il loop
            audioSource.loop = true;

            // Avvia la musica
            audioSource.Play();
        }
    }
}